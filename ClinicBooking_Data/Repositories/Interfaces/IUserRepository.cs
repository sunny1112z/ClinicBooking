using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBooking.Entities;
using ClinicBooking_Data.Entities;
namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<Role?> GetRoleByIdAsync(int roleId);

    }
}
