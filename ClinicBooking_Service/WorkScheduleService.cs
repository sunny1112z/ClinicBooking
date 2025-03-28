using ClinicBooking.Entities;
using ClinicBooking_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking_Service
{
    public class WorkScheduleService 
    {
        private readonly IWorkScheduleRepository _workScheduleRepository;

        public WorkScheduleService(IWorkScheduleRepository workScheduleRepository)
        {
            _workScheduleRepository = workScheduleRepository;
        }
        public async Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId, DateTime? date = null, int? status = null)
        {
            return await _workScheduleRepository.GetDoctorScheduleAsync(doctorId);
        }
        public async Task<IEnumerable<WorkSchedule>> GetAllWorkSchedulesAsync()
        {
            return await _workScheduleRepository.GetAllAsync();
        }
        public async Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId, DateTime selectedDate)
        {
            return await _workScheduleRepository.GetDoctorScheduleAsync(doctorId, selectedDate);
        }
        public async Task<List<WorkSchedule>> GetDoctorScheduleAsync(int doctorId)
        {
            return await _workScheduleRepository.GetDoctorScheduleAsync(doctorId);
        }

        public async Task<WorkSchedule?> GetWorkScheduleByIdAsync(int id)
        {
            return await _workScheduleRepository.GetByIdAsync(id);
        }

        public async Task AddWorkScheduleAsync(WorkSchedule schedule)
        {
            await _workScheduleRepository.AddAsync(schedule);
        }

        public async Task UpdateWorkScheduleAsync(WorkSchedule schedule)
        {
            await _workScheduleRepository.UpdateAsync(schedule);
        }

        public async Task DeleteWorkScheduleAsync(int id)
        {
            await _workScheduleRepository.DeleteAsync(id);
        }
    }
}
