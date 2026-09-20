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
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly AppDbContext _context;
        public EnrollmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Enrollment>> GetAllAsync()
        {
            return await _context.Enrollments.AsNoTracking().OrderBy(e => e.EnrollmentId).ToListAsync();
        }

        public async Task<Enrollment?> GetByIdAsync(int enrollmentid)
        {
            return await _context.Enrollments.Include(e => e.Course).Include(e => e.Student).FirstOrDefaultAsync(e => e.EnrollmentId == enrollmentid);
        }

        public async Task AddAsync(Enrollment enrollment)
        {
            await _context.Enrollments.AddAsync(enrollment);
        }

        public void Update(Enrollment enrollment)
        {
            _context.Enrollments.Update(enrollment);
        }

        public void Delete(Enrollment enrollment)
        {
            _context.Enrollments.Remove(enrollment);
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public async Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseid)
        {
            return await _context.Enrollments.Include(e => e.Student).Where(e => e.CourseId == courseid).ToListAsync();

        }
        public async Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentid)
        {
            return await _context.Enrollments.Include(e => e.Course).Where(e => e.StudentId == studentid).ToListAsync();
        }
    }
}
