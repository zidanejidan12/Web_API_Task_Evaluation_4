using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data.Interfaces
{
    public interface IRoleData
    {
        Task<Task> AddUserToRole(string username, int roleId);
    }
}
