using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class GarageController : Controller
    {
        private readonly IVehicalRepository _vehicalRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly INhaXeRepository _nhaXeRepository;

        public GarageController(IVehicalRepository vehicalRepository, IAdminRepository adminRepository, INhaXeRepository nhaXeRepository)
        {
            _vehicalRepository = vehicalRepository;
            _adminRepository = adminRepository;
            _nhaXeRepository = nhaXeRepository;
        }
        [HttpGet]
        public IActionResult Success()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Garage()
        {
            return View("Garage");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _vehicalRepository.GetAllAsync();
            var data = users.Select(x => new {
                x.MaXe,
                x.BienSoXe,
                x.CMND,
                x.MaNhaXe,
                TenKhachHang = x.KhachHang.TenKhachHang
            }).ToList();

            return Json(new { data });

        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var nhaXeList = await _nhaXeRepository.GetAllAsync(); // Lấy danh sách nhà xe
            var khachHangList = await _adminRepository.GetAllAsync(); // Lấy danh sách khách hàng

            ViewBag.NhaXeList = new SelectList(nhaXeList, "MaNhaXe", "MaNhaTro");
            ViewBag.KhachHangList = new SelectList(khachHangList, "MaKhachHang", "TenKhachHang");

            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Xe xe)
        {
            if (!ModelState.IsValid)
            {
                return View(xe);
            }

            var khachHang = await _adminRepository.GetByIdAsync(xe.MaKhachHang);
            if (khachHang == null)
            {
                ModelState.AddModelError("MaKhachHang", "Khách hàng không hợp lệ.");
                return View(xe);
            }
            await _vehicalRepository.AddAsync(xe);
            return RedirectToAction("Success");
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var xe = await _vehicalRepository.GetByIdAsync(id);
            if (xe == null)
            {
                return NotFound();
            }

            return View(xe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Xe xe)
        {
            if (!ModelState.IsValid)
            {
                return View("Update", xe);
            }

            var existingXe = await _vehicalRepository.GetByIdAsync(xe.MaXe);
            if (existingXe == null)
            {
                return NotFound();
            }

            existingXe.BienSoXe = xe.BienSoXe;
            existingXe.CMND = xe.CMND;
            existingXe.MaKhachHang = xe.MaKhachHang;

            await _vehicalRepository.UpdateAsync(existingXe);

            return RedirectToAction("Success");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var xe = await _vehicalRepository.GetByIdAsync(id);
            if (xe == null)
            {
                return NotFound();
            }
            return View(xe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var xe = await _vehicalRepository.GetByIdAsync(id);
            if (xe == null)
            {
                return NotFound();
            }

            await _vehicalRepository.DeleteAsync(id);
            return RedirectToAction("Garage"); // ✅ Chuyển hướng về danh sách xe
        }



    }
}
