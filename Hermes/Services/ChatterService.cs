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
using Google.Protobuf.WellKnownTypes;
using Hermes.Classes;

namespace Hermes.Services
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class ChatterService : Chatter.ChatterBase
    {
        private readonly ILogger<ChatterService> _logger;
        private readonly ChatManager _chatManager;


        public ChatterService(ILogger<ChatterService> logger, ChatManager chatManager)
        {
            _logger = logger;
            _chatManager = chatManager;
        }

        public override Task connect(Empty request, IServerStreamWriter<ChatReply> responseStream, ServerCallContext context)
        {
            var name = context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;

            try
            {
                _chatManager.Suscribe(name, responseStream);
                while (!context.CancellationToken.IsCancellationRequested)
                {

                }
                _chatManager.UnSuscribe(name);

            }
            catch (Exception)
            {
                _chatManager.UnSuscribe(name);
            }
            return Task.CompletedTask;
        }
        public override Task<Empty> chat(SendRequest sendRequest, ServerCallContext context)
        {
            var name = context.GetHttpContext().User.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
            var chatReply = new ChatReply { Message = sendRequest.Message, Time = sendRequest.Time, From = name };
            _chatManager.AddMessage(chatReply);
            return Task.FromResult(new Empty());
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
