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
    public class InstructorRepository : IInstructorRepository
    {
        private readonly AppDbContext _context;
        public InstructorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await _context.Instructors.AsNoTracking().OrderBy(i => i.InstructorId).ToListAsync();
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
            return await _context.Instructors.FirstOrDefaultAsync(i => i.InstructorId == id);
        }
        public async Task AddAsync(Instructor instructor)
        {
            await _context.Instructors.AddAsync(instructor);
        }
        public void Update(Instructor instructor)
        {
            _context.Instructors.Update(instructor);
        }
        public void Delete(Instructor instructor)
        {
            _context.Instructors.Remove(instructor);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Instructor?> GetByIdWithUserId(int UserId)
        {
            return await _context.Instructors.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == UserId);
        }
    }
}
