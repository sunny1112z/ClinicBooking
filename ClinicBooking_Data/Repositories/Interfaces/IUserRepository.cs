using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBooking.Entities;
namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<User?> GetByEmailAsync(string email);
        Task SaveResetTokenAsync(int userId, string token, DateTime expiry);
        Task<User?> GetByResetTokenAsync(string token);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
    }
}
