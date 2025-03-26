using ClinicBooking.Entities;
using ClinicBooking_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ClinicBooking_Data.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ClinicBookingContext _context;

        public RoleRepository(ClinicBookingContext context)
        {
            _context = context;
        }

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == id);
        }
    }
}
