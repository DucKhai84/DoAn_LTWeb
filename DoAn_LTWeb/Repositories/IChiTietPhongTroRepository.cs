using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IChiTietPhongTroRepository
    {
        Task<IEnumerable<ChiTietPhongTro>> GetAllAsync();
        Task<ChiTietPhongTro> GetByIdAsync(int id);
        Task AddAsync(ChiTietPhongTro chiTietPhongTro);
        Task UpdateAsync(ChiTietPhongTro chiTietPhongTro);
        Task DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
