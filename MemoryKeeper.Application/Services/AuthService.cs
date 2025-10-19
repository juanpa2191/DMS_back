using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace MemoryKeeper.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _unitOfWork.Users.ValidateUserCredentialsAsync(loginDto.Email, loginDto.Password);

            if (user == null)
            {
                throw new UnauthorizedAccessException("Invalid credentials");
            }

            var token = await GenerateJwtTokenAsync(new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            });

            return new LoginResponseDto
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        public async Task<UserDto> RegisterAsync(CreateUserDto createUserDto)
        {
            // Check if email already exists
            if (await _unitOfWork.Users.EmailExistsAsync(createUserDto.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = new Domain.Entities.User
            {
                Email = createUserDto.Email,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdUser = await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserDto
            {
                Id = createdUser.Id,
                Email = createdUser.Email,
                FirstName = createdUser.FirstName,
                LastName = createdUser.LastName,
                IsActive = createdUser.IsActive,
                CreatedAt = createdUser.CreatedAt
            };
        }

        public async Task<string> GenerateJwtTokenAsync(UserDto user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("userId", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecretKey12345678901234567890"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: creds);

            return await Task.FromResult(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "DefaultSecretKey12345678901234567890");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }
    }
}