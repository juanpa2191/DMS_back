using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Interfaces;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id {id} not found");
            }

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with email {email} not found");
            }

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt
            };
        }
    }
}