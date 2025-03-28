using ClinicBooking.Entities;
using ClinicBooking_Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking_Data.Repositories.Implementations
{
    public class WorkScheduleRepository : IWorkScheduleRepository
    {
        private readonly ClinicBookingContext _context;

        public WorkScheduleRepository(ClinicBookingContext context)
        {
            _context = context;
        }
        public async Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId)
        {
            return await _context.WorkSchedules
                                 .Where(s => s.DoctorID == doctorId)
                                 .OrderBy(s => s.WorkDate)
                                 .ThenBy(s => s.StartTime) .ThenBy(e =>e.EndTime)
                                 .ToListAsync();
        }
        public async Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId, DateTime selectedDate)
        {
            return await _context.WorkSchedules
                .Where(s => s.DoctorID == doctorId && s.WorkDate.Date == selectedDate.Date)
                .OrderBy(s => s.StartTime)
                .ToListAsync();
        }
      
        public async Task<IEnumerable<WorkSchedule>> GetAllAsync()
        {
            return await _context.WorkSchedules
                .Include(ws => ws.Doctor)
                .Include(ws => ws.User)
                .ToListAsync();
        }

        public async Task<WorkSchedule?> GetByIdAsync(int id)
        {
            return await _context.WorkSchedules
                .Include(ws => ws.Doctor)
                .Include(ws => ws.User)
                .FirstOrDefaultAsync(ws => ws.ScheduleID == id);
        }

        public async Task AddAsync(WorkSchedule schedule)
        {
            _context.WorkSchedules.Add(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WorkSchedule schedule)
        {
            _context.WorkSchedules.Update(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var schedule = await _context.WorkSchedules.FindAsync(id);
            if (schedule != null)
            {
                _context.WorkSchedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
        }
    }
}
