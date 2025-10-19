using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Domain.Interfaces
{
    public interface IMemoryRepository : IGenericRepository<Memory>
    {
        Task<IEnumerable<Memory>> GetMemoriesByUserIdAsync(int userId);
        Task<IEnumerable<Memory>> GetMemoriesByStatusAsync(MemoryStatus status);
        Task<IEnumerable<Memory>> SearchMemoriesAsync(string searchTerm);
        Task<IEnumerable<Memory>> GetMemoriesFromDateAsync(System.DateTime date);
        Task<Memory?> GetMemoryWithDetailsAsync(int memoryId);
        Task<IEnumerable<Memory>> GetMemoriesWithPlacesAsync(int userId);
        Task<IEnumerable<Memory>> GetMemoriesWithObjectsAsync(int userId);
        Task<IEnumerable<Memory>> GetMemoriesWithNotesAsync(int userId);
        Task<IEnumerable<Memory>> GetMemoriesWithPeopleAsync(int userId);
    }
}