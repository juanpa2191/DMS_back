using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Services
{
    public class ObjectService : IObjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ObjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ObjectDto> CreateObjectAsync(CreateObjectDto createObjectDto)
        {
            var obj = new Domain.Entities.Object
            {
                Name = createObjectDto.Name,
                Description = createObjectDto.Description,
                CreatedByUserId = createObjectDto.CreatedByUserId,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            var createdObject = await _unitOfWork.Objects.AddAsync(obj);
            await _unitOfWork.SaveChangesAsync();

            return await GetObjectByIdAsync(createdObject.Id);
        }

        public async Task<ObjectDto> GetObjectByIdAsync(int id)
        {
            var obj = await _unitOfWork.Objects.GetByIdAsync(id);
            if (obj == null)
            {
                throw new KeyNotFoundException($"Object with id {id} not found");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(obj.CreatedByUserId);

            return new ObjectDto
            {
                Id = obj.Id,
                Name = obj.Name,
                Description = obj.Description,
                CreatedByUserId = obj.CreatedByUserId,
                CreatedByUserName = user?.FullName ?? "Unknown",
                CreatedAt = obj.CreatedAt
            };
        }

        public async Task<IEnumerable<ObjectDto>> GetAllObjectsAsync()
        {
            var objects = await _unitOfWork.Objects.GetAllAsync();
            var objectDtos = new List<ObjectDto>();

            foreach (var obj in objects)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(obj.CreatedByUserId);
                objectDtos.Add(new ObjectDto
                {
                    Id = obj.Id,
                    Name = obj.Name,
                    Description = obj.Description,
                    CreatedByUserId = obj.CreatedByUserId,
                    CreatedByUserName = user?.FullName ?? "Unknown",
                    CreatedAt = obj.CreatedAt
                });
            }

            return objectDtos;
        }

        public async Task UpdateObjectAsync(int id, CreateObjectDto updateObjectDto)
        {
            var obj = await _unitOfWork.Objects.GetByIdAsync(id);
            if (obj == null)
            {
                throw new KeyNotFoundException($"Object with id {id} not found");
            }

            obj.Name = updateObjectDto.Name;
            obj.Description = updateObjectDto.Description;
            obj.UpdatedAt = System.DateTime.UtcNow;

            await _unitOfWork.Objects.UpdateAsync(obj);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteObjectAsync(int id)
        {
            var obj = await _unitOfWork.Objects.GetByIdAsync(id);
            if (obj == null)
            {
                throw new KeyNotFoundException($"Object with id {id} not found");
            }

            await _unitOfWork.Objects.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociateObjectToMemoryAsync(AssociateObjectToMemoryDto dto)
        {
            var memoryObject = new MemoryObject
            {
                MemoryId = dto.MemoryId,
                ObjectId = dto.ObjectId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = System.DateTime.UtcNow,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            await _unitOfWork.MemoryObjects.AddAsync(memoryObject);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}