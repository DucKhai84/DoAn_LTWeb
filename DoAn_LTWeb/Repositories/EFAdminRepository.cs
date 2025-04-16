
using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Repositories
{
    public class KhachHangRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public KhachHangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<KhachHang>> GetAllAsync()
        {
            return await _context.KhachHang
                .ToListAsync();
        }

        public async Task<KhachHang> GetByIdAsync(int id)
        {
            return await _context.KhachHang.FirstOrDefaultAsync(kh => kh.MaKhachHang == id);
        }

        public async Task AddAsync(KhachHang khachHang)
        {
            await _context.KhachHang.AddAsync(khachHang);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(KhachHang khachHang)
        {
            _context.KhachHang.Update(khachHang);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var khachHang = await _context.KhachHang.FindAsync(id);
            if (khachHang != null)
            {
                _context.KhachHang.Remove(khachHang);
                await _context.SaveChangesAsync();
            }
        }
    }
}
