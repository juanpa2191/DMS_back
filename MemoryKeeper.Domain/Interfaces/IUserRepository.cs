using MemoryKeeper.Domain.Entities;
using System.Threading.Tasks;

namespace MemoryKeeper.Domain.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<User?> ValidateUserCredentialsAsync(string email, string password);
    }
}