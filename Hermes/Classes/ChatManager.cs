using Grpc.Core;
using Hermes.Context;
using Hermes.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Classes
{
    public class ChatManager
    {
        //private readonly ConcurrentDictionary<Guid, ConcurrentQueue<ChatReply>> OfflineChatMessages = new ConcurrentDictionary<Guid, ConcurrentQueue<ChatReply>>();
        private readonly ConcurrentDictionary<Guid, HermesQueue<ChatReply>> ChatActiveClientsForMessages = new ConcurrentDictionary<Guid, HermesQueue<ChatReply>>();
        private readonly ConcurrentDictionary<Guid, HermesQueue<ChatStatus>> ChatActiveClientsForStatus = new ConcurrentDictionary<Guid, HermesQueue<ChatStatus>>();

        private readonly IServiceProvider _services;

        public ChatManager(IServiceProvider services)
        {
            _services = services;
        }


        //private async Task SendGroupMessage(ChatReply message)
        //{
        //    var receivers = ChatSubscriptions.Where(x => !string.Equals(x.Key, message.From)).Select(x => x.Value);
        //    var parallelOpts = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 1.0)) };
        //    await Parallel.ForEachAsync(receivers, parallelOpts, async (reciepient, token) => await reciepient.WriteAsync(message));
        //}
        public async Task SuscribeForMessages(Guid userId, IServerStreamWriter<ChatReply> streamingChannel)
        {
            var hq = new HermesQueue<ChatReply>(streamingChannel);

            ChatActiveClientsForMessages.TryAdd(userId, hq);
            await FetchOfflineMessagesFromDb(userId, hq);
            //OfflineChatMessages.TryRemove(userId, out _);
            PrintActiveThreads();
        }

        private async Task FetchOfflineMessagesFromDb(Guid userId, HermesQueue<ChatReply> hermesUserQueue)
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<HermesContext>();
            var offlineMessages = context.HermesMessages
                .Where(x => x.ReceiverId == userId && !x.Delivered)
                .OrderBy(x => x.GroupId)
                .ThenBy(x => x.Time)
                .ToImmutableList();
            if (offlineMessages != null)
            {
                foreach (var message in offlineMessages)
                {
                    var chatReply = new ChatReply
                    {
                        From = message.SenderId.ToString(),
                        Time = message.Time.ToString(),
                        Message = message.Message,
                        Groupid = message.GroupId?.ToString(),
                        To = message.ReceiverId.ToString()
                    };
                    message.Delivered = true;
                    hermesUserQueue.Enqueue(chatReply);
                }
                await context.SaveChangesAsync();
            }
        }
        public async Task SuscribeForStatus(Guid userId, IServerStreamWriter<ChatStatus> streamingChannel)
        {
            var hq = new HermesQueue<ChatStatus>(streamingChannel);
            ChatActiveClientsForStatus.TryAdd(userId, hq);
            var chatStatus = new ChatStatus {  IsOnline = true, From = userId.ToString() };
            await AddStatusAsync(chatStatus);
            PrintActiveThreads();
        }
        private static void PrintActiveThreads()
        {
            int max;
            ThreadPool.GetMaxThreads(out max, out _);
            int availableThreads;
            ThreadPool.GetAvailableThreads(out availableThreads, out _);
            int running = max - availableThreads;
            Console.WriteLine($"the running threads : {running} and the available {availableThreads}");
        }

        public void UnSuscribe(Guid userId)
        {
            ChatActiveClientsForStatus.TryRemove(userId, out var sq);
            ChatActiveClientsForMessages.TryRemove(userId, out var hq);
            
            if (hq != null && sq != null)
            {
                hq.Dispose();
                sq.Dispose();
            }

            Console.WriteLine($"Active cliennts: {ChatActiveClientsForMessages.Count}");
            //OfflineChatMessages.TryAdd(userId, new ConcurrentQueue<ChatReply>());
            PrintActiveThreads();

        }

        public async Task AddMessageAsync(ChatReply chatReply)
        {
            if (Guid.TryParse(chatReply.To, out var receiverGuid) && Guid.TryParse(chatReply.From, out var fromGuid))
            {
                using (var scope = _services.CreateScope())
                {
                    var databaseMessageWritter = scope.ServiceProvider.GetRequiredService<DatabaseMessageWritter>();
                    if (ChatActiveClientsForMessages.TryGetValue(receiverGuid, out HermesQueue<ChatReply> receiver))
                    {
                        receiver.Enqueue(chatReply);
                        await Task.Run(() => databaseMessageWritter.SaveMessage(chatReply, fromGuid));
                    }
                    else
                    {
                        //if (!OfflineChatMessages.ContainsKey(receiverGuid))
                        //    OfflineChatMessages.TryAdd(receiverGuid, new ConcurrentQueue<ChatReply>());

                        await Task.Run(() => databaseMessageWritter.SaveMessage(chatReply, fromGuid, false));
                        //OfflineChatMessages[receiverGuid].Enqueue(chatReply);

                    }
                }
                PrintActiveThreads();
            }


        }
        /// <summary>
        /// Add status async.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="chatStatus"></param>
        public async Task AddStatusAsync(ChatStatus chatStatus)
        {
            var userId = Guid.Parse(chatStatus.From);
            if (chatStatus.HasIsTyping && chatStatus.IsTyping && Guid.TryParse(chatStatus.To, out var toGuid) && ChatActiveClientsForStatus.ContainsKey(toGuid))
            {
                if (ChatActiveClientsForStatus.TryGetValue(toGuid, out HermesQueue<ChatStatus> receiver))
                {
                    receiver.Enqueue(chatStatus);
                }
            }
            if (chatStatus.HasIsOnline)
            {
                using var scope = _services.CreateScope();
                var currentUser = await scope.ServiceProvider.GetRequiredService<HermesContext>()
                    .Users
                    .Include(u => u.Contacts)
                    .AsNoTracking()
                    .Where(u => u.Id == userId)
                    .FirstOrDefaultAsync();
                foreach (var contact in currentUser.Contacts)
                {
                    if (contact.Id != userId && !contact.IsGroup)
                    {
                        chatStatus.To = contact.Id.ToString();
                        if (ChatActiveClientsForStatus.ContainsKey(contact.Id) && ChatActiveClientsForStatus.TryGetValue(contact.Id, out HermesQueue<ChatStatus> receiver))
                        {
                            receiver.Enqueue(chatStatus);
                        }
                    }

                }
            }
        }
    }
}
