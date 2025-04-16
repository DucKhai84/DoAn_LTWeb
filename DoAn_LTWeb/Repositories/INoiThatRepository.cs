using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface INoiThatRepository
    {
        Task<IEnumerable<NoiThat>> GetAllAsync();
        Task<NoiThat> GetByIdAsync(int id);
        Task AddAsync(NoiThat phongTro);
        Task UpdateAsync(NoiThat phongTro);
        Task DeleteAsync(int id);
    }
}
