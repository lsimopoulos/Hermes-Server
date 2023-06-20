using Hermes.Context;
using Hermes.Models;
using Hermes.Protos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hermes.Classes
{
    public class UsersManagers
    {
        private readonly CryptoHelper _cryptoHelper;
        private readonly ConcurrentDictionary<Guid, bool> OnlineUsers = new ConcurrentDictionary<Guid, bool>();
        private readonly IServiceProvider _serviceProvider;

        public UsersManagers(CryptoHelper cryptoHelper, IServiceProvider serviceProvider)
        {
            _cryptoHelper = cryptoHelper;
            _serviceProvider = serviceProvider;
        }

        public bool AddUserOnlineStatus(Guid userId)
        {
            return OnlineUsers.TryAdd(userId, true);
        }
        //public bool IsUserOnline(Guid userId)
        //{
        //    return OnlineUsers.ContainsKey(userId);
        //}
        public bool RemoveUser(Guid userId)
        {
            return OnlineUsers.TryRemove(userId, out var _);
        }

        /// <summary>
        /// Get the contacts of a loggedin user.
        /// </summary>
        /// <param name="userId">the user's id</param>
        public async Task<IEnumerable<Contact>> GetContactsAsync(string userId)
        {
            using var scope = _serviceProvider.CreateScope();
            var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
            var ext_id = Guid.Parse(userId);

            var contacts = new List<Contact>();
            var uscon = await hermesContext.Users
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
                contacts.Add(new Contact() { Id = user.Id.ToString(), Name = user.Name, Email = user.UserName, IsOnline = OnlineUsers.ContainsKey(user.Id), IsGroup = user.IsGroup });
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
            using var scope = _serviceProvider.CreateScope();
            var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
            var loweredEmail = acr.Email.ToLowerInvariant();
            var newContact = await hermesContext.Users.FindAsync(new object[] { loweredEmail, cancellationToken }, cancellationToken: cancellationToken);
            var currentUser = hermesContext.Users.FirstOrDefault(x => x.Id == userId);
            if (newContact == null || currentUser == null)
                return null;
            currentUser.Contacts.Add(newContact);
            newContact.Contacts.Add(currentUser);
            await hermesContext.SaveChangesAsync(cancellationToken);
            return new Contact { Id = newContact.Id.ToString(), Name = newContact.Name, Email = newContact.UserName, IsGroup = false, IsOnline = OnlineUsers.ContainsKey(newContact.Id) };


        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user"></param>
        public async Task<bool> AddUserAsync(HermesUser user, CancellationToken cancellationToken)
        {
            if (CheckIfEmailExists(user.UserName))
                return false;
            using var scope = _serviceProvider.CreateScope();
            var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
            user.Password = _cryptoHelper.Encrypt(user.Password);
            user.Contacts.Add(user);
            await hermesContext.Users.AddAsync(user, cancellationToken);
            return await hermesContext.SaveChangesAsync(cancellationToken) > 0;
        }
        /// <summary>
        /// Checks if id belongs to a group.
        /// </summary>
        /// <param name="id"></param>
        public async Task<bool> CheckIfGroupAsync(string id)
        {
            if (Guid.TryParse(id, out var parsedId))
            {
                using var scope = _serviceProvider.CreateScope();
                var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
                return await hermesContext.Users.AsNoTracking()
                    .AnyAsync(x => x.Id == parsedId && x.IsGroup);
            }
            return false;
        }

        public async Task<List<ChatReply>> GetMessagesForGroup(SendRequest sendRequest)
        {
            var result = new List<ChatReply>();

            if (Guid.TryParse(sendRequest.To, out var groupId))
            {
                using var scope = _serviceProvider.CreateScope();
                var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
                var senderId =  (await hermesContext.Users.AsNoTracking().Where(x => x.Id == Guid.Parse(sendRequest.From)).FirstOrDefaultAsync())?.Id;
                var groupMembers = (await hermesContext.Users.AsNoTracking().Include(u => u.Contacts).Where(x => x.Id == groupId && x.IsGroup).FirstOrDefaultAsync())?.Contacts;
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
            using var scope = _serviceProvider.CreateScope();
            var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
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
                   
                    var currentMember = hermesContext.Users.Where(u => u.Id == parsedId).FirstOrDefault();
                    if (currentMember != null)
                    {
                        hu.Contacts.Add(currentMember);
                        currentMember.Groups.Add(hu);
                        currentMember.Contacts.Add(hu);
                    }
                }
            }
            hermesContext.Users.Add(hu);

            group = new Contact { Id = hu.Id.ToString(), Name = hu.Name, Email = hu.UserName, IsGroup = true };
            return hermesContext.SaveChanges() > 0;
        }

        /// <summary>
        /// Check if the email exists.
        /// </summary>
        /// <param name="email"></param>
        private bool CheckIfEmailExists(string email)
        {
            using var scope = _serviceProvider.CreateScope();
            var hermesContext = scope.ServiceProvider.GetRequiredService<HermesContext>();
            return hermesContext.Users.Any(x => string.Equals(x.UserName, email, StringComparison.OrdinalIgnoreCase));
        }

    }
}
