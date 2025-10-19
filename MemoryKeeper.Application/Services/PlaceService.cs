using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Services
{
    public class PlaceService : IPlaceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlaceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PlaceDto> CreatePlaceAsync(CreatePlaceDto createPlaceDto)
        {
            var place = new Place
            {
                Name = createPlaceDto.Name,
                Description = createPlaceDto.Description,
                Address = createPlaceDto.Address,
                Latitude = createPlaceDto.Latitude,
                Longitude = createPlaceDto.Longitude,
                CreatedByUserId = createPlaceDto.CreatedByUserId,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            var createdPlace = await _unitOfWork.Places.AddAsync(place);
            await _unitOfWork.SaveChangesAsync();

            return await GetPlaceByIdAsync(createdPlace.Id);
        }

        public async Task<PlaceDto> GetPlaceByIdAsync(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null)
            {
                throw new KeyNotFoundException($"Place with id {id} not found");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(place.CreatedByUserId);

            return new PlaceDto
            {
                Id = place.Id,
                Name = place.Name,
                Description = place.Description,
                Address = place.Address,
                Latitude = place.Latitude,
                Longitude = place.Longitude,
                CreatedByUserId = place.CreatedByUserId,
                CreatedByUserName = user?.FullName ?? "Unknown",
                CreatedAt = place.CreatedAt
            };
        }

        public async Task<IEnumerable<PlaceDto>> GetAllPlacesAsync()
        {
            var places = await _unitOfWork.Places.GetAllAsync();
            var placeDtos = new List<PlaceDto>();

            foreach (var place in places)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(place.CreatedByUserId);
                placeDtos.Add(new PlaceDto
                {
                    Id = place.Id,
                    Name = place.Name,
                    Description = place.Description,
                    Address = place.Address,
                    Latitude = place.Latitude,
                    Longitude = place.Longitude,
                    CreatedByUserId = place.CreatedByUserId,
                    CreatedByUserName = user?.FullName ?? "Unknown",
                    CreatedAt = place.CreatedAt
                });
            }

            return placeDtos;
        }

        public async Task UpdatePlaceAsync(int id, CreatePlaceDto updatePlaceDto)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null)
            {
                throw new KeyNotFoundException($"Place with id {id} not found");
            }

            place.Name = updatePlaceDto.Name;
            place.Description = updatePlaceDto.Description;
            place.Address = updatePlaceDto.Address;
            place.Latitude = updatePlaceDto.Latitude;
            place.Longitude = updatePlaceDto.Longitude;
            place.UpdatedAt = System.DateTime.UtcNow;

            await _unitOfWork.Places.UpdateAsync(place);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePlaceAsync(int id)
        {
            var place = await _unitOfWork.Places.GetByIdAsync(id);
            if (place == null)
            {
                throw new KeyNotFoundException($"Place with id {id} not found");
            }

            await _unitOfWork.Places.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociatePlaceToMemoryAsync(AssociatePlaceToMemoryDto dto)
        {
            var memoryPlace = new MemoryPlace
            {
                MemoryId = dto.MemoryId,
                PlaceId = dto.PlaceId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = System.DateTime.UtcNow,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            await _unitOfWork.MemoryPlaces.AddAsync(memoryPlace);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}