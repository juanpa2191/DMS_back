using System;

namespace MemoryKeeper.Application.DTOs
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePlaceDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class AssociatePlaceToMemoryDto
    {
        public int MemoryId { get; set; }
        public int PlaceId { get; set; }
        public int AssociatedByUserId { get; set; }
    }
}