using ClinicBooking_Data.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicBooking_Data.Repositories.Interfaces;
using ClinicBooking.Entities;
namespace ClinicBooking_Service
{
   public  class DepartmentsService
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentsService(IDepartmentRepository departmentRepository)
        {
            this._departmentRepository = departmentRepository;
        }
         public async Task<IEnumerable<Department>> GetAllClinicsAsync()
        {
            return await  _departmentRepository.GetAllAsync();
        }

        public async Task<Department> GetClinicByIdAsync(int id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        public async Task AddClinicAsync(Department clinic)
        {
            await _departmentRepository.AddAsync(clinic);
        }

        public async Task UpdateClinicAsync(Department clinic)
        {
            await _departmentRepository.UpdateAsync(clinic);
        }

        public async Task DeleteClinicAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

    }
}