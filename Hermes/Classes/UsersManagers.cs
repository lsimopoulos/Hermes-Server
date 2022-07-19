using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Hermes.Models;
using Hermes.Protos;
using IdentityServer4.Test;

namespace Hermes.Classes
{
    public class UsersManagers
    {
        private readonly CryptoHelper _cryptoHelper;
        private readonly ConcurrentDictionary<Guid, HermesUser> _registeredUsers = new();

        public UsersManagers(CryptoHelper cryptoHelper)
        {
            _cryptoHelper = cryptoHelper;
        }

        /// <summary>
        ///     Get users.
        /// </summary>
        public List<HermesUser> GetUsers()
        {
            return _registeredUsers.Values.ToList();
        }

        /// <summary>
        /// Get the contacts of a loggedin user.
        /// </summary>
        /// <param name="externalId">the user's external id</param>
        public IEnumerable<Contact> GetContacts(string externalId)
        {
            var ext_id = Guid.Parse(externalId);
            if (_registeredUsers.ContainsKey(ext_id))
            {
                var contacts = new List<Contact>();
                foreach (var user in _registeredUsers[ext_id].Contacts)
                {
                    contacts.Add(new Contact() { Id = user.ExternalId.ToString(), Name = user.Name, Email = user.Username });
                }
                return contacts;
            }

            return new List<Contact>();
        }

        /// <summary>
        /// Add a contact.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        public Contact AddContact(Guid userId, AddContactRequest acr)
        {
            var newContact = _registeredUsers.Values.Where(x => string.Equals(x.Username, acr.Email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (newContact == null)
                return null;
            if (_registeredUsers.ContainsKey(userId))
            {
                _registeredUsers[userId].Contacts.Add(newContact);
                _registeredUsers[newContact.ExternalId].Contacts.Add(_registeredUsers[userId]);
                return new Contact { Id = newContact.ExternalId.ToString(), Name = newContact.Name, Email = newContact.Username };
            }


            return null;
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user"></param>
        public bool AddUser(HermesUser user)
        {
            user.Password = _cryptoHelper.Encrypt(user.Password);
            user.Contacts.Add(new HermesUser(user));
               
            return _registeredUsers.TryAdd(user.ExternalId, user);
        }

    }
}
