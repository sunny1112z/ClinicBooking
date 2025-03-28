using Microsoft.AspNetCore.Mvc;

namespace ClinicBooking.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
 
    using ClinicBooking_Service;
    using global::ClinicBooking.Services;
    using global::ClinicBooking.Entities;

    namespace ClinicBooking.Controllers
    {
        public class AppointmentsController : Controller
        {
            private readonly AppointmentService _appointmentService;
            private readonly DepartmentsService _departmentsService;
            private readonly DoctorService _doctorService;
            public AppointmentsController(AppointmentService appointmentService, DepartmentsService departmentsService, DoctorService doctorService)
            {
                _appointmentService = appointmentService;
                _departmentsService = departmentsService;
                _doctorService = doctorService;
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
               

                return View("BookDoctor"); 
            }


        }
    }

}
