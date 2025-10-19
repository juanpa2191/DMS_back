using MemoryKeeper.Application.DTOs;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id);
        Task<UserDto> GetUserByEmailAsync(string email);
    }
}