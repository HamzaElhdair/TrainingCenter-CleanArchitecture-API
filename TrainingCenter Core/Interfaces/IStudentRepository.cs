using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;

namespace TrainingCenter_Core.Interfaces
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int StudentId);
        Task AddAsync(Student student);
        void Update(Student student);
        void Delete(Student student);
        Task<bool> SaveChangesAsync();
        Task<Student?> GetByIdWithProfileAsync(int StudentId);
        Task<Student?> GetByIdWithUserId(int UserId);
    }
}
