using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFDieuKhoanRepository : IDieuKhoanRepository
    {
        private readonly ApplicationDbContext _context;

        public EFDieuKhoanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(DieuKhoan dieuKhoan)
        {
            _context.DieuKhoan.Add(dieuKhoan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dieuKhoan = await _context.DieuKhoan.FindAsync(id);
            if (dieuKhoan != null)
            {
                _context.DieuKhoan.Remove(dieuKhoan);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DieuKhoan>> GetAllAsync()
        {
            return await _context.DieuKhoan
                .ToListAsync();
        }

        public async Task<DieuKhoan> GetByIdAsync(int id)
        {
            return await _context.DieuKhoan
                .FirstOrDefaultAsync(p => p.MaDieuKhoan == id);
        }

        public async Task UpdateAsync(DieuKhoan dieuKhoan)
        {
            _context.DieuKhoan.Update(dieuKhoan);
            await _context.SaveChangesAsync();
        }
    }
}
