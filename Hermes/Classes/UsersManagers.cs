using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hermes.Context;
using Hermes.Models;
using Hermes.Protos;
using IdentityServer4.Test;
using Microsoft.EntityFrameworkCore;

namespace Hermes.Classes
{
    public class UsersManagers
    {
        private readonly CryptoHelper _cryptoHelper;
        private readonly HermesContext _hermesContext;

        public UsersManagers(CryptoHelper cryptoHelper, HermesContext hermesContext)
        {
            _cryptoHelper = cryptoHelper;
            _hermesContext = hermesContext;
        }


        /// <summary>
        /// Get the contacts of a loggedin user.
        /// </summary>
        /// <param name="userId">the user's id</param>
        public async Task<IEnumerable<Contact>> GetContactsAsync(string userId)
        {
            var ext_id = Guid.Parse(userId);

            var contacts = new List<Contact>();
            var uscon = await _hermesContext.Users
                .Include(x => x.Contacts)
                .AsNoTracking()
                .Where(x => x.Id == ext_id)?
                .SelectMany(x => x.Contacts)
                .ToListAsync();
            uscon = uscon.OrderByDescending(x => x.Id == ext_id)
                    .ThenBy(u => u.Name).ToList();
            if (uscon == null || !uscon.Any())
                return new List<Contact>();

            foreach (var user in uscon)
            {
                contacts.Add(new Contact() { Id = user.Id.ToString(), Name = user.Name, Email = user.UserName });
            }
            return contacts;
        }

        /// <summary>
        /// Add a contact.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        public async Task<Contact> AddContactAsync(Guid userId, AddContactRequest acr, CancellationToken cancellationToken)
        {
            var loweredEmail = acr.Email.ToLowerInvariant();
            var newContact = await _hermesContext.Users.FindAsync(loweredEmail, cancellationToken);
            var currentUser = _hermesContext.Users.FirstOrDefault(x => x.Id == userId);
            if (newContact == null || currentUser == null)
                return null;
            currentUser.Contacts.Add(newContact);
            newContact.Contacts.Add(currentUser);
            await _hermesContext.SaveChangesAsync(cancellationToken);
            return new Contact { Id = newContact.Id.ToString(), Name = newContact.Name, Email = newContact.UserName, IsGroup = false };


        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user"></param>
        public async Task<bool> AddUserAsync(HermesUser user, CancellationToken cancellationToken)
        {
            if (CheckIfEmailExists(user.UserName))
                return false;
            user.Password = _cryptoHelper.Encrypt(user.Password);
            user.Contacts.Add(user);
            await _hermesContext.Users.AddAsync(user, cancellationToken);
            return await _hermesContext.SaveChangesAsync(cancellationToken) > 0;
        }
        /// <summary>
        /// Checks if id belongs to a group.
        /// </summary>
        /// <param name="id"></param>
        public async Task<bool> CheckIfGroupAsync(string id)
        {
            if (Guid.TryParse(id, out var parsedId))
            {
                return await _hermesContext.Users.AsNoTracking()
                    .AnyAsync(x => x.Id == parsedId && x.IsGroup);
            }
            return false;
        }

        public List<ChatReply> GetMessagesForGroup(SendRequest sendRequest)
        {
            var result = new List<ChatReply>();

            if (Guid.TryParse(sendRequest.To, out var groupId))
            {
                var senderId = _hermesContext.Users.AsNoTracking().Where(x => x.Id == Guid.Parse(sendRequest.From)).FirstOrDefault()?.Id;
                var groupMembers = _hermesContext.Users.Include(u => u.Contacts).Where(x => x.Id == groupId && x.IsGroup).ToList();
                foreach (var member in groupMembers)
                {
                    if (senderId != null && member.Id == senderId)
                        continue;
                    result.Add(new ChatReply { Message = sendRequest.Message, Time = sendRequest.Time, From = sendRequest.From, To = member.Id.ToString(), Groupid = sendRequest.To });
                }
            }
            return result;
        }

        public bool TryAddGroup(AddGroupRequest addGroupRequest, Guid userId, out Contact group)
        {
            var hu = new HermesUser
            {
                IsGroup = true,
                Name = addGroupRequest.Name,
                //GroupOwnerId= userId,
            };
            hu.UserName = $"{hu.Id}@{addGroupRequest.Name}.com";
            foreach (var member in addGroupRequest.Members)
            {
                if (Guid.TryParse(member.Id, out var parsedId))
                {
                    var currentMember = _hermesContext.Users.Where(u => u.Id == parsedId).FirstOrDefault();
                    if (currentMember != null)
                    {
                        hu.Contacts.Add(currentMember);
                        currentMember.Groups.Add(hu);
                        currentMember.Contacts.Add(hu);
                    }
                }
            }
            _hermesContext.Users.Add(hu);

            group = new Contact { Id = hu.Id.ToString(), Name = hu.Name, Email = hu.UserName, IsGroup = true };
            return  _hermesContext.SaveChanges() > 0;
        }

        /// <summary>
        /// Check if the email exists.
        /// </summary>
        /// <param name="email"></param>
        private bool CheckIfEmailExists(string email)
        {
            return _hermesContext.Users.Any(x => string.Equals(x.UserName, email, StringComparison.OrdinalIgnoreCase));
        }

    }
}
