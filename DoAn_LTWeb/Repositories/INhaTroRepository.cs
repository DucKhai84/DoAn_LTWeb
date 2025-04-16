using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface INhaTroRepository
    {
        Task<IEnumerable<NhaTro>> GetAllAsync();
        Task<NhaTro> GetByIdAsync(int id);
        Task AddAsync(NhaTro nhaTro);
        Task UpdateAsync(NhaTro nhaTro);
        Task DeleteAsync(int id);
    }
}
