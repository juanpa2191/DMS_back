using MemoryKeeper.Domain.Common;
using System;

namespace MemoryKeeper.Domain.Entities
{
    public class MemoryObject : BaseEntity
    {
        public int MemoryId { get; set; }
        public int ObjectId { get; set; }
        public int AssociatedByUserId { get; set; }
        public DateTime AssociatedAt { get; set; }

        // Navigation properties
        public virtual Memory Memory { get; set; } = null!;
        public virtual Object Object { get; set; } = null!;
        public virtual User AssociatedByUser { get; set; } = null!;
    }
}