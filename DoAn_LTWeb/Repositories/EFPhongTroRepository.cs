using Microsoft.EntityFrameworkCore;
using DoAn_LTWeb.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DoAn_LTWeb.Data;

namespace DoAn_LTWeb.Repositories
{
    public class EFPhongTroRepository : IPhongTroRepository
    {
        private readonly ApplicationDbContext _context;

        public EFPhongTroRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhongTro>> GetAllAsync()
        {
            return await _context.PhongTro
                .Include(p => p.NhaTro) 
                .ToListAsync();
        }

        public async Task<PhongTro> GetByIdAsync(int id)
        {
            return await _context.PhongTro.Include(p => p.NhaTro).FirstOrDefaultAsync(p => p.MaPhongTro == id);
        }

        public async Task AddAsync(PhongTro phongTro)
        {
            _context.PhongTro.Add(phongTro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhongTro phongTro)
        {
            _context.PhongTro.Update(phongTro);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var phongTro = await _context.PhongTro.FindAsync(id);
            if (phongTro != null)
            {
                _context.PhongTro.Remove(phongTro);
                await _context.SaveChangesAsync();
            }
        }
    }
}
