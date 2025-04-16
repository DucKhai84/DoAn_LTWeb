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
    public class NoiThatController : Controller
    {
        private readonly INoiThatRepository _noiThatRepository;

        public NoiThatController(INoiThatRepository noiThatRepository)
        {
            _noiThatRepository = noiThatRepository;
        }

        public async Task<IActionResult> Index()
        {
            var noiThat = await _noiThatRepository.GetAllAsync();
            return View(noiThat);
        }

        public async Task<IActionResult> Index_Admin()
        {
            var noiThat = await _noiThatRepository.GetAllAsync();
            return View(noiThat);

        }

        public async Task<IActionResult> Display(int id)
        {
            var noiThat = await _noiThatRepository.GetByIdAsync(id);
            if (noiThat == null)
            {
                return NotFound();
            }
            return View(noiThat);
        }

        public async Task<IActionResult> Add()
        {
            var noiThat = new NoiThat();
            return View(noiThat);

        }


        [HttpPost]
        public async Task<IActionResult> Add(NoiThat noiThat, IFormFile imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                // Lưu ảnh đại diện
                if (imageUrl != null)
                {
                    noiThat.ImageUrl = await SaveImage(imageUrl);
                }

                // Lưu danh sách ảnh khác nếu có
                if (imageUrls != null && imageUrls.Any())
                {
                    foreach (var file in imageUrls)
                    {
                        var imageUrlString = await SaveImage(file);
                        noiThat.ImageUrls.Add(imageUrlString);
                    }
                }



                await _noiThatRepository.AddAsync(noiThat);
                //return RedirectToAction(nameof(Index));
                return RedirectToAction(nameof(Index_Admin));

            }
            return View(noiThat);
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
            var noiThat = await _noiThatRepository.GetByIdAsync(id);
            if (noiThat == null)
            {
                return NotFound();
            }
            return View(noiThat);

        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NoiThat noiThat, IFormFile imageUrl, List<IFormFile>? imageUrls)
        {
            ModelState.Remove("ImageUrl");

            if (id != noiThat.MaNoiThat)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var existingNoiThat = await _noiThatRepository.GetByIdAsync(id);
                if (existingNoiThat == null)
                {
                    return NotFound();
                }

                long maxFileSize = 5 * 1024 * 1024;

                if (imageUrl == null)
                {
                    noiThat.ImageUrl = existingNoiThat.ImageUrl;
                }
                else
                {
                    noiThat.ImageUrl = await SaveImage(imageUrl);
                }

                if (imageUrls == null || !imageUrls.Any())
                {
                    noiThat.ImageUrls = existingNoiThat.ImageUrls ?? new List<string>();
                }
                else
                {
                    noiThat.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        if (file.Length <= maxFileSize)
                        {
                            noiThat.ImageUrls.Add(await SaveImage(file));
                        }
                    }
                }

                existingNoiThat.TenNoiThat = noiThat.TenNoiThat;
                existingNoiThat.LoaiNoiThat = noiThat.LoaiNoiThat;
                existingNoiThat.TinhTrang = noiThat.TinhTrang;
                existingNoiThat.MoTa = noiThat.MoTa;
                existingNoiThat.Gia = noiThat.Gia;
                existingNoiThat.NgayMua = noiThat.NgayMua;
                existingNoiThat.ImageUrl = noiThat.ImageUrl;
                existingNoiThat.ImageUrls = noiThat.ImageUrls;

                await _noiThatRepository.UpdateAsync(existingNoiThat);
                return RedirectToAction(nameof(Index_Admin));
            }
            return View(noiThat);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var nhaTro = await _noiThatRepository.GetByIdAsync(id);
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
                await _noiThatRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index_Admin));
            }
            return NotFound();

        }
    }
}
