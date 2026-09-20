using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TrainingCenter_Core.DTOs.Auth;
using TrainingCenter_Core.DTOs.User;
using TrainingCenter_Core.Entities;
using TrainingCenter_Core.Interfaces;

namespace TrainingCenter_Core.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;

        }
        private static UserResponseDto MapToResponseDto(User user)
        {
            return new UserResponseDto(
                  user.UserId,
                  user.Email,
                  user.Role,
                  user.IsActive,
                  user.CreatedAt

            );
        }

        public async Task<TokenResponse?> LoginAsync(LoginUserDto loginUserDto)
        {

            var user = await _userRepository.GetByEmailAsync(loginUserDto.Email);
            if (user == null) return null;

            bool IsPasswordValid = BCrypt.Net.BCrypt.Verify(loginUserDto.Password, user.PasswordHash);
            if (!IsPasswordValid) return null;

            var accessToken = GenerateJwtToken(user);

            var refreshToken = GenerateRefreshToken();
            user.RefreshTokenHash = ComputeSha256Hash(refreshToken);
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            user.RefreshTokenRevokedAt = null;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();


            return new TokenResponse(accessToken, refreshToken);
        }
        public async Task<TokenResponse?> RefreshTokenAsync(RefreshRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
                return null;


            var incomingTokenHash = ComputeSha256Hash(request.RefreshToken);


            var user = await _userRepository.GetByRefreshTokenHashAsync(incomingTokenHash);

            if (user == null || user.RefreshTokenExpiresAt <= DateTime.UtcNow || user.RefreshTokenRevokedAt != null)
            {
                return null;
            }


            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshTokenHash = ComputeSha256Hash(newRefreshToken);
            user.RefreshTokenExpiresAt = DateTime.UtcNow.AddDays(7);
            user.RefreshTokenRevokedAt = null;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return new TokenResponse(newAccessToken, newRefreshToken);
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;


            user.RefreshTokenHash = null;
            user.RefreshTokenExpiresAt = null;
            user.RefreshTokenRevokedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            return await _userRepository.SaveChangesAsync();
        }

        private static string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string ComputeSha256Hash(string rawData)
        {
            using var sha256Hash = SHA256.Create();
            var bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                  new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                  new Claim(ClaimTypes.Email, user.Email),
                  new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllAsync();


            return users.Select(MapToResponseDto);
        }

        public async Task<UserResponseDto?> GetUserByIdAsync(int UserId)
        {
            var user = await _userRepository.GetByIdAsync(UserId);
            if (user == null)
                return null;

            return MapToResponseDto(user);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return null;

            return MapToResponseDto(user);
        }

        public async Task<UserResponseDto> CreateUserAsync(CreateUserDto createUserDto)
        {

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User
            {
                Email = createUserDto.Email,
                PasswordHash = passwordHash,
                Role = createUserDto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return MapToResponseDto(user);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int UserId, UpdateUserDto dto)
        {

            var user = await _userRepository.GetByIdAsync(UserId);
            if (user == null) return null;

            user.Email = dto.Email;

            user.Role = dto.Role;
            user.IsActive = dto.IsActive;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();


            return MapToResponseDto(user);
        }

        public async Task<bool> DeleteUserAsync(int UserId)
        {
            var user = await _userRepository.GetByIdAsync(UserId);
            if (user == null) return false;

            _userRepository.Delete(user);
            return await _userRepository.SaveChangesAsync();
        }
    }
}
