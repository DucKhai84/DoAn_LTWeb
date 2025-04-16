using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface INhanVienRepository
    {
        Task<IEnumerable<NhanVien>> GetAllAsync();
        Task<NhanVien> GetByIdAsync(int id);
        Task AddAsync(NhanVien phongTro);
        Task UpdateAsync(NhanVien phongTro);
        Task DeleteAsync(int id);
    }
}
