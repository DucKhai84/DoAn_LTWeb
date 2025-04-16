using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFNhanVienRepository : INhanVienRepository
    {
        private readonly ApplicationDbContext _context;
        public EFNhanVienRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NhanVien nhanVien)
        {
            _context.NhanVien.Add(nhanVien);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nhanVien = await _context.NhanVien.FindAsync(id);
            if (nhanVien != null)
            {
                _context.NhanVien.Remove(nhanVien);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<NhanVien>> GetAllAsync()
        {
            return await _context.NhanVien
               .Include(p => p.NhaTro)
               .ToListAsync();
        }

        public  async Task<NhanVien> GetByIdAsync(int id)
        {
            return await _context.NhanVien.Include(p => p.NhaTro).FirstOrDefaultAsync(p => p.MaNhanVien == id);
        }

        public async Task UpdateAsync(NhanVien nhanVien)
        {
            _context.NhanVien.Update(nhanVien);
            await _context.SaveChangesAsync();
        }
    }
}
