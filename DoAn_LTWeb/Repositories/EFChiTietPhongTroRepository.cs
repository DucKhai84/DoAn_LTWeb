using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Repositories
{
    public class EFChiTietPhongTroRepository : IChiTietPhongTroRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChiTietPhongTroRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChiTietPhongTro chiTietPhongTro)
        {
            if (chiTietPhongTro == null)
            {
                throw new ArgumentNullException(nameof(chiTietPhongTro), "Chi tiết phòng trọ không thể null.");
            }

            var noiThat = await _context.NoiThat.FindAsync(chiTietPhongTro.MaNoiThat);
            var phongTro = await _context.PhongTro.FindAsync(chiTietPhongTro.MaPhongTro);

            if (noiThat == null)
            {
                throw new KeyNotFoundException("MaNoiThat không tồn tại trong bảng NoiThat.");
            }

            if (phongTro == null)
            {
                throw new KeyNotFoundException("MaPhongTro không tồn tại trong bảng PhongTro.");
            }

            await _context.ChiTietPhongTro.AddAsync(chiTietPhongTro);
            await _context.SaveChangesAsync();
        }



        public async Task DeleteAsync(int id)
        {
            var chiTiet = await _context.ChiTietPhongTro
                .Include(p => p.NoiThat)
                .Include(p => p.PhongTro)
                .FirstOrDefaultAsync(p => p.MaChiTietPhongTro == id);

            if (chiTiet != null)
            {
                _context.ChiTietPhongTro.Remove(chiTiet);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<ChiTietPhongTro>> GetAllAsync()
        {
            return await _context.ChiTietPhongTro
                .Include(p => p.NoiThat)
                .Include(p => p.PhongTro)
                .Include(p => p.PhongTro.NhaTro)
                .ToListAsync();
        }


        public async Task<ChiTietPhongTro> GetByIdAsync(int id)
        {
            return await _context.ChiTietPhongTro
                .Include(p => p.NoiThat)
                .Include(p => p.PhongTro)
                .FirstOrDefaultAsync(p => p.MaChiTietPhongTro == id);
        }

        public async Task UpdateAsync(ChiTietPhongTro chiTietPhongTro)
        {
            var existing = await _context.ChiTietPhongTro.FindAsync(chiTietPhongTro.MaChiTietPhongTro);
            if (existing == null)
            {
                throw new KeyNotFoundException("Chi tiết phòng trọ không tồn tại.");
            }

            var noiThat = await _context.NoiThat.FindAsync(chiTietPhongTro.MaNoiThat);
            var phongTro = await _context.PhongTro.FindAsync(chiTietPhongTro.MaPhongTro);

            if (noiThat == null)
            {
                throw new KeyNotFoundException("MaNoiThat không tồn tại trong bảng NoiThat.");
            }

            if (phongTro == null)
            {
                throw new KeyNotFoundException("MaPhongTro không tồn tại trong bảng PhongTro.");
            }

            _context.Entry(existing).CurrentValues.SetValues(chiTietPhongTro);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
