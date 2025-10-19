using Microsoft.EntityFrameworkCore;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Enums;
using MemoryKeeper.Domain.Interfaces;
using MemoryKeeper.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryKeeper.Infrastructure.Repositories
{
    public class MemoryRepository : GenericRepository<Memory>, IMemoryRepository
    {
        public MemoryRepository(MemoryKeeperDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Memory>> GetMemoriesByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(m => m.CreatedByUserId == userId)
                .Include(m => m.CreatedByUser)
                .Include(m => m.ConfirmedByUser)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesByStatusAsync(MemoryStatus status)
        {
            return await _dbSet
                .Where(m => m.Status == status)
                .Include(m => m.CreatedByUser)
                .Include(m => m.ConfirmedByUser)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Memory>> SearchMemoriesAsync(string searchTerm)
        {
            return await _dbSet
                .Where(m => m.Title.Contains(searchTerm) || m.Description.Contains(searchTerm))
                .Include(m => m.CreatedByUser)
                .Include(m => m.ConfirmedByUser)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesFromDateAsync(DateTime date)
        {
            var startDate = date.Date;
            var endDate = startDate.AddDays(1);

            return await _dbSet
                .Where(m => m.OccurredAt >= startDate && m.OccurredAt < endDate)
                .Include(m => m.CreatedByUser)
                .Include(m => m.ConfirmedByUser)
                .OrderByDescending(m => m.OccurredAt)
                .ToListAsync();
        }

        public async Task<Memory?> GetMemoryWithDetailsAsync(int memoryId)
        {
            return await _dbSet
                .Where(m => m.Id == memoryId)
                .Include(m => m.CreatedByUser)
                .Include(m => m.ConfirmedByUser)
                .Include(m => m.MemoryPlaces)
                    .ThenInclude(mp => mp.Place)
                .Include(m => m.MemoryPlaces)
                    .ThenInclude(mp => mp.AssociatedByUser)
                .Include(m => m.MemoryObjects)
                    .ThenInclude(mo => mo.Object)
                .Include(m => m.MemoryObjects)
                    .ThenInclude(mo => mo.AssociatedByUser)
                .Include(m => m.MemoryNotes)
                    .ThenInclude(mn => mn.Note)
                .Include(m => m.MemoryNotes)
                    .ThenInclude(mn => mn.AssociatedByUser)
                .Include(m => m.MemoryPeople)
                    .ThenInclude(mp => mp.Person)
                .Include(m => m.MemoryPeople)
                    .ThenInclude(mp => mp.AssociatedByUser)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesWithPlacesAsync(int userId)
        {
            return await _dbSet
                .Where(m => m.CreatedByUserId == userId && m.MemoryPlaces.Any())
                .Include(m => m.CreatedByUser)
                .Include(m => m.MemoryPlaces)
                    .ThenInclude(mp => mp.Place)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesWithObjectsAsync(int userId)
        {
            return await _dbSet
                .Where(m => m.CreatedByUserId == userId && m.MemoryObjects.Any())
                .Include(m => m.CreatedByUser)
                .Include(m => m.MemoryObjects)
                    .ThenInclude(mo => mo.Object)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesWithNotesAsync(int userId)
        {
            return await _dbSet
                .Where(m => m.CreatedByUserId == userId && m.MemoryNotes.Any())
                .Include(m => m.CreatedByUser)
                .Include(m => m.MemoryNotes)
                    .ThenInclude(mn => mn.Note)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Memory>> GetMemoriesWithPeopleAsync(int userId)
        {
            return await _dbSet
                .Where(m => m.CreatedByUserId == userId && m.MemoryPeople.Any())
                .Include(m => m.CreatedByUser)
                .Include(m => m.MemoryPeople)
                    .ThenInclude(mp => mp.Person)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();
        }
    }
}