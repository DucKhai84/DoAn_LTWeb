using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }
        public IActionResult Index()
        {
            return View("~/Areas/Admin/Views/Admin/Index.cshtml");
        }

        public IActionResult Index1()
        {
            return View("~/Areas/Admin/Views/Admin/Index1.cshtml");
        }
        public IActionResult Rooms()
        {
            return View();
        }
        
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Genders = new SelectList(new[] { "Nam", "Nữ" });
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Genders = new SelectList(new[] { "Nam", "Nữ" });
                return View(khachHang);
            }

            await _adminRepository.AddAsync(khachHang);
            return RedirectToAction("Success");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _adminRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Genders = new SelectList(new[] { "Nam", "Nữ" }, user.GioiTinh);

            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _adminRepository.GetAllAsync();
            return Json(new { data = users }); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KhachHang khachHang)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", khachHang);
            }

            var existingKhachHang = await _adminRepository.GetByIdAsync(khachHang.MaKhachHang);
            if (existingKhachHang == null)
            {
                return NotFound();
            }

            existingKhachHang.TenKhachHang = khachHang.TenKhachHang;
            existingKhachHang.SoDienThoai = khachHang.SoDienThoai;
            existingKhachHang.Email = khachHang.Email;
            existingKhachHang.CMND = khachHang.CMND;
            existingKhachHang.GioiTinh = khachHang.GioiTinh;
            existingKhachHang.NgaySinh = khachHang.NgaySinh;

            await _adminRepository.UpdateAsync(existingKhachHang);

            return RedirectToAction("Success");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _adminRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _adminRepository.DeleteAsync(id);
            return RedirectToAction("Success");
        }


    }
}
