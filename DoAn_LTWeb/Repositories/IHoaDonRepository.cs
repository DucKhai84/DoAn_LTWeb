using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IHoaDonRepository
    {
        Task<IEnumerable<HoaDon>> GetAllAsync();
        Task<HoaDon> GetByIdAsync(int id);
        Task AddAsync(HoaDon hoaDon);
        Task UpdateAsync(HoaDon hoaDon);
        Task DeleteAsync(int id);
      
        Task<IEnumerable<HoaDon>> GetAllByKhachHangAsync(int maKhachHang);
        

    }
}
