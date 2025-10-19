using MemoryKeeper.Domain.Common;
using System;

namespace MemoryKeeper.Domain.Entities
{
    public class MemoryNote : BaseEntity
    {
        public int MemoryId { get; set; }
        public int NoteId { get; set; }
        public int AssociatedByUserId { get; set; }
        public DateTime AssociatedAt { get; set; }

        // Navigation properties
        public virtual Memory Memory { get; set; } = null!;
        public virtual Note Note { get; set; } = null!;
        public virtual User AssociatedByUser { get; set; } = null!;
    }
}