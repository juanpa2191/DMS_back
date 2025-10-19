using System;

namespace MemoryKeeper.Application.DTOs
{
    public class ObjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateObjectDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class AssociateObjectToMemoryDto
    {
        public int MemoryId { get; set; }
        public int ObjectId { get; set; }
        public int AssociatedByUserId { get; set; }
    }
}