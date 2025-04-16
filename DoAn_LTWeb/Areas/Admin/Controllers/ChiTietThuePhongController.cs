using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChiTietThuePhongController : Controller
    {
        private readonly IHopDongRepository _hopdongrepository;
        private readonly IDieuKhoanRepository _dieukhoanrepository;
        private readonly IPhongTroRepository _phongtrorepository;
        private readonly IChiTietThuePhongRepository _chiTietThuePhongrepository;
        private readonly IAdminRepository _adminrepository;
        private readonly EFAspRepository _fasprepository;

        public ChiTietThuePhongController(IHopDongRepository hopdongrepository, IDieuKhoanRepository dieukhoanrepository, IPhongTroRepository phongtrorepository, IChiTietThuePhongRepository chitietthuephongrepository, IAdminRepository adminrepository, EFAspRepository fasprepository)
        {
            _hopdongrepository = hopdongrepository;
            _dieukhoanrepository = dieukhoanrepository;
            _phongtrorepository = phongtrorepository;
            _chiTietThuePhongrepository = chitietthuephongrepository;
            _adminrepository = adminrepository;
            _fasprepository = fasprepository;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> Index(int maHopDong)
        {
            var hopDongList = await _hopdongrepository.GetAllAsync();
            var userList = await _adminrepository.GetAllAsync();

            var hopDong = hopDongList.FirstOrDefault(h => h.MaHopDong == maHopDong);

            if (hopDong == null)
            {
                return NotFound("Không tìm thấy hợp đồng!");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if(userId == null)
            {
                return NotFound("Không tìm thấy id");
            }

            var user = userList.FirstOrDefault(u => u.UserId == userId);

            if(userId == null)
            {
                return NotFound("Không tìm thấy khách hàng");
            }

            int? maPhongTro = HttpContext.Session.GetInt32("MaPhongTro");

            if (maPhongTro == null)
            {
                return Content("Không tìm thấy dữ liệu trong Session!");
            }

            var chiTiet = new ChiTietThuePhongViewModel()
            {
                MaHopDong = hopDong.MaHopDong,
                MaKhachHang = user.MaKhachHang,
                TenKhachHang = user.TenKhachHang,
                MaPhongTro = (int)maPhongTro,
            };

            return View(chiTiet);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Add(ChiTietThuePhongViewModel model)
        {
            // Kiểm tra dữ liệu đầu vào
            if (model.MaHopDong <= 0)
            {
                return BadRequest("Mã hợp đồng không hợp lệ.");
            }

            // Kiểm tra xem MaHopDong có tồn tại trong bảng HopDong hay không
            var hopDong = await _hopdongrepository.GetByIdAsync(model.MaHopDong);
            if (hopDong == null)
            {
                return NotFound("Hợp đồng không tồn tại.");
            }

            var chiTietThuePhong = new ChiTietThuePhong
            {
                MaHopDong = model.MaHopDong,
                MaKhachHang = model.MaKhachHang,
                MaPhongTro = model.MaPhongTro,
                SoLuong = model.SoLuong,
            };

            await _chiTietThuePhongrepository.AddAsync(chiTietThuePhong);

            //return RedirectToAction("Success");

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Success()
        {
            return View();
        }
    }
}
