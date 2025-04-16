using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Authorization;
using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_LTWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAdminRepository _adminRepository;
        private readonly IHopDongRepository _hopDongRepository;
        private readonly IPhongTroRepository _phongTroRepository;
        private readonly INhaTroRepository _nhaTroRepository;
        private readonly IChiTietPhongTroRepository _chiTietPhongTroRepository;

        public HomeController(ILogger<HomeController> logger, IAdminRepository adminRepository, IHopDongRepository hopDongRepository, IPhongTroRepository phongTroRepository, INhaTroRepository nhaTroRepository, IChiTietPhongTroRepository chiTietPhongTroRepository)
        {
            _logger = logger;
            _adminRepository = adminRepository;
            _hopDongRepository = hopDongRepository;
            _phongTroRepository = phongTroRepository;
            _nhaTroRepository = nhaTroRepository;
            _chiTietPhongTroRepository = chiTietPhongTroRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {
            var phongTro = await _phongTroRepository.GetByIdAsync(id);
            var nhaTro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTros = new SelectList(nhaTro, "MaNhaTro", "TenNhaTro");
            var chitietphongtro = await _chiTietPhongTroRepository.GetAllAsync();
            ViewBag.ChiTietPhongTro = new SelectList(chitietphongtro, "MaChiTietPhongTro", "GiaPhong");
            return View(phongTro);

        }

        public async Task<IActionResult> Display()
        {
            var phongTro = await _phongTroRepository.GetAllAsync();
            var chiTietPhongTro = await _chiTietPhongTroRepository.GetAllAsync();

            var model = new PhongTroViewModel
            {
                PhongTro = phongTro,
                ChiTietPhongTro = chiTietPhongTro
            };

            return View(model);
        }

        public async Task<IActionResult> Display2()
        {
            var phongTro = await _phongTroRepository.GetAllAsync();
            var chiTietPhongTro = await _chiTietPhongTroRepository.GetAllAsync();

            var model = new PhongTroViewModel
            {
                PhongTro = phongTro,
                ChiTietPhongTro = chiTietPhongTro
            };

            return View(model);
        }

        public async Task<IActionResult> Dashboard()
        {
            // 1. Thống kê lượt đăng ký
            var admins = await _adminRepository.GetAllAsync();
            var registrationData = admins
                .GroupBy(u => u.NgayDangKy.Month)
                .Select(g => new RegistionViewModel
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Month)
                .ToList();

            // 2. Thống kê hợp đồng theo hình thức ký kết
            var contractDates = await _hopDongRepository.GetAllAsync();
            var contractData = contractDates
                .GroupBy(h => h.HinhThuc)
                .Select(g => new ContractViewModel
                {
                    HinhThuc = g.Key,
                    Count = g.Count()
                })
                .ToList();

            // 3. Đưa vào ViewModel tổng hợp
            var viewModel = new DashboardViewModel
            {
                RegistrationStats = registrationData,
                ContractStats = contractData
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}