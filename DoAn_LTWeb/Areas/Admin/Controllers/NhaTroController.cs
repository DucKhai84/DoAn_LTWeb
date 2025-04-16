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
    public class NhaTroController : Controller
    {
        private readonly IPhongTroRepository _phongTroRepository;
        private readonly INhaTroRepository _nhaTroRepository;

        public NhaTroController(IPhongTroRepository phongTroRepository, INhaTroRepository nhaTroRepository)
        {
            _phongTroRepository = phongTroRepository;
            _nhaTroRepository = nhaTroRepository;
        }

        public async Task<IActionResult> Index()
        {
            var nhaTro = await _nhaTroRepository.GetAllAsync();
            return View(nhaTro);
        }

        public async Task<IActionResult> Index_Admin()
        {
            var nhaTro = await _nhaTroRepository.GetAllAsync();
            return View(nhaTro);

        }

        public async Task<IActionResult> Display(int id)
        {
            var nhaTro = await _nhaTroRepository.GetByIdAsync(id);
            if (nhaTro == null)
            {
                return NotFound();
            }
            return View(nhaTro);
        }

        public async Task<IActionResult> Add()
        {
            var nhaTro = new NhaTro(); 
            return View(nhaTro);

        }


        [HttpPost]
        public async Task<IActionResult> Add(NhaTro nhaTro, IFormFile imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                // Lưu ảnh đại diện
                if (imageUrl != null)
                {
                    nhaTro.ImageUrl = await SaveImage(imageUrl);
                }

                // Lưu danh sách ảnh khác nếu có
                if (imageUrls != null && imageUrls.Any())
                {
                    foreach (var file in imageUrls)
                    {
                        var imageUrlString = await SaveImage(file);
                        nhaTro.ImageUrls.Add(imageUrlString);
                    }
                }

                await _nhaTroRepository.AddAsync(nhaTro);
                return RedirectToAction(nameof(Index_Admin));



                await _nhaTroRepository.AddAsync(nhaTro);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index_Admin));

            }
            return View(nhaTro);
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
            var nhaTro = await _nhaTroRepository.GetByIdAsync(id);
            if (nhaTro == null)
            {
                return NotFound();
            }
            return View(nhaTro);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NhaTro nhaTro, IFormFile imageUrl, List<IFormFile>? imageUrls)
        {
            ModelState.Remove("ImageUrl");

            if (id != nhaTro.MaNhaTro)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingNhaTro = await _nhaTroRepository.GetByIdAsync(id);
                if (existingNhaTro == null)
                {
                    return NotFound();
                }

                long maxFileSize = 5 * 1024 * 1024;

                if (imageUrl == null)
                {
                    nhaTro.ImageUrl = existingNhaTro.ImageUrl;
                }
                else
                {
                    nhaTro.ImageUrl = await SaveImage(imageUrl);
                }

                if (imageUrls == null || !imageUrls.Any())
                {
                    nhaTro.ImageUrls = existingNhaTro.ImageUrls ?? new List<string>();
                }
                else
                {
                    nhaTro.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        if (file.Length <= maxFileSize)
                        {
                            nhaTro.ImageUrls.Add(await SaveImage(file));
                        }
                    }
                }

                existingNhaTro.TenNhaTro = nhaTro.TenNhaTro;
                existingNhaTro.DiaChi = nhaTro.DiaChi;
                existingNhaTro.ImageUrl = nhaTro.ImageUrl;
                existingNhaTro.ImageUrls = nhaTro.ImageUrls;

                await _nhaTroRepository.UpdateAsync(existingNhaTro);
                return RedirectToAction(nameof(Index_Admin));
            }
            return View(nhaTro);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var nhaTro = await _nhaTroRepository.GetByIdAsync(id);
            if (nhaTro == null)
            {
                return NotFound();
            }
            return View(nhaTro);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed()
        {
            if (Request.Form["delete"].Count > 0)
            {
                int id = int.Parse(Request.Form["MaNhaTro"]);
                await _nhaTroRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index_Admin));
            }
            return NotFound();
            
        }
    }
}