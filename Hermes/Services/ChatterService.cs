using Grpc.Core;
using Hermes.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;

namespace Hermes.Services
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class ChatterService : Chatter.ChatterBase
    {
        private readonly ILogger<ChatterService> _logger;

        private static readonly ConcurrentDictionary<string, IServerStreamWriter<ChatReply>> ChatSubscriptions =
            new();

        public ChatterService(ILogger<ChatterService> logger)
        {
            _logger = logger;
        }

        public override async Task chat(SendRequest sendRequest,
            IServerStreamWriter<ChatReply> responseStream, ServerCallContext context)
        {
            var name = context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var connectionId = context.GetHttpContext().Connection.Id;
            var chatReply = new ChatReply { Message = sendRequest.Message,Time = sendRequest.Time,Name = name, Sent = false };

            //_logger.LogInformation($"Connection id: {connectionId}");
            if (!ChatSubscriptions.ContainsKey(name))
                ChatSubscriptions.TryAdd(name, responseStream);
            //else if(ChatSubscriptions.ContainsKey(connectionId) && ChatSubscriptions[connectionId].Item2.Equals(name))
            //{
            //    ChatSubscriptions[connectionId] = (responseStream,name);
            //}

            _logger.LogInformation($"{name} connected");

            try
            {
                while (!context.CancellationToken.IsCancellationRequested)
                {
                    if (string.IsNullOrEmpty(sendRequest.Message)) continue;
                    foreach (var chatSubscription in ChatSubscriptions)
                    {

                        if (!chatSubscription.Key.Equals(name) && !chatReply.Sent)
                        {
                           
                            //for debugging reasons TODO remove it
                            _logger.LogInformation($" the user:  {name} sent the message : {sendRequest.Message}");
                            if(!chatReply.Sent )
                            {
                                await chatSubscription.Value.WriteAsync(chatReply);
                                chatReply.Sent = true;
                            }

                        }

                    }
                }
                ChatSubscriptions.TryRemove(name,out _);

            }
            catch (IOException)
            {
                _logger.LogInformation($"Connection for {name} was aborted.");
            }

        }



        //public override async Task streamChat(IAsyncStreamReader<SendRequest> requestStream,
        //    IServerStreamWriter<ChatReply> responseStream, ServerCallContext context)
        //{
        //    var name = context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        //    var connectionId = context.GetHttpContext().Connection.Id;
        //    _logger.LogInformation($"Connection id: {connectionId}");
        //    if (!ChatSubscriptions.ContainsKey(connectionId))
        //        ChatSubscriptions.TryAdd(connectionId, (responseStream,name));

        //    _logger.LogInformation($"{name} connected");

        //    try
        //    {
        //        while (await requestStream.MoveNext())
        //        {
        //            if (string.IsNullOrEmpty(requestStream.Current.Message)) continue;
        //            foreach (var chatSubscription in ChatSubscriptions)
        //            {

        //                //for debugging reasons TODO remove it
        //                _logger.LogInformation($" the user:  {name} sent the message : {requestStream.Current.Message}");

        //                if (chatSubscription.Key != connectionId)
        //                    await chatSubscription.Value.Item1.WriteAsync(new ChatReply
        //                    {
        //                        Message = $"{DateTime.Now} -  {name} :  {requestStream.Current.Message}"
        //                    });
        //            }
        //        }
        //    }
        //    catch (IOException)
        //    {
        //        _logger.LogInformation($"Connection for {name} was aborted.");
        //    }

        //}
    }
}
