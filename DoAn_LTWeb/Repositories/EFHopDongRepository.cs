using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Repositories
{
    public class EFHopDongRepository : IHopDongRepository
    {
        private readonly ApplicationDbContext _context;

        public EFHopDongRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(HopDong hopDong)
        {
            _context.HopDong.Add(hopDong);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var hopDong = await _context.HopDong.FindAsync(id);
            if (hopDong != null)
            {
                _context.HopDong.Remove(hopDong);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<HopDong>> GetAllAsync()
        {
            return await _context.HopDong
                .ToListAsync();
        }
        public async Task<HopDong> GetByIdAsync(int id)
        {
            return await _context.HopDong
                .FirstOrDefaultAsync(p => p.MaHopDong == id);
        }

        public async Task UpdateAsync(HopDong hopDong)
        {
            _context.HopDong.Update(hopDong);
            await _context.SaveChangesAsync();
        }
    }
}
