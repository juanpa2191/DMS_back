using MemoryKeeper.Domain.Common;
using System.Collections.Generic;

namespace MemoryKeeper.Domain.Entities
{
    public class Place : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int CreatedByUserId { get; set; }

        // Navigation properties
        public virtual User CreatedByUser { get; set; } = null!;
        public virtual ICollection<MemoryPlace> MemoryPlaces { get; set; } = new List<MemoryPlace>();
    }
}