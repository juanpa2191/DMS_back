using MemoryKeeper.Domain.Enums;
using System;
using System.Collections.Generic;

namespace MemoryKeeper.Application.DTOs
{
    public class MemoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime OccurredAt { get; set; }
        public MemoryStatus Status { get; set; }
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public int? ConfirmedByUserId { get; set; }
        public string? ConfirmedByUserName { get; set; }
        public DateTime? ConfirmedAt { get; set; }
        public bool IsConfirmed => Status == MemoryStatus.Confirmado;
        
        public List<PlaceDto> Places { get; set; } = new();
        public List<ObjectDto> Objects { get; set; } = new();
        public List<NoteDto> Notes { get; set; } = new();
        public List<PersonDto> People { get; set; } = new();
    }

    public class CreateMemoryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class UpdateMemoryDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime OccurredAt { get; set; }
    }

    public class ConfirmMemoryDto
    {
        public int MemoryId { get; set; }
        public int ConfirmedByUserId { get; set; }
    }

    public class MemorySearchDto
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int? UserId { get; set; }
        public MemoryStatus? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}