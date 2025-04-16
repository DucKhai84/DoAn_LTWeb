using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAn_LTWeb.Repositories
{
    public class EFAspRepository : Controller
    {
        private readonly ApplicationDbContext _context;

        public EFAspRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IdentityUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
