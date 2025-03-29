using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClinicBooking.Services;
using ClinicBooking.Entities;
using ClinicBooking_Service;
using System.Security.Claims;
using ClinicBooking.Models;

namespace ClinicBooking.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _appointmentService;
        private readonly DepartmentsService _departmentsService;
        private readonly DoctorService _doctorService;
        private readonly WorkScheduleService _workScheduleService;
        private readonly AccountService _accountService;
        private readonly EmailService _emailService;
        public AppointmentsController(AppointmentService appointmentService, DepartmentsService departmentsService, DoctorService doctorService, WorkScheduleService workScheduleService, AccountService accountService, EmailService emailService)
        {
            _appointmentService = appointmentService;
            _departmentsService = departmentsService;
            _doctorService = doctorService;
            _workScheduleService = workScheduleService;
            _accountService = accountService;
            _emailService = emailService;
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
        [HttpPost]

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DoctorSchedule(int doctorId, DateTime selectedDate, TimeSpan selectedTime)
        {
            // Kiểm tra đăng nhập
            //if (!User.Identity.IsAuthenticated)
            //{
            //    return RedirectToAction("Login", "Auth");
            //}

            var userId = "23";
                
            //    User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Console.WriteLine("UserId từ JWT: " + userId);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("Bạn cần đăng nhập để đặt lịch.");
            }


            // Kiểm tra ngày giờ hợp lệ
            if (selectedDate < DateTime.UtcNow.Date || selectedTime < TimeSpan.Zero)
            {
                return BadRequest("Thời gian không hợp lệ.");
            }

            var doctor = await _doctorService.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                return NotFound("Bác sĩ không tồn tại.");
            }

            bool appointmentExists = await _appointmentService.CheckAppointmentExistsAsync(doctorId, selectedDate, selectedTime);
            if (appointmentExists)
            {
                ModelState.AddModelError("", "Khung giờ này đã có người đặt. Vui lòng chọn khung giờ khác.");
                return RedirectToAction("DoctorSchedule", new { doctorId, selectedDate });
            }

            var appointment = new Appointment
            {
                UserId = int.Parse(userId),
                DoctorId = doctorId,
                StartTime = selectedDate.Date + selectedTime,
                EndTime = (selectedDate.Date + selectedTime).AddMinutes(30),
                Status = 1,
                Subject = "Khám bệnh"
            };

            await _appointmentService.CreateAppointmentAsync(appointment);

            return RedirectToAction("AppointmentConfirmation", new { appointmentId = appointment.AppointmentId });
        }

        public async Task<IActionResult> AppointmentConfirmation(int appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound("Lịch hẹn không tồn tại.");
            }

            var doctor = await _doctorService.GetDoctorByIdAsync(appointment.DoctorId);
            var user = await _accountService.GetUserByIdAsync(appointment.UserId);

            if (doctor == null || user == null)
            {
                return NotFound("Dữ liệu không hợp lệ.");
            }

            var viewModel = new AppointmentConfirmationViewModel
            {
                AppointmentId = appointment.AppointmentId,
                DoctorName = doctor.FullName,
                PatientName = user.FullName,
                AppointmentDate = appointment.StartTime.ToString("yyyy-MM-dd"),
                StartTime = appointment.StartTime.ToString("HH:mm"),
                EndTime = appointment.EndTime.ToString("HH:mm"),
                Subject = appointment.Subject
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ConfirmAppointment(int appointmentId)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(appointmentId);
            if (appointment == null)
            {
                return NotFound("Lịch hẹn không tồn tại.");
            }

            // Gửi email xác nhận
            var subject = "Xác nhận lịch hẹn";
            var message = $@"
            Xin chào {appointment.User.Username},
            <br/><br/>
            Lịch hẹn của bạn đã được xác nhận với bác sĩ <strong>{appointment.Doctor.FullName}</strong>.
            <br/>
            - Ngày: {appointment.StartTime}
            <br/>
            - Giờ: {appointment.StartTime} - {appointment.EndTime}
            <br/><br/>
            Trân trọng,<br/>
            Phòng khám XYZ";

            await _emailService.SendEmailAsync(appointment.User.Email, subject, message);

            TempData["SuccessMessage"] = "Lịch hẹn đã được xác nhận và email đã được gửi!";
            return RedirectToAction("Index", "Home");
        }

    }
}
