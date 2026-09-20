using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;
using TrainingCenter_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TrainingCenter_Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().OrderBy(u => u.UserId).ToListAsync();
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> GetByIdAsync(int UserId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }
        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() > 0);
        }
        public async Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.RefreshTokenHash == refreshTokenHash);
        }
    }
}
