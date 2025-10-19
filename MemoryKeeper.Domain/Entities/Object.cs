using MemoryKeeper.Domain.Common;
using System.Collections.Generic;

namespace MemoryKeeper.Domain.Entities
{
    public class Object : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual ICollection<MemoryObject> MemoryObjects { get; set; } = new List<MemoryObject>();
    }
}