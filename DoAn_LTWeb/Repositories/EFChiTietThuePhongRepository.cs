using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using DoAn_LTWeb.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFChiTietThuePhongRepository : IChiTietThuePhongRepository
    {
        private readonly ApplicationDbContext _context;

        public EFChiTietThuePhongRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChiTietThuePhong chiTietThuePhong)
        {
            if (chiTietThuePhong == null)
            {
                throw new ArgumentNullException(nameof(chiTietThuePhong), "Chi tiết phòng trọ không thể null.");
            }

            //var noiThat = await _context.NoiThat.FindAsync(chiTietPhongTro.MaNoiThat);
            //var phongTro = await _context.PhongTro.FindAsync(chiTietPhongTro.MaPhongTro);

            //if (noiThat == null)
            //{
            //    throw new KeyNotFoundException("MaNoiThat không tồn tại trong bảng NoiThat.");
            //}

            //if (phongTro == null)
            //{
            //    throw new KeyNotFoundException("MaPhongTro không tồn tại trong bảng PhongTro.");
            //}

            await _context.ChiTietThuePhong.AddAsync(chiTietThuePhong);
            await _context.SaveChangesAsync();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ChiTietThuePhong>> GetAllAsync()
        {
            return await _context.ChiTietThuePhong
           /*     .Include(ct => ct.HopDong)
                .Include(ct => ct.PhongTro)
                .Include(ct => ct.KhachHang)*/
                .ToListAsync();
        }

        public Task<ChiTietThuePhong> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ChiTietThuePhong chiTietThuePhong)
        {
            throw new NotImplementedException();
        }
    }
}
