using MemoryKeeper.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IMemoryService
    {
        Task<MemoryDto> CreateMemoryAsync(CreateMemoryDto createMemoryDto);
        Task<MemoryDto> UpdateMemoryAsync(UpdateMemoryDto updateMemoryDto);
        Task<MemoryDto> GetMemoryByIdAsync(int id);
        Task<MemoryDto> GetMemoryWithDetailsAsync(int id);
        Task<IEnumerable<MemoryDto>> GetMemoriesByUserIdAsync(int userId);
        Task<IEnumerable<MemoryDto>> SearchMemoriesAsync(MemorySearchDto searchDto);
        Task<MemoryDto> ConfirmMemoryAsync(ConfirmMemoryDto confirmMemoryDto);
        Task DeleteMemoryAsync(int id);
        
        // Asociaciones
        Task AssociatePlaceToMemoryAsync(AssociatePlaceToMemoryDto dto);
        Task AssociateObjectToMemoryAsync(AssociateObjectToMemoryDto dto);
        Task AssociateNoteToMemoryAsync(AssociateNoteToMemoryDto dto);
        Task AssociatePersonToMemoryAsync(AssociatePersonToMemoryDto dto);
        
        // Listados de elementos asociados
        Task<IEnumerable<PlaceDto>> GetMemoryPlacesAsync(int memoryId);
        Task<IEnumerable<ObjectDto>> GetMemoryObjectsAsync(int memoryId);
        Task<IEnumerable<NoteDto>> GetMemoryNotesAsync(int memoryId);
        Task<IEnumerable<PersonDto>> GetMemoryPeopleAsync(int memoryId);
    }
}