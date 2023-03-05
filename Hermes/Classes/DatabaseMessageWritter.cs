﻿using Hermes.Context;
using Hermes.Models;
using Hermes.Protos;
using System;
using System.Linq;
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
        public async Task SaveMessage(ChatReply chatReply, Guid senderId)
        {
            var receiverId = _hermesContext.Users.Where(x => string.Equals(x.UserName, chatReply.To)).Select(x => x.Id).FirstOrDefault();
            var hermesMessage = new HermesMessage { Message = chatReply.Message, SenderId = senderId, GroupId = chatReply.HasGroupid ? Guid.Parse(chatReply.Groupid) : null, ReceiverId = receiverId, Time = DateTimeOffset.Parse(chatReply.Time) };
            _hermesContext.HermesMessages.Add(hermesMessage);
            await _hermesContext.SaveChangesAsync();
        }
    }
}
