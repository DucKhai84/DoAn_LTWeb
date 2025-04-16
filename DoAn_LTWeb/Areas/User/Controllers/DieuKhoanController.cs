using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DoAn_LTWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class DieuKhoanController : Controller
    {
        private readonly IHopDongRepository _hopdongrepository;
        private readonly IDieuKhoanRepository _dieukhoanrepository;


        public DieuKhoanController(IHopDongRepository hopdongrepository, IDieuKhoanRepository dieukhoanrepository)
        {
            _hopdongrepository = hopdongrepository;
            _dieukhoanrepository = dieukhoanrepository;
        }

        public async Task<IActionResult> Index()
        {
            var nhaTro = await _dieukhoanrepository.GetAllAsync();
            return View(nhaTro);
        }

    }
}
