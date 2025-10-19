using System;

namespace MemoryKeeper.Application.DTOs
{
    public class PersonDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }

    public class CreatePersonDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class AssociatePersonToMemoryDto
    {
        public int MemoryId { get; set; }
        public int PersonId { get; set; }
        public int AssociatedByUserId { get; set; }
    }
}