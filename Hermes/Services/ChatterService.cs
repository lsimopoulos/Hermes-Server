using Grpc.Core;
using Hermes.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Google.Protobuf.WellKnownTypes;
using Hermes.Classes;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Hermes.Services
{
    [Authorize(AuthenticationSchemes = IdentityServerAuthenticationDefaults.AuthenticationScheme)]
    public class ChatterService : Chatter.ChatterBase
    {
        private readonly ILogger<ChatterService> _logger;
        private readonly ChatManager _chatManager;
        private readonly UsersManagers _usersManager;

        public ChatterService(ILogger<ChatterService> logger, ChatManager chatManager, UsersManagers userManager)
        {
            _logger = logger;
            _chatManager = chatManager;
            _usersManager = userManager;
        }

        public override async Task connect(Empty request, IServerStreamWriter<ChatReply> responseStream, ServerCallContext context)
        {
            var hehe = context.GetHttpContext().User;
            if (Guid.TryParse(context.GetHttpContext().User.Claims.Where(static x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value, out var ext_id))
            {
                _chatManager.Suscribe(ext_id, responseStream);
              
                try
                {
                    await Task.Delay(-1, context.CancellationToken);
                }
                catch (TaskCanceledException)
                {
                    _chatManager.UnSuscribe(ext_id);
                }
                catch (Exception)
                {
                    _chatManager.UnSuscribe(ext_id);
                }
            }
        }
        public override Task<Contact> addGroup(AddGroupRequest request, ServerCallContext context)
        {
            if (_usersManager.TryAddGroup(request,Guid.NewGuid(), out var group))
            {
                return Task.FromResult(group);
            }


            throw new RpcException(new Status(StatusCode.Internal, "Group was not created"));
        }

        public override async Task<Empty> chat(SendRequest sendRequest, ServerCallContext context)
        {
            if (_usersManager.CheckIfGroup(sendRequest.To))
            {
                var messages = _usersManager.GetMessagesForGroup(sendRequest);
                foreach (var msg in messages)
                {
                    await _chatManager.AddMessage(msg);
                }
            }
            else
            {
                var chatReply = new ChatReply { Message = sendRequest.Message, Time = sendRequest.Time, From = sendRequest.From, To = sendRequest.To };
                await _chatManager.AddMessage(chatReply);
            }

            return new Empty();
        }
        public override Task<GetContactsReply> getContacts(Empty request, ServerCallContext context)
        {
            var externalId = context.GetHttpContext().User.Claims.Where(static x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            var contacts = _usersManager.GetContacts(externalId);
            var reply = new GetContactsReply();
            reply.Contacts.AddRange(contacts);
            return Task.FromResult(reply);
        }

        public override Task<Contact> addContact(AddContactRequest request, ServerCallContext context)
        {
            if (Guid.TryParse(context.GetHttpContext().User.Claims.Where(static x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value, out var externalId))
            {
                var ret = _usersManager.AddContact(externalId, request);
                if (ret == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, "Email was not found in the server"));
                }

                return Task.FromResult(ret);

            }
            return null;
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
