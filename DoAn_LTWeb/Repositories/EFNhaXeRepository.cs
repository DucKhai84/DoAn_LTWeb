using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFNhaXeRepository : INhaXeRepository
    {
        private readonly ApplicationDbContext _context;

        public EFNhaXeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<NhaXe>> GetAllAsync()
        {
            return await _context.NhaXe
                .ToListAsync();
        }

    }
}
