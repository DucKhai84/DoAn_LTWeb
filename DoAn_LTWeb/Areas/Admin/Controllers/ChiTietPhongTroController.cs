using Microsoft.AspNetCore.Mvc;
using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using Microsoft.AspNetCore.Authorization;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ChiTietPhongTroController : Controller
    {
        private readonly IChiTietPhongTroRepository _chiTietPhongTroRepository;
        private readonly INoiThatRepository _noiThatRepository;
        private readonly IPhongTroRepository _phongTroRepository;
        private readonly INhaTroRepository _nhaTroRepository;

        public ChiTietPhongTroController(IChiTietPhongTroRepository chiTietPhongTroRepository, INoiThatRepository noiThatRepository, IPhongTroRepository phongTroRepository, INhaTroRepository nhaTroRepository)
        {
            _chiTietPhongTroRepository = chiTietPhongTroRepository;
            _noiThatRepository = noiThatRepository;
            _phongTroRepository = phongTroRepository;
            _nhaTroRepository = nhaTroRepository;
        }

        
        public async Task<IActionResult> Index()
        {
            var chiTietPhongTro = await _chiTietPhongTroRepository.GetAllAsync();
            return View(chiTietPhongTro);
        }

        public async Task<IActionResult> Index_Admin()
        {
            var chiTietPhongTro = await _chiTietPhongTroRepository.GetAllAsync();
            return View(chiTietPhongTro);
        }

        public async Task<IActionResult> Display(int id)
        {
            var chiTietPhongTro = await _chiTietPhongTroRepository.GetByIdAsync(id);
            if (chiTietPhongTro == null)
            {
                return NotFound();
            }
            return View(chiTietPhongTro);
        }

        /*Add*/
        public async Task<IActionResult> Add(int? id)
        {
            var chiTietPhongTro = new ChiTietPhongTro();

            var phongtro = await _phongTroRepository.GetAllAsync();
            var noithat = await _noiThatRepository.GetAllAsync();
            var nhatro = await _nhaTroRepository.GetAllAsync();


            var phongTroWithNhaTro = phongtro
                .Select(p => new
                {
                    MaPhongTro = p.MaPhongTro,
                    TenPhongTro = p.TenPhongTro + " - " + (p.NhaTro != null ? p.NhaTro.TenNhaTro : "Chưa có nhà trọ")
                })
                .ToList();

            chiTietPhongTro.MaPhongTro = id ?? 0;
            //ChiTietPhongTro.NhaTro.MaNhaTro = id ?? 0;

            ViewBag.PhongTro = new SelectList(phongTroWithNhaTro, "MaPhongTro", "TenPhongTro");
            ViewBag.NoiThat = new SelectList(noithat, "MaNoiThat", "TenNoiThat");
            ViewBag.NhaTro = new SelectList(nhatro, "MaNhaTro", "TenNhaTro");

            return View(chiTietPhongTro);
        }



        [HttpPost]
        public async Task<IActionResult> Add(ChiTietPhongTro chiTietPhongTro, float GiaPhong, int MaPhongTro, List<int> MaNoiThat)
        {
            if (ModelState.IsValid)
            {
                foreach (var noiThatId in MaNoiThat)
                {
                    var chiTiet = new ChiTietPhongTro
                    {
                        MaPhongTro = MaPhongTro,
                        MaNoiThat = noiThatId,
                        GiaPhong = GiaPhong
                    };
                    await _chiTietPhongTroRepository.AddAsync(chiTiet);
                }

                return RedirectToAction("Index_Admin", "PhongTro");
            }

            // Nếu ModelState không hợp lệ, tải lại danh sách
            var phongTroList = await _phongTroRepository.GetAllAsync();
            var noiThatList = await _noiThatRepository.GetAllAsync();
            var nhatro = await _nhaTroRepository.GetAllAsync();

         /*   var phongTroWithNhaTro = phongTroList
                .Select(p => new {
                    MaPhongTro = p.MaPhongTro,
                    TenPhongTro = p.TenPhongTro + " - " + (p.NhaTro != null ? p.NhaTro.TenNhaTro : "Chưa có nhà trọ")
                })
                .ToList();*/

            ViewBag.PhongTro = new SelectList(phongTroList, "MaPhongTro", "TenPhongTro");
            ViewBag.NoiThat = new SelectList(noiThatList, "MaNoiThat", "TenNoiThat");
            ViewBag.NhaTro = new SelectList(nhatro, "MaNhaTro", "TenNhaTro");

            return View(chiTietPhongTro);
        }


        /*Update*/

        public async Task<IActionResult> Update(int id)
         {
            var chitietphongtro = await _chiTietPhongTroRepository.GetByIdAsync(id);
            if (chitietphongtro == null)
            {
                return NotFound();
            }

            var phongTro = await _phongTroRepository.GetAllAsync();
            var noiThat = await _noiThatRepository.GetAllAsync();
            ViewBag.PhongTro = new SelectList(phongTro, "MaPhongTro", "TenPhongTro");
            ViewBag.NoiThat = new SelectList(noiThat, "MaNoiThat", "TenNoiThat");
            return View(chitietphongtro);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, ChiTietPhongTro chiTietPhongTro, float GiaPhong, int MaNoiThat, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
        

            if (id != chiTietPhongTro.MaChiTietPhongTro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingphongTro = await _chiTietPhongTroRepository.GetByIdAsync(id);
                if (existingphongTro == null)
                {
                    return NotFound();
                }

                existingphongTro.GiaPhong = chiTietPhongTro.GiaPhong;
                
               


                await _chiTietPhongTroRepository.UpdateAsync(existingphongTro);
                return RedirectToAction("Index_Admin");
            }

            var noiThatList = await _noiThatRepository.GetAllAsync();
            var phongTroList = await _phongTroRepository.GetAllAsync();
            ViewBag.PhongTroList = new SelectList(phongTroList, "MaPhongTro", "TenPhongTro");
            ViewBag.NoiThatList = new SelectList(noiThatList, "MaNoiThat", "TenNoiThat");
            return View(chiTietPhongTro);
        }




        /*Delete*/

        public async Task<IActionResult> Delete(int id)
        {
            var chitietphongtro = await _chiTietPhongTroRepository.GetByIdAsync(id);
            if (chitietphongtro == null)
            {
                return NotFound();
            }
            return View(chitietphongtro);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed()
        {
            if (Request.Form["delete"].Count > 0)
            {
                int id = int.Parse(Request.Form["MaChiTietPhongTro"]);
                await _chiTietPhongTroRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index_Admin));
            }
            return NotFound();
        }

     
    }
}
