using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HopDongController : Controller
    {
        private readonly IHopDongRepository _hopdongrepository;
        private readonly IDieuKhoanRepository _dieukhoanrepository;
        private readonly IPhongTroRepository _phongtrorepository;
        private readonly IChiTietThuePhongRepository _chiTietThuePhongrepository;

        public HopDongController(
            IHopDongRepository hopdongrepository,
            IDieuKhoanRepository dieukhoanrepository,
            IPhongTroRepository phongtrorepository,
            IChiTietThuePhongRepository chitietthuephongrepository)
        {
            _hopdongrepository = hopdongrepository;
            _dieukhoanrepository = dieukhoanrepository;
            _phongtrorepository = phongtrorepository;
            _chiTietThuePhongrepository = chitietthuephongrepository;
        }

        public async Task<IActionResult> Success()
        {
            return View();
        }

        public async Task<IActionResult> Index_Admin()
        {
            var hopDong = await _hopdongrepository.GetAllAsync();
            return View(hopDong);

        }
        [HttpGet]
        public async Task<IActionResult> Add(int maPhongTro)
        {
            var phongTroList = await _phongtrorepository.GetAllAsync();
           
            var dieuKhoanList = await _dieukhoanrepository.GetAllAsync();
            if (dieuKhoanList == null || !dieuKhoanList.Any())
            {
                ModelState.AddModelError("", "Không có điều khoản nào trong hệ thống.");
            }

            HttpContext.Session.SetInt32("MaPhongTro", maPhongTro);
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Add(HopDong hopDong, int maPhongTro)
        {
            if (!ModelState.IsValid)
            {
                var dieuKhoanList = await _dieukhoanrepository.GetAllAsync();
                ViewBag.DieuKhoanList = new SelectList(dieuKhoanList, "MaDieuKhoan", "TenDieuKhoan");
                return View(hopDong);
            }

            await _hopdongrepository.AddAsync(hopDong);

            return RedirectToAction("Index", "DieuKhoan", new { id = hopDong.MaHopDong, MaPhongTro = maPhongTro });
        }

        public async Task<IActionResult> Update(int id)
        {
            var hopDong = await _hopdongrepository.GetByIdAsync(id);
            if (hopDong == null)
            {
                return NotFound();
            }

            var dieuKhoan = await _dieukhoanrepository.GetAllAsync();
            ViewBag.DieuKhoan = new SelectList(dieuKhoan, "MaDieuKhoan", "NoiDung");
            return View(hopDong);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, HopDong hopDong, string HinhThuc, DateTime NgayBatDau, DateTime NgayKetThuc, List<IFormFile>? imageUrls)
        {

            if (ModelState.IsValid)
            {
                var existinghopDong = await _hopdongrepository.GetByIdAsync(id);
                if (existinghopDong == null)
                {
                    return NotFound();
                }

                existinghopDong.HinhThuc = hopDong.HinhThuc;
                existinghopDong.NgayBatDau = hopDong.NgayBatDau;
                existinghopDong.NgayKetThuc = hopDong.NgayKetThuc;


                await _hopdongrepository.UpdateAsync(existinghopDong);
                return RedirectToAction("Index_Admin");
            }

            var dieuKhoan = await _dieukhoanrepository.GetAllAsync();
            ViewBag.DieuKhoan = new SelectList(dieuKhoan, "MaDieuKhoan", "NoiDung");
            return View(hopDong);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hopDong = await _hopdongrepository.GetByIdAsync(id);
            if (hopDong == null)
            {
                return NotFound();
            }
            return View(hopDong);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hopDong = await _hopdongrepository.GetByIdAsync(id);
            if (hopDong == null)
            {
                return NotFound();
            }

            await _hopdongrepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index_Admin));
        }
    }

}
