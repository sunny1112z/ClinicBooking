using Microsoft.AspNetCore.Mvc;
using ClinicBooking.Services;
using ClinicBooking.Entities;
using System.Threading.Tasks;
using ClinicBooking_Service;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace ClinicBooking.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly DepartmentsService _departmentService;

        public DepartmentController(DepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }


        public async Task<IActionResult> Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = page ?? 1;

            var departments = await _departmentService.GetAllClinicsAsync();

            var pagedList = departments.AsQueryable() 
                                       .OrderBy(d => d.DepartmentName)
                                       .ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }



        // ========== CREATE ==========
        public IActionResult Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.AddClinicAsync(department);
                return Json(new { success = true, message = " Add chuyên khoa thành công!" });
            }
            return PartialView("_Create", department);
        }

        // ========== UPDATE ==========
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetClinicByIdAsync(id);
            if (department == null) return NotFound();
            return PartialView("_Edit", department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateClinicAsync(department);
                return Json(new { success = true });
            }
            return PartialView("_Edit", department);
        }

        // ========== DELETE ==========
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var department = await _departmentService.GetClinicByIdAsync(id);
            if (department == null) return Json(new { success = false, message = "Không tìm thấy chuyên khoa!" });

            await _departmentService.DeleteClinicAsync(id);
            return Json(new { success = true });
        }

    }
}
