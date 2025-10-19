using System;

namespace MemoryKeeper.Application.DTOs
{
    public class NoteDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; }
        public string CreatedByUserName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class CreateNoteDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int CreatedByUserId { get; set; }
    }

    public class AssociateNoteToMemoryDto
    {
        public int MemoryId { get; set; }
        public int NoteId { get; set; }
        public int AssociatedByUserId { get; set; }
    }
}