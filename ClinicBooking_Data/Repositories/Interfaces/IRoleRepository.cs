using ClinicBooking_Data.Entities;
using System.Threading.Tasks;

namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        Task<Role?> GetByIdAsync(int id);
    }
}
