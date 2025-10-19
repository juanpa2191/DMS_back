using MemoryKeeper.Domain.Common;
using System.Collections.Generic;

namespace MemoryKeeper.Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Memory> CreatedMemories { get; set; } = new List<Memory>();
        public virtual ICollection<Memory> ConfirmedMemories { get; set; } = new List<Memory>();
        public virtual ICollection<Place> CreatedPlaces { get; set; } = new List<Place>();
        public virtual ICollection<Object> CreatedObjects { get; set; } = new List<Object>();
        public virtual ICollection<Note> CreatedNotes { get; set; } = new List<Note>();
        public virtual ICollection<Person> CreatedPeople { get; set; } = new List<Person>();
        public virtual ICollection<MemoryPlace> AssociatedMemoryPlaces { get; set; } = new List<MemoryPlace>();
        public virtual ICollection<MemoryObject> AssociatedMemoryObjects { get; set; } = new List<MemoryObject>();
        public virtual ICollection<MemoryNote> AssociatedMemoryNotes { get; set; } = new List<MemoryNote>();
        public virtual ICollection<MemoryPerson> AssociatedMemoryPeople { get; set; } = new List<MemoryPerson>();

        public string FullName => $"{FirstName} {LastName}";
    }
}