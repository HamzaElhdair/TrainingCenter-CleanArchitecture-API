using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.Entities;

namespace TrainingCenter_Core.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int UserId);
        Task<User?> GetByRefreshTokenHashAsync(string refreshTokenHash);
        Task AddAsync(User user);
        void Update(User user);
        void Delete(User user);
        Task<bool> SaveChangesAsync();


    }
}
