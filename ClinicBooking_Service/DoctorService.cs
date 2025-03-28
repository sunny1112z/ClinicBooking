using ClinicBooking.Entities;
using ClinicBooking_Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicBooking_Service
{
    public class DoctorService 
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<Doctor>> GetAllDoctorsAsync()
        {
            return await _doctorRepository.GetAllAsync();
        }
        public async Task<List<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            return await _doctorRepository.GetDoctorsByDepartmentIdAsync(departmentId);
        }
        public async Task<Doctor?> GetDoctorByIdAsync(int id)
        {
            return await _doctorRepository.GetByIdAsync(id);
        }

        public async Task CreateDoctorAsync(Doctor doctor)
        {
            await _doctorRepository.AddAsync(doctor);
        }

        public async Task UpdateDoctorAsync(Doctor doctor)
        {
            await _doctorRepository.UpdateAsync(doctor);
        }

        public async Task DeleteDoctorAsync(int id)
        {
            await _doctorRepository.DeleteAsync(id);
        }
    }
}
