using System.Collections.Generic;
using System.Threading.Tasks;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.BusinessLogic.Interfaces
{
    public interface IUserBLL
    {
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserByUsername(string username);
        Task<User> Login(string username, string password);
        Task<bool> ChangePassword(string username, string newPassword);
        Task<User> RegisterUser(User newUser);
    }
}
