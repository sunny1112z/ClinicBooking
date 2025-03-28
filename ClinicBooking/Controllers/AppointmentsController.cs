using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Entities;
using ClinicBooking.Services;

namespace ClinicBooking.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(AppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

      
        public async Task<IActionResult> Index()
        {
            var appointments = await _appointmentService.GetAllAppointmentsAsync();
            return View(appointments);
        }

      
        public async Task<IActionResult> Details(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // Hiển thị form tạo mới cuộc hẹn
        public IActionResult Create()
        {
            return View();
        }

        // Xử lý tạo cuộc hẹn mới
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment)
        {
            if (!ModelState.IsValid) return View(appointment);

            await _appointmentService.CreateAppointmentAsync(appointment);
            return RedirectToAction(nameof(Index));
        }

        // Hiển thị form chỉnh sửa cuộc hẹn
        public async Task<IActionResult> Edit(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // Xử lý cập nhật cuộc hẹn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Appointment appointment)
        {
            if (id != appointment.AppointmentId) return BadRequest();

            if (!ModelState.IsValid) return View(appointment);

            var updated = await _appointmentService.UpdateAppointmentAsync(appointment);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // Xác nhận xóa cuộc hẹn
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _appointmentService.GetAppointmentByIdAsync(id);
            if (appointment == null) return NotFound();
            return View(appointment);
        }

        // Xử lý xóa cuộc hẹn
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _appointmentService.DeleteAppointmentAsync(id);
            if (!deleted) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
