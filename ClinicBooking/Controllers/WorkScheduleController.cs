using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClinicBooking.Entities;
using ClinicBooking.Services;
using System.Linq;
using ClinicBooking_Service;
using Microsoft.EntityFrameworkCore;

namespace ClinicBooking.Controllers
{
    public class WorkScheduleController : Controller
    {
        private readonly WorkScheduleService _workScheduleService;
        private readonly DoctorService _doctorService;

        public WorkScheduleController(WorkScheduleService workScheduleService,DoctorService doctorService)
        {
            _workScheduleService = workScheduleService;
            _doctorService = doctorService;
        }

        // Hiển thị danh sách lịch làm việc (View)
        public async Task<IActionResult> Index()
        {
            var schedules = await _workScheduleService.GetAllWorkSchedulesAsync();
            return View(schedules);
        }

        // Hiển thị chi tiết lịch làm việc
        public async Task<IActionResult> Details(int id)
        {
            var schedule = await _workScheduleService.GetWorkScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        // Hiển thị form tạo mới lịch làm việc
        public async Task<IActionResult> Create()
        {
            var doctors = await _doctorService.GetAllDoctorsAsync(); 
            ViewBag.Doctor = doctors; 
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] WorkSchedule model)
        {
            // Kiểm tra ModelState hợp lệ
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState không hợp lệ:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            try
            {
                // Gọi service để thêm lịch làm việc vào database
                await _workScheduleService.AddWorkScheduleAsync(model);

                return Ok(new { message = "Thêm lịch làm việc thành công!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Lỗi server: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var schedule = await _workScheduleService.GetWorkScheduleByIdAsync(id);
            var doctors = await _doctorService.GetAllDoctorsAsync();

            if (schedule == null)
            {
                return NotFound();
            }

            ViewBag.Doctor = doctors;
            return View(schedule);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkSchedule schedule)
        {
            Console.WriteLine($"ScheduleID: {schedule.ScheduleID}");
            Console.WriteLine($"WorkDate: {schedule.WorkDate}");
            Console.WriteLine($"StartTime: {schedule.StartTime}");
            Console.WriteLine($"EndTime: {schedule.EndTime}");
            Console.WriteLine($"Status: {schedule.Status}");
            Console.WriteLine($"DoctorId: {schedule.DoctorID}");
            Console.WriteLine($"UserId: {schedule.UserId}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState không hợp lệ:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return BadRequest(ModelState);
            }

            await _workScheduleService.UpdateWorkScheduleAsync(schedule);
            return Json(new { message = "Cập nhật thành công!" });
        }




        // Hiển thị trang xác nhận xóa
        public async Task<IActionResult> Delete(int id)
        {
            var schedule = await _workScheduleService.GetWorkScheduleByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule);
        }

        // Xử lý xóa lịch làm việc
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _workScheduleService.DeleteWorkScheduleAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
