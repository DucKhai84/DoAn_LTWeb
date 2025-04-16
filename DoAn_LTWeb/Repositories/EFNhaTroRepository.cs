using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Repositories
{
    public class EFNhaTroRepository : INhaTroRepository
    {
        private readonly ApplicationDbContext _context;

        public EFNhaTroRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NhaTro>> GetAllAsync()
        {
            return await _context.NhaTro
                .Include(p => p.PhongTro) // Include thông tin về NhaTro
                .ToListAsync();
        }

        public async Task<NhaTro> GetByIdAsync(int id)
        {
            return await _context.NhaTro.Include(p => p.PhongTro).FirstOrDefaultAsync(p => p.MaNhaTro == id);
        }

        public async Task AddAsync(NhaTro nhaTro)
        {
            _context.NhaTro.Add(nhaTro);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(NhaTro nhaTro)
        {
            _context.NhaTro.Update(nhaTro);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var nhaTro = await _context.NhaTro.FindAsync(id);
            if (nhaTro != null)
            {
                _context.NhaTro.Remove(nhaTro);
                await _context.SaveChangesAsync();
            }
        }

        //public async Task DeleteAsync(int id)
        //{
        //    var nhaTro = await _context.NhaTro
        //        .Include(n => n.PhongTro) // Include để lấy danh sách phòng trọ trước khi xóa
        //        .FirstOrDefaultAsync(n => n.MaNhaTro == id);

        //    if (nhaTro != null)
        //    {
        //        // Xóa các phòng trọ trước khi xóa nhà trọ
        //        _context.PhongTro.RemoveRange(nhaTro.PhongTro);

        //        _context.NhaTro.Remove(nhaTro);
        //        await _context.SaveChangesAsync();
        //    }
        //}

    }
}
