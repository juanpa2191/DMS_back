using MemoryKeeper.Domain.Common;
using System.Collections.Generic;

namespace MemoryKeeper.Domain.Entities
{
    public class Person : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual ICollection<MemoryPerson> MemoryPeople { get; set; } = new List<MemoryPerson>();

        public string FullName => $"{FirstName} {LastName}";
    }
}