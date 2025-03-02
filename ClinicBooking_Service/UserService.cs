using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using ClinicBooking.Entities;
using ClinicBooking_Data.Repositories.Interfaces;
namespace ClinicBooking_Service
{
    internal class UserService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            if (user != null && user.VerifyPassword(password))
            {
                return user;
            }
            return null;
        }
    }       
}

