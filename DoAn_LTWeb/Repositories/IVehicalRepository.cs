using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IVehicalRepository
    {
        Task<IEnumerable<Xe>> GetAllAsync();
        Task<Xe> GetByIdAsync(int id);
        Task AddAsync(Xe khachHang);
        Task UpdateAsync(Xe khachHang);
        Task DeleteAsync(int id);

    }
}
