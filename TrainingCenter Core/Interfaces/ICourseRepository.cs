using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;

namespace TrainingCenter_Core.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAllAsync();
        Task<Course?> GetByIdAsync(int CourseId);
        Task AddAsync(Course course);
        void Update(Course course);
        void Delete(Course course);
        Task<bool> SaveChangesAsync();
    }
}
