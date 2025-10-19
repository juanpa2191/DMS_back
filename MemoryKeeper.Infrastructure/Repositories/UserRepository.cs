using Microsoft.EntityFrameworkCore;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Interfaces;
using MemoryKeeper.Infrastructure.Data;
using System.Threading.Tasks;

namespace MemoryKeeper.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MemoryKeeperDbContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<User?> ValidateUserCredentialsAsync(string email, string password)
        {
            var user = await GetByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }
    }
}