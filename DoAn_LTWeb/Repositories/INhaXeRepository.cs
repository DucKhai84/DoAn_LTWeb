using DoAn_LTWeb.Models;

namespace DoAn_LTWeb.Repositories
{
    public interface INhaXeRepository 
    {
        Task<IEnumerable<NhaXe>> GetAllAsync();
    }
}
