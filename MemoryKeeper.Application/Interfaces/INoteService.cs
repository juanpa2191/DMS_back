using MemoryKeeper.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface INoteService
    {
        Task<IEnumerable<NoteDto>> GetAllNotesAsync();
        Task<NoteDto> GetNoteByIdAsync(int id);
        Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto);
        Task AssociateNoteToMemoryAsync(AssociateNoteToMemoryDto dto);
    }
}