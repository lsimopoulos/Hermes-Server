using Grpc.Core;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Classes
{
    public class HermesQueue<T> : IDisposable where T : new()
    {
        private readonly BlockingCollection<T> _queue;
        private readonly IServerStreamWriter<T> _receiver;
        private readonly CancellationTokenSource cts;

        public HermesQueue(IServerStreamWriter<T> receiver)
        {
            _queue = new BlockingCollection<T>();
            var thread = new Thread(new ThreadStart(ProcessQueue))
            {
                IsBackground = true
            };
            thread.Start();
            _receiver = receiver;
            cts = new CancellationTokenSource();

        }
        private void ProcessQueue()
        {
            foreach (var message in _queue.GetConsumingEnumerable())
            {
                SendMessage(message).GetAwaiter().GetResult();
            }
        }

        private async Task SendMessage(T message)
        {
            try
            {
                await _receiver.WriteAsync(message, cts.Token);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void Dispose()
        {
            _queue.Dispose();
            cts.Cancel();
            cts.Dispose();
            GC.SuppressFinalize(this);
        }

        public bool Enqueue(T message)
        {

            return _queue.TryAdd(message);
        }

    }
}

