using MemoryKeeper.Application.DTOs;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> LoginAsync(LoginDto loginDto);
        Task<UserDto> RegisterAsync(CreateUserDto createUserDto);
        Task<string> GenerateJwtTokenAsync(UserDto user);
        Task<bool> ValidateTokenAsync(string token);
    }
}