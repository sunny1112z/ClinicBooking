using ClinicBooking.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IWorkScheduleRepository
    {
        Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId, DateTime selectedDate);
        Task<IEnumerable<WorkSchedule>> GetAllAsync();
        Task<WorkSchedule?> GetByIdAsync(int id);
        Task AddAsync(WorkSchedule schedule);
        Task UpdateAsync(WorkSchedule schedule);
        Task DeleteAsync(int id);
        Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId);

    }
}
