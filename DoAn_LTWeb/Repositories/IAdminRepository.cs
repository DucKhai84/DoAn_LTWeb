using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IAdminRepository
    {
        Task<IEnumerable<KhachHang>> GetAllAsync();
        Task<KhachHang> GetByIdAsync(int id);
        Task AddAsync(KhachHang khachHang);
        Task UpdateAsync(KhachHang khachHang);
        Task DeleteAsync(int id);
    }
}
