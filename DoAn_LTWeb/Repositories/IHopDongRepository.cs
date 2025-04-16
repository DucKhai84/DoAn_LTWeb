using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IHopDongRepository
    {
        Task<IEnumerable<HopDong>> GetAllAsync();
        Task<HopDong> GetByIdAsync(int id);
        Task AddAsync(HopDong hopDong);
        Task UpdateAsync(HopDong hopDong);
        Task DeleteAsync(int id);
    }
}

