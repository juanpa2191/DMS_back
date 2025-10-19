using MemoryKeeper.Domain.Common;
using System;

namespace MemoryKeeper.Domain.Entities
{
    public class MemoryPerson : BaseEntity
    {
        public int MemoryId { get; set; }
        public int PersonId { get; set; }
        public int AssociatedByUserId { get; set; }
        public DateTime AssociatedAt { get; set; }

        // Navigation properties
        public virtual Memory Memory { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
        public virtual User AssociatedByUser { get; set; } = null!;
    }
}