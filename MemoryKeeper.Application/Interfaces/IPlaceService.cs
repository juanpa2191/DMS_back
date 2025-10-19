using MemoryKeeper.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IPlaceService
    {
        Task<IEnumerable<PlaceDto>> GetAllPlacesAsync();
        Task<PlaceDto> GetPlaceByIdAsync(int id);
        Task<PlaceDto> CreatePlaceAsync(CreatePlaceDto createPlaceDto);
        Task AssociatePlaceToMemoryAsync(AssociatePlaceToMemoryDto dto);
    }
}