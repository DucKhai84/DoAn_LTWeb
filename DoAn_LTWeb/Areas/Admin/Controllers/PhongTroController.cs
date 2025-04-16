 using Microsoft.AspNetCore.Mvc;
using DoAn_LTWeb.Repositories;
using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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


        public async Task<IActionResult> Detail(int id)
        {
            var phongTro = await _phongTroRepository.GetByIdAsync(id);
            var nhaTro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTros = new SelectList(nhaTro, "MaNhaTro", "TenNhaTro");
            var chitietphongtro = await _chiTietPhongTroRepository.GetAllAsync();
            ViewBag.ChiTietPhongTro = new SelectList(chitietphongtro, "MaChiTietPhongTro", "GiaPhong");
            return View(phongTro);

        }
        public async Task<IActionResult> Index_Admin()
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

        [Route("Them-san-pham")]
        public async Task<IActionResult> Add()
        {
            var phongtro = new PhongTro();
            var phongtros = await _phongTroRepository.GetAllAsync();
            var nhaTros = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTro = new SelectList(nhaTros, "MaNhaTro", "TenNhaTro"); ;
            return View(phongtro);
        }

        [Route("Them-san-pham")]
        [HttpPost]
        public async Task<IActionResult> Add(PhongTro phongTro, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                // Lưu ảnh đại diện
                if (imageUrl != null)
                {
                    phongTro.ImageUrl = await SaveImage(imageUrl);
                }

                // Lưu danh sách ảnh khác nếu có
                if (imageUrls != null && imageUrls.Any())
                {
                    foreach (var file in imageUrls)
                    {
                        var imageUrlString = await SaveImage(file);
                        phongTro.ImageUrls.Add(imageUrlString);
                    }
                }

                await _phongTroRepository.AddAsync(phongTro);
                return RedirectToAction("Add", "ChiTietPhongTro", new { id = phongTro.MaPhongTro });
            
            //return RedirectToAction(nameof(Index));
            //return RedirectToAction("Add", "ChiTietPhongTro");


        }

            var nhatro = await _nhaTroRepository.GetAllAsync();
            ViewBag.nhatro = new SelectList(nhatro, "MaNhaTro", "TenNhaTro");
            return RedirectToAction("Index_Admin");

        }



        private async Task<string> SaveImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                return string.Empty;
            }

            try
            {
                // Đảm bảo thư mục tồn tại
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file duy nhất
                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                // Trả về đường dẫn file ảnh
                return "/images/" + uniqueFileName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<IActionResult> Update(int id)
        {
            var phongTro = await _phongTroRepository.GetByIdAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }

            var nhaTro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTros = new SelectList(nhaTro , "MaNhaTro" , "TenNhaTro");
            return View(phongTro);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PhongTro phongTro, float GiaPhong, int MaNoiThat, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
           ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl

            if (id != phongTro.MaPhongTro)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingphongTro = await _phongTroRepository.GetByIdAsync(id);
                if (existingphongTro == null)
                {
                    return NotFound();
                }

                long maxFileSize = 5 * 1024 * 1024; // 5MB

                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (imageUrl == null)
                {
                    phongTro.ImageUrl = existingphongTro.ImageUrl;
                }
                else if (imageUrl.Length <= maxFileSize)
                {
                    phongTro.ImageUrl = await SaveImage(imageUrl);
                }

                // Cập nhật danh sách ảnh phụ
                if (imageUrls == null || !imageUrls.Any())
                {
                    phongTro.ImageUrls = existingphongTro.ImageUrls;
                }
                else
                {
                    phongTro.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        if (file.Length <= maxFileSize)
                        {
                            phongTro.ImageUrls.Add(await SaveImage(file));
                        }
                    }
                }

                existingphongTro.TenPhongTro = phongTro.TenPhongTro;
                existingphongTro.ImageUrl = phongTro.ImageUrl;
                existingphongTro.Mota = phongTro.Mota;
                existingphongTro.DienTich = phongTro.DienTich;
                existingphongTro.TrangThai = phongTro.TrangThai;
                existingphongTro.ImageUrls = phongTro.ImageUrls;


                await _phongTroRepository.UpdateAsync(existingphongTro);
                return RedirectToAction("Index_Admin");
            }

            var noiThatList = await _noiThatRepository.GetAllAsync();
            ViewBag.NoiThatList = new SelectList(noiThatList, "MaNoiThat", "TenNoiThat");
            return View(phongTro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var phongTro = await _phongTroRepository.GetByIdAsync(id);
            if (phongTro == null)
            {
                return NotFound();
            }
            return View(phongTro);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed()
        {  
            if (Request.Form["delete"].Count > 0)
            {
                int id = int.Parse(Request.Form["MaPhongTro"]);
                await _phongTroRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index_Admin));
            } 
            return NotFound();
        }


    }
}
