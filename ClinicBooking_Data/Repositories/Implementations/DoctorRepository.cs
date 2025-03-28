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
    public class DoctorRepository : IDoctorRepository
    {
        private readonly ClinicBookingContext _context;

        public DoctorRepository(ClinicBookingContext context)
        {
            _context = context;
        }
        public async Task<List<Doctor>> GetDoctorsByDepartmentIdAsync(int departmentId)
        {
            return await _context.Doctors
                .Where(d => d.DepartmentId == departmentId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Doctor>> GetAllAsync()
        {
            return await _context.Doctors.Include(d => d.Department).ToListAsync();
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            return await _context.Doctors.Include(d => d.Department)
                                         .FirstOrDefaultAsync(d => d.DoctorId == id);
        }

        public async Task AddAsync(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor != null)
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
            }
        }
    }
}
