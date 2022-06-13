using Grpc.Core;
using Hermes.Protos;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Classes
{
    public class ChatManager : IDisposable  
    {
        private readonly ConcurrentDictionary<string, IServerStreamWriter<ChatReply>> ChatSubscriptions = new ConcurrentDictionary<string, IServerStreamWriter<ChatReply>>();
        private readonly BlockingCollection<ChatReply> ChatMessages = new BlockingCollection<ChatReply>();
        public ChatManager()
        {
            var thread = new Thread(new ThreadStart(ProcessQueue));
            thread.IsBackground = true;
            thread.Start();
        }

        private void ProcessQueue()
        {
            foreach (var message in ChatMessages.GetConsumingEnumerable())
            {
                SendMessage(message).GetAwaiter().GetResult();
            }
        }

        private async Task SendMessage(ChatReply message)
        {
            var receivers = ChatSubscriptions.Where(x => !string.Equals(x.Key, message.From)).Select(x => x.Value);
            var parallelOpts = new ParallelOptions { MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling((Environment.ProcessorCount * 0.75) * 1.0)) };
            await Parallel.ForEachAsync(receivers, parallelOpts, async (reciepient, token) => await reciepient.WriteAsync(message));
        }
        public bool Suscribe(string name, IServerStreamWriter<ChatReply> streamingChannel)
        {
            return ChatSubscriptions.TryAdd(name, streamingChannel);
        }

        public bool UnSuscribe(string name)
        {
            return ChatSubscriptions.TryRemove(name, out _);
        }
        public void AddMessage(ChatReply chatReply)
        {
            if (!ChatMessages.IsAddingCompleted)
                ChatMessages.TryAdd(chatReply);
        }

        public void Dispose()
        {
            ChatMessages.CompleteAdding();
        }
    }
}
