using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DieuKhoanController : Controller
    {
        private readonly IHopDongRepository _hopdongrepository;
        private readonly IDieuKhoanRepository _dieukhoanrepository;


        public DieuKhoanController(IHopDongRepository hopdongrepository, IDieuKhoanRepository dieukhoanrepository)
        {
            _hopdongrepository = hopdongrepository;
            _dieukhoanrepository = dieukhoanrepository;
        }

        public async Task<IActionResult> Success()
        {
            return View();
        }
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index(int id)
        {
            var dieuKhoanList = await _dieukhoanrepository.GetAllAsync();
            ViewBag.MaHopDong = id;
            if (dieuKhoanList == null || !dieuKhoanList.Any())
            {
                ViewBag.DieuKhoan = new List<DoAn_LTWeb.Models.DieuKhoan>();
            }
            else
            {
                ViewBag.DieuKhoan = dieuKhoanList;
            }

            return View();
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> IndexAdmin()
        {
            var dieuKhoanList = await _dieukhoanrepository.GetAllAsync();
            return View(dieuKhoanList);
        }

        [HttpGet]
        public async Task<IActionResult> GetDieuKhoans()
        {
            var dieuKhoans = await _dieukhoanrepository.GetAllAsync();
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver(),
                Formatting = Formatting.Indented,
            };
            return Content(JsonConvert.SerializeObject(new { data = dieuKhoans }, settings), "application/json; charset=utf-8");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add()
        {
            var dieuKhoanList = await _dieukhoanrepository.GetAllAsync();
            return View(dieuKhoanList);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Add(DieuKhoan dieuKhoan)
        {
            if (!ModelState.IsValid)
            {
                return View();

            }
            await _dieukhoanrepository.AddAsync(dieuKhoan);

            return RedirectToAction("Success", "DieuKhoan");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id)
        {
            var nhaTro = await _dieukhoanrepository.GetByIdAsync(id);
            if (nhaTro == null)
            {
                return NotFound();
            }
            return View(nhaTro);

        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, DieuKhoan nhaTro, string NoiDung)
        {

            var existingNhaTro = await _dieukhoanrepository.GetByIdAsync(id);

            if (existingNhaTro == null) { return NotFound("Mã điều khoản hiện không tồn tại !!"); }

            if (!ModelState.IsValid)
            {
                return View();

            }

            if (existingNhaTro == null)
            {
                return NotFound();
            }


            existingNhaTro.NoiDung = nhaTro.NoiDung;

            await _dieukhoanrepository.UpdateAsync(existingNhaTro);

            return RedirectToAction("Success", "DieuKhoan");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var dieuKhoan = await _dieukhoanrepository.GetByIdAsync(id);
            if (dieuKhoan == null)
            {
                return NotFound();
            }
            return View(dieuKhoan);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dieuKhoan = await _dieukhoanrepository.GetByIdAsync(id);
            if (dieuKhoan == null)
            {
                return NotFound("Lỗi: Không tìm thấy điều khoản để xóa.");
            }

            await _dieukhoanrepository.DeleteAsync(id);
            return RedirectToAction("Success", "DieuKhoan");

        }
    }
}
