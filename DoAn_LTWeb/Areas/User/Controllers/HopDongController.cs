using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IdentityModel.Claims;

namespace DoAn_LTWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class HopDongController : Controller
    {
        private readonly IHopDongRepository _hopdongrepository;
        private readonly IDieuKhoanRepository _dieukhoanrepository;
        private readonly IAdminRepository _adminrepository;
        private readonly IChiTietThuePhongRepository _chiTietThuePhongrepository;

        public HopDongController(IHopDongRepository hopdongrepository, IDieuKhoanRepository dieukhoanrepository, IAdminRepository adminrepository, IChiTietThuePhongRepository chiTietThuePhongrepository)
        {
            _hopdongrepository = hopdongrepository;
            _dieukhoanrepository = dieukhoanrepository;
            _adminrepository = adminrepository;
            _chiTietThuePhongrepository = chiTietThuePhongrepository;
        }

        public async Task<IActionResult> Index()
        {      

            var chiTietList = await _chiTietThuePhongrepository.GetAllAsync();

            if (chiTietList == null)
            {
                return NotFound("Lỗi");
            }

            var userList = await _adminrepository.GetAllAsync();
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = userList.FirstOrDefault(u => u.UserId == userId);

            if(userId == null)
            {
                return NotFound();
            }

            var chiTiet = chiTietList.FirstOrDefault(c => c.MaKhachHang == user.MaKhachHang);

            if (chiTiet == null)
            {
                ViewBag.ErrorMessage = "Bạn đã tạo lịch hẹn với chúng tôi chưa nè ? Vui lòng liên hệ quản trị viên.";
                return View();
            }


            var hopDongList = await _hopdongrepository.GetAllAsync();

            if (hopDongList == null)
            {
                return NotFound("Lỗi");
            }

            var hopDong = hopDongList.FirstOrDefault(h => h.MaHopDong == chiTiet.MaHopDong);

            var appointment = new AppointmentViewModel
            {
                MaHopDong = hopDong.MaHopDong,
                HinhThuc =  hopDong.HinhThuc,
                NgayBatDau = hopDong.NgayBatDau,
                NgayKetThuc = hopDong.NgayKetThuc,
                TenKhachHang = user.TenKhachHang,
                SoDienThoai = user.SoDienThoai,
                Email = user.Email,
                MaPhongTro = chiTiet.MaPhongTro,
                SoLuong = chiTiet.SoLuong

            };
            return View(appointment);

        }

       

        public async Task<IActionResult> Add()
        {
            var hopDong = new HopDong();
            var hopDongs = await _hopdongrepository.GetAllAsync();
            var dieukhoan = await _dieukhoanrepository.GetAllAsync();
            ViewBag.DieuKhoan = new SelectList(dieukhoan, "MaDieuKhoan", "NoiDung");
            return View(hopDong);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HopDong hopDong)
        {
            if (ModelState.IsValid)
            {
                await _hopdongrepository.AddAsync(hopDong);
                return RedirectToAction(nameof(Index));

                //return RedirectToAction(nameof(Index));
                //return RedirectToAction("Add", "ChiTietPhongTro");


            }

            var dieuKhoan = await _dieukhoanrepository.GetAllAsync();
            ViewBag.DieuKhoan = new SelectList(dieuKhoan, "MaDieuKhoan", "NoiDung");
            return RedirectToAction("Index_Admin");

        }

       

    }

}
