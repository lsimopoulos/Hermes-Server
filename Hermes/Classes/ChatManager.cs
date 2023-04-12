using Grpc.Core;
using Hermes.Models;
using Hermes.Protos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Classes
{
    public class ChatManager
    {
        private readonly IServiceProvider _services;
        public ChatManager(IServiceProvider services)
        {
            _services = services;
        }
        private readonly ConcurrentDictionary<Guid, ConcurrentQueue<ChatReply>> OfflineChatMessages = new ConcurrentDictionary<Guid, ConcurrentQueue<ChatReply>>();
        private readonly ConcurrentDictionary<Guid, HermesQueue<ChatReply>> ChatActiveClients = new ConcurrentDictionary<Guid, HermesQueue<ChatReply>>();

        //private async Task SendGroupMessage(ChatReply message)
        //{
        //    var receivers = ChatSubscriptions.Where(x => !string.Equals(x.Key, message.From)).Select(x => x.Value);
        //    var parallelOpts = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 1.0)) };
        //    await Parallel.ForEachAsync(receivers, parallelOpts, async (reciepient, token) => await reciepient.WriteAsync(message));
        //}
        public void Suscribe(Guid userId, IServerStreamWriter<ChatReply> streamingChannel)
        {
            var hq = new HermesQueue<ChatReply>(streamingChannel);
            if (OfflineChatMessages.TryGetValue(userId, out ConcurrentQueue<ChatReply> value) && !value.IsEmpty)
            {
                foreach (var chatOfflineMessage in OfflineChatMessages[userId])
                {
                    hq.Enqueue(chatOfflineMessage);
                }

            }
            ChatActiveClients.TryAdd(userId, hq);
            OfflineChatMessages.TryRemove(userId, out _);
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
            ChatActiveClients.TryRemove(userId, out var hq);
            if (hq != null)
                hq.Dispose();
            Console.WriteLine($"Active cliennts: {ChatActiveClients.Count}");
            OfflineChatMessages.TryAdd(userId, new ConcurrentQueue<ChatReply>());
            PrintActiveThreads();

        }
        public async Task AddMessageAsync(ChatReply chatReply)
        {
            if (Guid.TryParse(chatReply.To, out var receiverGuid) && Guid.TryParse(chatReply.From, out var fromGuid))
            {
                using (var scope = _services.CreateScope())
                {
                    var databaseMessageWritter = scope.ServiceProvider.GetRequiredService<DatabaseMessageWritter>();
                    var hermesMessage = new HermesMessage();
                    await Task.Run(() => databaseMessageWritter.SaveMessage(chatReply,fromGuid));
                }
                if (ChatActiveClients.TryGetValue(receiverGuid, out HermesQueue<ChatReply> receiver))
                {
                    receiver.Enqueue(chatReply);
                }
                else
                {
                    if (!OfflineChatMessages.ContainsKey(receiverGuid))
                        OfflineChatMessages.TryAdd(receiverGuid, new ConcurrentQueue<ChatReply>());
                    OfflineChatMessages[receiverGuid].Enqueue(chatReply);
                }
                PrintActiveThreads();
            }


        }
    }
}
