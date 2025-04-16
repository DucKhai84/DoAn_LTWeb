using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface IPhongTroRepository
    {
        Task<IEnumerable<PhongTro>> GetAllAsync();
        Task<PhongTro> GetByIdAsync(int id);
        Task AddAsync(PhongTro phongTro);
        Task UpdateAsync(PhongTro phongTro);
        Task DeleteAsync(int id);
    }
}
