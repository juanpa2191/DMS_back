using System;
using System.Threading.Tasks;

namespace MemoryKeeper.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IMemoryRepository Memories { get; }
        IGenericRepository<Entities.Place> Places { get; }
        IGenericRepository<Entities.Object> Objects { get; }
        IGenericRepository<Entities.Note> Notes { get; }
        IGenericRepository<Entities.Person> People { get; }
        IGenericRepository<Entities.MemoryPlace> MemoryPlaces { get; }
        IGenericRepository<Entities.MemoryObject> MemoryObjects { get; }
        IGenericRepository<Entities.MemoryNote> MemoryNotes { get; }
        IGenericRepository<Entities.MemoryPerson> MemoryPeople { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}