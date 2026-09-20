using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;

namespace TrainingCenter_Core.Interfaces
{
    public interface IInstructorRepository
    {
        Task<IEnumerable<Instructor>> GetAllAsync();
        Task<Instructor?> GetByIdAsync(int id);
        Task AddAsync(Instructor instructor);
        void Update(Instructor instructor);
        void Delete(Instructor instructor);
        Task<bool> SaveChangesAsync();
        Task<Instructor?> GetByIdWithUserId(int UserId);
    }
}
