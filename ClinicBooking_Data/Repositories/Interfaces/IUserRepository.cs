using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicBooking.Entities;

namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<Role?> GetRoleByIdAsync(int roleId);
        Task<User?> GetUserByEmailAsync(string email);

        Task<User?> GetByEmailAsync(string email);
        Task SaveResetTokenAsync(int userId, string token, DateTime expiry);
        Task<User?> GetByResetTokenAsync(string token);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int userId);
        Task<User?> GetUserByIdAsync(int id);
    }
}
