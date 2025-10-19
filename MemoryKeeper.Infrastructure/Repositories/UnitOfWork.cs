using Microsoft.EntityFrameworkCore.Storage;
using MemoryKeeper.Domain.Interfaces;
using MemoryKeeper.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace MemoryKeeper.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MemoryKeeperDbContext _context;
        private IDbContextTransaction? _transaction;

        private IUserRepository? _users;
        private IMemoryRepository? _memories;
        private IGenericRepository<Domain.Entities.Place>? _places;
        private IGenericRepository<Domain.Entities.Object>? _objects;
        private IGenericRepository<Domain.Entities.Note>? _notes;
        private IGenericRepository<Domain.Entities.Person>? _people;
        private IGenericRepository<Domain.Entities.MemoryPlace>? _memoryPlaces;
        private IGenericRepository<Domain.Entities.MemoryObject>? _memoryObjects;
        private IGenericRepository<Domain.Entities.MemoryNote>? _memoryNotes;
        private IGenericRepository<Domain.Entities.MemoryPerson>? _memoryPeople;

        public UnitOfWork(MemoryKeeperDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _users ??= new UserRepository(_context);

        public IMemoryRepository Memories => _memories ??= new MemoryRepository(_context);

        public IGenericRepository<Domain.Entities.Place> Places => 
            _places ??= new GenericRepository<Domain.Entities.Place>(_context);

        public IGenericRepository<Domain.Entities.Object> Objects => 
            _objects ??= new GenericRepository<Domain.Entities.Object>(_context);

        public IGenericRepository<Domain.Entities.Note> Notes => 
            _notes ??= new GenericRepository<Domain.Entities.Note>(_context);

        public IGenericRepository<Domain.Entities.Person> People => 
            _people ??= new GenericRepository<Domain.Entities.Person>(_context);

        public IGenericRepository<Domain.Entities.MemoryPlace> MemoryPlaces => 
            _memoryPlaces ??= new GenericRepository<Domain.Entities.MemoryPlace>(_context);

        public IGenericRepository<Domain.Entities.MemoryObject> MemoryObjects => 
            _memoryObjects ??= new GenericRepository<Domain.Entities.MemoryObject>(_context);

        public IGenericRepository<Domain.Entities.MemoryNote> MemoryNotes => 
            _memoryNotes ??= new GenericRepository<Domain.Entities.MemoryNote>(_context);

        public IGenericRepository<Domain.Entities.MemoryPerson> MemoryPeople => 
            _memoryPeople ??= new GenericRepository<Domain.Entities.MemoryPerson>(_context);

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}