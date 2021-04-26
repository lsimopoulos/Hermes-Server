using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Test;

namespace Hermes.Classes
{
    public class UsersManagers
    {
        private readonly CryptoHelper _cryptoHelper;

        public UsersManagers(CryptoHelper cryptoHelper)
        {
            _cryptoHelper = cryptoHelper;
        }
        private readonly ConcurrentDictionary<string, TestUser> _registeredUsers = new();

        /// <summary>
        ///     Get users.
        /// </summary>
        public List<TestUser> GetUsers()
        {
            return _registeredUsers.Values.ToList();
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user"></param>
        public bool AddUser(TestUser user)
        {
            user.SubjectId = Guid.NewGuid().ToString();
            user.Password = _cryptoHelper.Encrypt(user.Password);
            return _registeredUsers.TryAdd(user.SubjectId, user);
        }
        
    }
}
