using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IChiTietThuePhongRepository
    {
        Task<IEnumerable<ChiTietThuePhong>> GetAllAsync();
        Task<ChiTietThuePhong> GetByIdAsync(int id);
        Task AddAsync(ChiTietThuePhong chiTietThuePhong);
        Task UpdateAsync(ChiTietThuePhong chiTietThuePhong);
        Task DeleteAsync(int id);
    }
}
