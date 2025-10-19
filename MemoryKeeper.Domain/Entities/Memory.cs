using MemoryKeeper.Domain.Common;
using MemoryKeeper.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MemoryKeeper.Domain.Entities
{
    public class Memory : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public MemoryStatus Status { get; set; } = MemoryStatus.Sospecha;
        public int CreatedByUserId { get; set; }
        public int? ConfirmedByUserId { get; set; }
        public DateTime? ConfirmedAt { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual User? ConfirmedByUser { get; set; }
        public virtual ICollection<MemoryPlace> MemoryPlaces { get; set; } = new List<MemoryPlace>();
        public virtual ICollection<MemoryObject> MemoryObjects { get; set; } = new List<MemoryObject>();
        public virtual ICollection<MemoryNote> MemoryNotes { get; set; } = new List<MemoryNote>();
        public virtual ICollection<MemoryPerson> MemoryPeople { get; set; } = new List<MemoryPerson>();

        public bool IsConfirmed => Status == MemoryStatus.Confirmado;
        
        public void ConfirmMemory(int confirmedByUserId)
        {
            Status = MemoryStatus.Confirmado;
            ConfirmedByUserId = confirmedByUserId;
            ConfirmedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}