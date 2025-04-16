using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IDieuKhoanRepository
    {
        Task<IEnumerable<DieuKhoan>> GetAllAsync();
        Task<DieuKhoan> GetByIdAsync(int id);
        Task AddAsync(DieuKhoan dieuKhoan);
        Task UpdateAsync(DieuKhoan dieuKhoan);
        Task DeleteAsync(int id);
    }
}
