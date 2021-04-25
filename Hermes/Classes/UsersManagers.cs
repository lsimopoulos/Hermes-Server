using System;
using System.Collections.Generic;
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
        private readonly List<TestUser> _registeredUsers = new();

        /// <summary>
        ///     Get users.
        /// </summary>
        public  List<TestUser> GetUsers()
        {
            return _registeredUsers;
        }

        /// <summary>
        /// Add new user.
        /// </summary>
        /// <param name="user"></param>
        public void AddUser(TestUser user)
        {
            user.SubjectId = Guid.NewGuid().ToString();
            user.Password = _cryptoHelper.Encrypt(user.Password);
            _registeredUsers.Add(user);
        }

        public  void SeedDemoUsers()
        {
            AddUser(new TestUser{Username = "leo",Password = "password"});
            AddUser(new TestUser { Username = "test", Password = "pass" });
        }
    }
}
