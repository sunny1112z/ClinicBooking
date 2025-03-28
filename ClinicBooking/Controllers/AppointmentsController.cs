using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicBooking.Services;
using ClinicBooking.Entities;
using ClinicBooking_Service;

namespace ClinicBooking.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly DepartmentsService _departmentsService;
        private readonly DoctorService _doctorService;
        private readonly WorkScheduleService _workScheduleService;
        public AppointmentsController(AppointmentService appointmentService, DepartmentsService departmentsService, DoctorService doctorService, WorkScheduleService workScheduleService)
        {
            _appointmentService = appointmentService;
            _departmentsService = departmentsService;
            _doctorService = doctorService;
            _workScheduleService = workScheduleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsByDepartment(int departmentId)
        {
            var doctors = await _doctorService.GetDoctorsByDepartmentIdAsync(departmentId);
            return Json(doctors);
        }

        [HttpGet]
        public async Task<IActionResult> BookDoctor(int? departmentId)
        {
            var departments = await _departmentsService.GetAllClinicsAsync();
            ViewBag.Departments = departments;

            if (departmentId.HasValue)
            {
                var doctors = await _doctorService.GetDoctorsByDepartmentIdAsync(departmentId.Value);
                ViewBag.Doctors = doctors;
            }

            return View("BookDoctor");
        }
        public async Task<IActionResult> DoctorSchedule(int doctorId, DateTime? selectedDate)
        {
            DateTime searchDate = selectedDate ?? DateTime.Today;

            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            var department = await _departmentsService.GetClinicByIdAsync(doctorId);
            var schedule = await _workScheduleService.GetDoctorScheduleAsync(doctorId, searchDate) ?? new List<WorkSchedule>();

            // Chia theo buổi sáng, chiều, tối
            var morningSlots = schedule.Where(s => s.StartTime.Hours < 12).ToList();
            var afternoonSlots = schedule.Where(s => s.StartTime.Hours >= 12 && s.StartTime.Hours < 18).ToList();
            var eveningSlots = schedule.Where(s => s.StartTime.Hours >= 18).ToList();

            ViewBag.Doctor = doctor;
            ViewBag.Department = department;
            ViewBag.SelectedDate = searchDate;
            ViewBag.MorningSlots = morningSlots;
            ViewBag.AfternoonSlots = afternoonSlots;
            ViewBag.EveningSlots = eveningSlots;

            return View();
        }




    }
}
