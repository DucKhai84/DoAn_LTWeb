using DoAn_LTWeb.Data;
using DoAn_LTWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DoAn_LTWeb.Repositories
{
    public class EFNoiThatRepository : INoiThatRepository
    {
        private readonly ApplicationDbContext _context;

        public EFNoiThatRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NoiThat noiThat)
        {
            await _context.NoiThat.AddAsync(noiThat);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.NoiThat.FindAsync(id);
            if (entity != null)
            {
                _context.NoiThat.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

       /* public async Task<IEnumerable<NoiThat>> GetAllAsync()
        {
            return await _context.NoiThat
                 .Include(p => p.chiTietPhongTro)
                 .ToListAsync();
        }*/
        public async Task<IEnumerable<NoiThat>> GetAllAsync()
        {
            return await _context.NoiThat.ToListAsync();
        }


        public async Task<NoiThat> GetByIdAsync(int id)
        {
            return await _context.NoiThat.FindAsync(id);
        }

        public async Task UpdateAsync(NoiThat noiThat)
        {
            _context.NoiThat.Update(noiThat);
            await _context.SaveChangesAsync();
        }
    }
}
