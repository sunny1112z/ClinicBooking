using ClinicBooking.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking_Data.Repositories.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<List<Appointment>> GetBookedAppointmentsAsync(int doctorId, DateTime date);
        Task<bool> CheckAppointmentExistsAsync(int doctorId, DateTime selectedDate, TimeSpan selectedTime);
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(int id);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
        Task DeleteAsync(int id);
        Task SaveAsync();
    }
}
