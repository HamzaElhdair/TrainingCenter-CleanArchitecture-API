using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingCenter_Core.DTOs.Auth;
using TrainingCenter_Core.DTOs.User;

namespace TrainingCenter_Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDto>> GetAllUsersAsync();
        Task<UserResponseDto?> GetUserByIdAsync(int UserId);
        Task<UserResponseDto?> GetUserByEmailAsync(string email);
        Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserResponseDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int UserId);
        Task<TokenResponse?> LoginAsync(LoginUserDto loginUserDto);
        Task<TokenResponse?> RefreshTokenAsync(RefreshRequest request);
        Task<bool> LogoutAsync(int userId);

    }
}
