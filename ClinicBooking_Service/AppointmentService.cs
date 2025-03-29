using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicBooking.Entities;
using ClinicBooking.Repositories;
using ClinicBooking_Data.Repositories.Interfaces;

namespace ClinicBooking.Services
{
    public class AppointmentService 
    {
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository)
        {
            _appointmentRepository = appointmentRepository;
        }
        public async Task<bool> CheckAppointmentExistsAsync(int doctorId, DateTime selectedDate, TimeSpan selectedTime)
        {
            return await _appointmentRepository.CheckAppointmentExistsAsync(doctorId, selectedDate, selectedTime);
        }
        public async Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return await _appointmentRepository.GetAllAsync();
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return await _appointmentRepository.GetByIdAsync(id);
        }

        public async Task<bool> CreateAppointmentAsync(Appointment appointment)
        {
            await _appointmentRepository.AddAsync(appointment);
            return true;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            var existingAppointment = await _appointmentRepository.GetByIdAsync(appointment.AppointmentId);
            if (existingAppointment == null) return false;

            existingAppointment.Subject = appointment.Subject;
            existingAppointment.Description = appointment.Description;
            existingAppointment.StartTime = appointment.StartTime;
            existingAppointment.EndTime = appointment.EndTime;
            existingAppointment.Status = appointment.Status;
            existingAppointment.ZoomInfo = appointment.ZoomInfo;
            existingAppointment.MedicalResult = appointment.MedicalResult;
            existingAppointment.UserId = appointment.UserId;
            existingAppointment.DoctorId = appointment.DoctorId;

            await _appointmentRepository.UpdateAsync(existingAppointment);
            return true;
        }

        public async Task<bool> DeleteAppointmentAsync(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null) return false;

            await _appointmentRepository.DeleteAsync(id);
            return true;
        }
    }
}
