using MemoryKeeper.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IObjectService
    {
        Task<IEnumerable<ObjectDto>> GetAllObjectsAsync();
        Task<ObjectDto> GetObjectByIdAsync(int id);
        Task<ObjectDto> CreateObjectAsync(CreateObjectDto createObjectDto);
        Task AssociateObjectToMemoryAsync(AssociateObjectToMemoryDto dto);
    }
}