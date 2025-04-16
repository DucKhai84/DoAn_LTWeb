using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Claims;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class ProfileController : Controller
    {
        private readonly IAdminRepository _adminrepository;

        public ProfileController( IAdminRepository adminrepository)
        { 
            _adminrepository = adminrepository;
        }
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userList = await _adminrepository.GetAllAsync();

            if (userList == null)
            {
                return NotFound("Lỗi");
            }

            var user = userList.FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                return NotFound("Lỗi");
            }


            var profile = new ProfileViewModel
            {
                TenKhachHang = user.TenKhachHang,
                SoDienThoai = user.SoDienThoai,
                Email = user.Email,
                CMND = user.CMND,
                GioiTinh = user.GioiTinh,
                NgaySinh = user.NgaySinh,
                NgayDangKy = user.NgayDangKy
            };

            return View(profile);
        }
    }
}
