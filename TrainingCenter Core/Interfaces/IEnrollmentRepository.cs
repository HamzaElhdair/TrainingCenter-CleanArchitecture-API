using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;

namespace TrainingCenter_Core.Interfaces
{
    public interface IEnrollmentRepository
    {
        Task<IEnumerable<Enrollment>> GetAllAsync();
        Task<Enrollment?> GetByIdAsync(int enrollmentid);
        Task AddAsync(Enrollment enrollment);
        void Update(Enrollment enrollment);
        void Delete(Enrollment enrollment);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Enrollment>> GetByCourseIdAsync(int courseid);
        Task<IEnumerable<Enrollment>> GetByStudentIdAsync(int studentid);
    }
}

