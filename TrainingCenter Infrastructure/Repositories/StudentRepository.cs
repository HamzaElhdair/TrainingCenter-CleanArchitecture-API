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
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _context.Students.AsNoTracking().OrderBy(s => s.StudentId).ToListAsync();
        }
        public async Task<Student?> GetByIdAsync(int StudentId)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == StudentId);
        }
        public async Task AddAsync(Student student)
        {
            await _context.Students.AddAsync(student);
        }
        public void Update(Student student)
        {
            _context.Students.Update(student);
        }
        public void Delete(Student student)
        {
            _context.Students.Remove(student);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<Student?> GetByIdWithProfileAsync(int StudentId)
        {
            return await _context.Students.Include(s => s.StudentProfile).FirstOrDefaultAsync(s => s.StudentId == StudentId);
        }
        public async Task<Student?> GetByIdWithUserId(int UserId)
        {
            return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == UserId);
        }

    }
}
