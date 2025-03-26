using ClinicBooking.Entities;

using System.Threading.Tasks;

namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IRoleRepository 
    {
        Task<Role?> GetByIdAsync(int id);
    }
}
