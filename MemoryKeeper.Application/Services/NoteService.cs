using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Services
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoteService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto)
        {
            var note = new Note
            {
                Title = createNoteDto.Title,
                Content = createNoteDto.Content,
                CreatedByUserId = createNoteDto.CreatedByUserId,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            var createdNote = await _unitOfWork.Notes.AddAsync(note);
            await _unitOfWork.SaveChangesAsync();

            return await GetNoteByIdAsync(createdNote.Id);
        }

        public async Task<NoteDto> GetNoteByIdAsync(int id)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(id);
            if (note == null)
            {
                throw new KeyNotFoundException($"Note with id {id} not found");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(note.CreatedByUserId);

            return new NoteDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedByUserId = note.CreatedByUserId,
                CreatedByUserName = user?.FullName ?? "Unknown",
                CreatedAt = note.CreatedAt
            };
        }

        public async Task<IEnumerable<NoteDto>> GetAllNotesAsync()
        {
            var notes = await _unitOfWork.Notes.GetAllAsync();
            var noteDtos = new List<NoteDto>();

            foreach (var note in notes)
            {
                var user = await _unitOfWork.Users.GetByIdAsync(note.CreatedByUserId);
                noteDtos.Add(new NoteDto
                {
                    Id = note.Id,
                    Title = note.Title,
                    Content = note.Content,
                    CreatedByUserId = note.CreatedByUserId,
                    CreatedByUserName = user?.FullName ?? "Unknown",
                    CreatedAt = note.CreatedAt
                });
            }

            return noteDtos;
        }

        public async Task UpdateNoteAsync(int id, CreateNoteDto updateNoteDto)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(id);
            if (note == null)
            {
                throw new KeyNotFoundException($"Note with id {id} not found");
            }

            note.Title = updateNoteDto.Title;
            note.Content = updateNoteDto.Content;
            note.UpdatedAt = System.DateTime.UtcNow;

            await _unitOfWork.Notes.UpdateAsync(note);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(int id)
        {
            var note = await _unitOfWork.Notes.GetByIdAsync(id);
            if (note == null)
            {
                throw new KeyNotFoundException($"Note with id {id} not found");
            }

            await _unitOfWork.Notes.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociateNoteToMemoryAsync(AssociateNoteToMemoryDto dto)
        {
            var memoryNote = new MemoryNote
            {
                MemoryId = dto.MemoryId,
                NoteId = dto.NoteId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = System.DateTime.UtcNow,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            await _unitOfWork.MemoryNotes.AddAsync(memoryNote);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}