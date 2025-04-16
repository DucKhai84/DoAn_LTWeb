 using Microsoft.AspNetCore.Mvc;
using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DoAn_LTWeb.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class PhongTroController : Controller
    {
        private readonly IPhongTroRepository _phongTroRepository;
        private readonly INhaTroRepository _nhaTroRepository;
        private readonly IChiTietPhongTroRepository _chiTietPhongTroRepository;
        private readonly INoiThatRepository _noiThatRepository;

        public PhongTroController(IPhongTroRepository phongTroRepository, INhaTroRepository nhaTroRepository,INoiThatRepository noiThatRepository,IChiTietPhongTroRepository chiTietPhongTroRepository)
        {
            _phongTroRepository = phongTroRepository;
            _nhaTroRepository = nhaTroRepository;
            _chiTietPhongTroRepository = chiTietPhongTroRepository;
            _noiThatRepository = noiThatRepository;
        }

        public async Task<IActionResult> Index()
        {
            var phongTro = await _phongTroRepository.GetAllAsync();
            return View(phongTro);

        }

       
        /*Hien thi chi tiet phong tro*/
        public async Task<IActionResult> Display(int id)
        {
            var phongTro = await _phongTroRepository.GetByIdAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }
            return View(phongTro);
        }

       
    }
}
