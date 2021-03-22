using Grpc.Core;
using Hermes.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Hermes.Services
{
    [Authorize(Policy = "secureHermes")]
    public class ChatterService : Chatter.ChatterBase
    {
        private readonly ILogger<ChatterService> _logger;

        private static readonly ConcurrentDictionary<string, IServerStreamWriter<ChatReply>> ChatSubscriptions =
            new();

        public ChatterService(ILogger<ChatterService> logger)
        {
            _logger = logger;
        }

        public override async Task Chat(IAsyncStreamReader<SendRequest> requestStream,
            IServerStreamWriter<ChatReply> responseStream, ServerCallContext context)
        {
            var name = context.RequestHeaders.FirstOrDefault(x => x.Key.Equals("name"))?.Value;

            var connectionId = context.GetHttpContext().Connection.Id;
            _logger.LogInformation($"Connection id: {connectionId}");
            if (!ChatSubscriptions.ContainsKey(connectionId))
                ChatSubscriptions.TryAdd(connectionId, responseStream);

            _logger.LogInformation($"{name} connected");

            try
            {
                while (await requestStream.MoveNext())
                {
                    if (string.IsNullOrEmpty(requestStream.Current.Message)) continue;
                    foreach (var chatSubscription in ChatSubscriptions.Values)
                    {
                        await chatSubscription.WriteAsync(new ChatReply
                        {
                            Message = $"{DateTime.Now} -  {name} :  {requestStream.Current.Message}"
                        });
                    }
                }
            }
            catch (IOException)
            {
                _logger.LogInformation($"Connection for {name} was aborted.");
            }

        }
    }
}
