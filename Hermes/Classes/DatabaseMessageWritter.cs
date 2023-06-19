using Hermes.Context;
using Hermes.Models;
using Hermes.Protos;
using System;
using System.Threading.Tasks;

namespace Hermes.Classes
{
    public class DatabaseMessageWritter
    {
        private readonly HermesContext _hermesContext;

        public DatabaseMessageWritter(HermesContext hermesContext)
        {
            _hermesContext = hermesContext;
        }
        public async Task SaveMessage(ChatReply chatReply, Guid senderId, bool delivered = true)
        {
            var receiverId = (await _hermesContext.Users.FindAsync(Guid.Parse(chatReply.To))).Id;
            var hermesMessage = new HermesMessage { Message = chatReply.Message, Delivered = delivered, SenderId = senderId, GroupId = chatReply.HasGroupid ? Guid.Parse(chatReply.Groupid) : null, ReceiverId = receiverId, Time = DateTimeOffset.Parse(chatReply.Time) };
            _hermesContext.HermesMessages.Add(hermesMessage);
            await _hermesContext.SaveChangesAsync();
        }
    }
}
