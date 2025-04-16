using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFVehicalRepository : IVehicalRepository
    {
        private readonly ApplicationDbContext _context;

        public EFVehicalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy danh sách tất cả Xe
        public async Task<IEnumerable<Xe>> GetAllAsync()
        {
            return await _context.Xe
                .Include(x => x.KhachHang) // Đảm bảo lấy thông tin khách hàng
                .Select(x => new Xe
                {
                    MaXe = x.MaXe,
                    BienSoXe = x.BienSoXe,
                    CMND = x.CMND,
                    MaNhaXe = x.MaNhaXe,
                    MaKhachHang = x.MaKhachHang,
                    KhachHang = new KhachHang { TenKhachHang = x.KhachHang.TenKhachHang } // Chỉ lấy tên khách hàng
                })
                .ToListAsync();
        }


        // Lấy Xe theo ID
        public async Task<Xe> GetByIdAsync(int id)
        {
            return await _context.Xe.FindAsync(id);
        }

        // Thêm mới Xe
        public async Task AddAsync(Xe xe)
        {
            await _context.Xe.AddAsync(xe);
            await _context.SaveChangesAsync();
        }

        // Cập nhật thông tin Xe
        public async Task UpdateAsync(Xe xe)
        {
            _context.Xe.Update(xe);
            await _context.SaveChangesAsync();
        }

        // Xóa Xe theo ID
        public async Task DeleteAsync(int id)
        {
            var xe = await _context.Xe.FindAsync(id);
            if (xe != null)
            {
                _context.Xe.Remove(xe);
                await _context.SaveChangesAsync();
            }
        }

     

    }
}
