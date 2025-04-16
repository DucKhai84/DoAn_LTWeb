using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class NhanVienController : Controller
    {
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly INhaTroRepository _nhaTroRepository;

        public NhanVienController(INhanVienRepository nhanVienRepository,INhaTroRepository nhaTroRepository)
        {
            _nhanVienRepository = nhanVienRepository;
            _nhaTroRepository = nhaTroRepository;
        }

        public async Task<IActionResult> Index()
        {
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            return View(nhanVien);
        }

        public async Task<IActionResult> Index_Admin()
        {
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            return View(nhanVien);

        }

        public async Task<IActionResult> Add()
        {
            var nhanVien = new NhanVien();
            var nhaTro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTro = new SelectList(nhaTro, "MaNhaTro", "TenNhaTro"); ;
            /* ViewBag.Genders = new SelectList(new[] { "Nam", "Nữ" });*/
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Add(NhanVien nhanVien, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            if (ModelState.IsValid)
            {
                // Lưu ảnh đại diện
                if (imageUrl != null)
                {
                    nhanVien.ImageUrl = await SaveImage(imageUrl);
                }

                // Lưu danh sách ảnh khác nếu có
                if (imageUrls != null && imageUrls.Any())
                {
                    foreach (var file in imageUrls)
                    {
                        var imageUrlString = await SaveImage(file);
                        nhanVien.ImageUrls.Add(imageUrlString);
                    }
                }

                await _nhanVienRepository.AddAsync(nhanVien);
                return RedirectToAction(nameof(Index_Admin));

            }

            var nhatro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTro = new SelectList(nhatro, "MaNhaTro", "TenNhaTro");
            return View(nhanVien);
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
            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            var nhaTro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTro = new SelectList(nhaTro, "MaNhaTro", "TenNhaTro");
            ViewBag.Genders = new SelectList(new[] { "Nam", "Nữ" });
            return View(nhanVien);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, NhanVien nhanVien, IFormFile? imageUrl, List<IFormFile>? imageUrls)
        {
            ModelState.Remove("ImageUrl"); // Loại bỏ xác thực ModelState cho ImageUrl

            if (id != nhanVien.MaNhanVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingnhanVien = await _nhanVienRepository.GetByIdAsync(id);
                if (existingnhanVien == null)
                {
                    return NotFound();
                }

                long maxFileSize = 5 * 1024 * 1024; // 5MB

                // Giữ nguyên thông tin hình ảnh nếu không có hình mới được tải lên
                if (imageUrl == null)
                {
                    nhanVien.ImageUrl = existingnhanVien.ImageUrl;
                }
                else if (imageUrl.Length <= maxFileSize)
                {
                    nhanVien.ImageUrl = await SaveImage(imageUrl);
                }

                // Cập nhật danh sách ảnh phụ
                if (imageUrls == null || !imageUrls.Any())
                {
                    nhanVien.ImageUrls = existingnhanVien.ImageUrls;
                }
                else
                {
                    nhanVien.ImageUrls = new List<string>();
                    foreach (var file in imageUrls)
                    {
                        if (file.Length <= maxFileSize)
                        {
                            nhanVien.ImageUrls.Add(await SaveImage(file));
                        }
                    }
                }

                existingnhanVien.TenNhanVien = nhanVien.TenNhanVien;
                existingnhanVien.SoDienThoai = nhanVien.SoDienThoai;
                existingnhanVien.CMND = nhanVien.CMND;
                existingnhanVien.Email = nhanVien.Email;
                existingnhanVien.GioiTinh = nhanVien.GioiTinh;
                existingnhanVien.NgaySinh = nhanVien.NgaySinh;
                existingnhanVien.NgayVaoLam = nhanVien.NgayVaoLam;
                existingnhanVien.ImageUrl = nhanVien.ImageUrl;
                existingnhanVien.ImageUrls = nhanVien.ImageUrls;

                await _nhanVienRepository.UpdateAsync(existingnhanVien);
                return RedirectToAction("Index_Admin");
            }

            var nhaTro = await _nhaTroRepository.GetAllAsync();
            ViewBag.NhaTro = new SelectList(nhaTro, "MaNhaTro", "TenNhaTro");
            return View(nhanVien);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var nhanVien = await _nhanVienRepository.GetByIdAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            return View(nhanVien);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed()
        {
            if (Request.Form["delete"].Count > 0)
            {
                int id = int.Parse(Request.Form["MaNhanVien"]);
                await _nhanVienRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index_Admin));
            }
            return NotFound();
        }


    }
}
