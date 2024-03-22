using System.Collections.Generic;
using System.Threading.Tasks;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data.Interfaces
{
    public interface IUserData
    {
        Task<IEnumerable<User>> GetAllWithRoles();
        Task<User> GetUserWithRoles(string username);
        Task<User> GetByUsername(string username);
        Task<User> Login(string username, string password);
        Task ChangePassword(string username, string newPassword);
    }
}
