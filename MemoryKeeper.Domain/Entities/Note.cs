using MemoryKeeper.Domain.Common;
using System.Collections.Generic;

namespace MemoryKeeper.Domain.Entities
{
    public class Note : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual ICollection<MemoryNote> MemoryNotes { get; set; } = new List<MemoryNote>();
    }
}