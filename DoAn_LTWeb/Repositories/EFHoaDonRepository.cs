using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFHoaDonRepository : IHoaDonRepository
    {
        private readonly ApplicationDbContext _context;

        public EFHoaDonRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(HoaDon hoaDon)
        {
            _context.HoaDon.Add(hoaDon);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hoaDon = await _context.HoaDon.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDon.Remove(hoaDon);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<HoaDon>> GetAllAsync()
        {
            return await _context.HoaDon
                .Include(p => p.NhanVien)
                .Include(p => p.KhachHang)
                .ToListAsync();
        }
        public async Task<IEnumerable<HoaDon>> GetAllByKhachHangAsync(int maKhachHang)
        {
            return await _context.HoaDon
                .Include(h => h.KhachHang)
                .Include(h => h.NhanVien)
                .Where(h => h.MaKhachHang == maKhachHang)
                .ToListAsync();
        }



        public async Task<HoaDon> GetByIdAsync(int id)
        {
            return await _context.HoaDon
                .Include(p => p.NhanVien)
                .Include(p => p.KhachHang)
                .FirstOrDefaultAsync(p => p.MaHoaDon == id);
        }

        public async Task UpdateAsync(HoaDon hoaDon)
        {
            _context.HoaDon.Update(hoaDon);
            await _context.SaveChangesAsync();
        }
    }
}
