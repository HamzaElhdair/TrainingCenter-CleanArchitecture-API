using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;
using TrainingCenter_Infrastructure.Data;

namespace TrainingCenter_Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.AsNoTracking().OrderBy(c => c.CourseId).ToListAsync();
        }
        public async Task<Course?> GetByIdAsync(int courseId)
        {
            return await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == courseId);
        }
        public async Task AddAsync(Course course)
        {
            await _context.Courses.AddAsync(course);
        }
        public void Update(Course course)
        {
            _context.Courses.Update(course);
        }
        public void Delete(Course course)
        {
            _context.Courses.Remove(course);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }
    }
}
