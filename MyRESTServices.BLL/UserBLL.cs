using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyRESTServices.BusinessLogic.Interfaces;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.BusinessLogic.Implementations
{
    public class UserBLL : IUserBLL
    {
        private readonly IUserData _userData;

        public UserBLL(IUserData userData)
        {
            _userData = userData;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userData.GetAllWithRoles();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _userData.GetUserWithRoles(username);
        }

        public async Task<User> Login(string username, string password)
        {
            return await _userData.Login(username, password);
        }

        public async Task<bool> ChangePassword(string username, string newPassword)
        {
            await _userData.ChangePassword(username, newPassword);
            return true;
        }

        public async Task<User> RegisterUser(User newUser)
        {
            throw new NotImplementedException();
        }
    }
}
