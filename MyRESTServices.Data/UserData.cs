using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyRESTServices.Data.Interfaces;
using MyRESTServices.Domain.Models;

namespace MyRESTServices.Data.Implementations
{
    public class UserData : IUserData
    {
        private readonly AppDbContext _context;

        public UserData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllWithRoles()
        {
            return await _context.Users.Include(u => u.Roles).ToListAsync();
        }

        public async Task<User> GetUserWithRoles(string username)
        {
            return await _context.Users.Include(u => u.Roles)
                                        .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> Login(string username, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }

        public async Task ChangePassword(string username, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user != null)
            {
                user.Password = newPassword;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("User not found.");
            }
        }
    }
}
