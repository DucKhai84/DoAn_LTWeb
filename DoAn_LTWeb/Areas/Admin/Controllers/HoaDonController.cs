using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.NETCore;
using System.Data;

namespace DoAn_LTWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HoaDonController : Controller
    {
        private readonly INhanVienRepository _nhanVienRepository;
        private readonly IHoaDonRepository _hoaDonRepository;
        private readonly IAdminRepository _khachHangRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IChiTietPhongTroRepository _chiTietPhongTroRepository;

        public HoaDonController(INhanVienRepository nhanVienRepository, IHoaDonRepository hoaDonRepository, IAdminRepository khachHangRepository, IWebHostEnvironment webHostEnvironment, IChiTietPhongTroRepository chiTietPhongTroRepository)
        {
            _nhanVienRepository = nhanVienRepository;
            _hoaDonRepository = hoaDonRepository;
            _khachHangRepository = khachHangRepository;

            _webHostEnvironment = webHostEnvironment;
            _chiTietPhongTroRepository = chiTietPhongTroRepository;
        }

        [Route("hoa-don")]
        public async Task<IActionResult> Index()
        {
            var hoaDon = await _hoaDonRepository.GetAllAsync();
            var khachHang = await _khachHangRepository.GetAllAsync();
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            ViewBag.KhachHang = new SelectList(khachHang, "MaKhachHang", "TenKhachHang");
            ViewBag.NhanVien = new SelectList(nhanVien, "MaNhanVien", "TenNhanVien"); ;
            return View(hoaDon);
        }

        public async Task<IActionResult> Add()
        {
            var hoaDon = new HoaDon();
            var khachHang = await _khachHangRepository.GetAllAsync();
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            ViewBag.KhachHang = new SelectList(khachHang, "MaKhachHang", "TenKhachHang");
            ViewBag.NhanVien = new SelectList(nhanVien, "MaNhanVien", "TenNhanVien"); ;
            return View(hoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                await _hoaDonRepository.AddAsync(hoaDon);
                return RedirectToAction(nameof(Index));
            }
            Console.WriteLine("Dữ liệu không hợp lệ!");
            // Nếu có lỗi, quay lại form thay vì redirect
            var khachHang = await _khachHangRepository.GetAllAsync();
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            ViewBag.KhachHang = new SelectList(khachHang, "MaKhachHang", "TenKhachHang");
            ViewBag.NhanVien = new SelectList(nhanVien, "MaNhanVien", "TenNhanVien");

            return View(hoaDon); // ✅ Trả về View thay vì RedirectToAction("Index")
        }


        public async Task<IActionResult> Update(int id)
        {
            var hoaDon = await _hoaDonRepository.GetByIdAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            var khachHang = await _khachHangRepository.GetAllAsync();
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            ViewBag.KhachHang = new SelectList(khachHang, "MaKhachHang", "TenKhachHang");
            ViewBag.NhanVien = new SelectList(nhanVien, "MaNhanVien", "TenNhanVien"); ;
            return View(hoaDon);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, HoaDon hoaDon)
        {

            if (id != hoaDon.MaHoaDon)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingphongTro = await _hoaDonRepository.GetByIdAsync(id);
                if (existingphongTro == null)
                {
                    return NotFound();
                }

                existingphongTro.TienPhong = hoaDon.TienPhong;
                existingphongTro.NgayLap = hoaDon.NgayLap;

                await _hoaDonRepository.UpdateAsync(existingphongTro);
                return RedirectToAction("Index");
            }

            var khachHang = await _khachHangRepository.GetAllAsync();
            var nhanVien = await _nhanVienRepository.GetAllAsync();
            ViewBag.KhachHang = new SelectList(khachHang, "MaKhachHang", "TenKhachHang");
            ViewBag.NhanVien = new SelectList(nhanVien, "MaNhanVien", "TenNhanVien"); ;
            return View(hoaDon);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var hoaDon = await _hoaDonRepository.GetByIdAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }
            return View(hoaDon);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed()
        {
            if (Request.Form["delete"].Count > 0)
            {
                int id = int.Parse(Request.Form["MaHoaDon"]);
                await _hoaDonRepository.DeleteAsync(id);
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetHoaDon()
        {
            var hoaDons = await _hoaDonRepository.GetAllAsync();

            var result = hoaDons.Select(h => new
            {
                MaHoaDon = h.MaHoaDon,
                KhachHang = h.KhachHang?.TenKhachHang ?? "N/A",
                NhanVien = h.NhanVien?.TenNhanVien ?? "N/A",
                TienPhong = h.TienPhong,
                NgayLap = h.NgayLap.ToString("yyyy-MM-dd")
            }).ToList();

            return Ok(new { data = result });
        }





        public async Task<IActionResult> PrintReport()
        {
            string renderFormat = "PDF";
            string extension = "pdf";
            string mimeType = "application/pdf";
            using var report = new LocalReport();
            var dt = new DataTable();
            dt = await GetHoaDonList();

            report.DataSources.Add(new ReportDataSource("dsHoaDon", dt));

            var parameters = new[] { new ReportParameter("param1", "Hoa Don Tien Phong") };
            report.ReportPath = $"{this._webHostEnvironment.WebRootPath}\\Reports\\rptHoaDon.rdlc";
            report.SetParameters(parameters);

            //sub 

            var pdf = report.Render(renderFormat);
            return File(pdf, mimeType, $"HoaDon.{extension}");
        }


        public async Task<DataTable> GetHoaDonList()
        {
            var dt = new DataTable();
            dt.Columns.Add("MaHoaDon", typeof(int));
            dt.Columns.Add("TenKhachHang", typeof(string));
            dt.Columns.Add("TenNhanVien", typeof(string));
            dt.Columns.Add("GiaPhong", typeof(decimal));
            dt.Columns.Add("TienPhong", typeof(decimal));
            dt.Columns.Add("SoDien", typeof(int));
            dt.Columns.Add("SoNuoc", typeof(int));
            dt.Columns.Add("NgayLap", typeof(DateTime));

            var hoaDonList = await _hoaDonRepository.GetAllAsync();
            var chiTietPhongTroList = await _chiTietPhongTroRepository.GetAllAsync();

            foreach (var item in hoaDonList)
            {
                var phongTro = chiTietPhongTroList.FirstOrDefault(); // Lấy phòng trọ đầu tiên (có thể cần chỉnh sửa)
                var row = dt.NewRow();
                row["MaHoaDon"] = item.MaHoaDon;
                row["TenKhachHang"] = item.KhachHang.TenKhachHang;
                row["TenNhanVien"] = item.NhanVien.TenNhanVien;
                row["SoDien"] = item.SoDien;
                row["SoNuoc"] = item.SoNuoc;
                row["GiaPhong"] = phongTro?.GiaPhong*100000; // Tránh lỗi null
                row["TienPhong"] = ((item.SoDien * 3500) + (item.SoNuoc * 15000) + (phongTro?.GiaPhong * 1000000 ?? 0));
                row["NgayLap"] = item.NgayLap;
                dt.Rows.Add(row);
            }


            return dt;
        }
    }
}
