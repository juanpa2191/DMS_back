using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Services
{
    public class MemoryService : IMemoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MemoryDto> CreateMemoryAsync(CreateMemoryDto createMemoryDto)
        {
            var memory = new Memory
            {
                Title = createMemoryDto.Title,
                Description = createMemoryDto.Description,
                OccurredAt = createMemoryDto.OccurredAt,
                CreatedByUserId = createMemoryDto.CreatedByUserId,
                Status = Domain.Enums.MemoryStatus.Sospecha,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdMemory = await _unitOfWork.Memories.AddAsync(memory);
            await _unitOfWork.SaveChangesAsync();

            return await GetMemoryByIdAsync(createdMemory.Id);
        }

        public async Task<MemoryDto> UpdateMemoryAsync(UpdateMemoryDto updateMemoryDto)
        {
            var memory = await _unitOfWork.Memories.GetByIdAsync(updateMemoryDto.Id);
            if (memory == null)
            {
                throw new KeyNotFoundException($"Memory with id {updateMemoryDto.Id} not found");
            }

            memory.Title = updateMemoryDto.Title;
            memory.Description = updateMemoryDto.Description;
            memory.OccurredAt = updateMemoryDto.OccurredAt;
            memory.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.Memories.UpdateAsync(memory);
            await _unitOfWork.SaveChangesAsync();

            return await GetMemoryByIdAsync(memory.Id);
        }

        public async Task<MemoryDto> GetMemoryByIdAsync(int id)
        {
            var memory = await _unitOfWork.Memories.GetMemoryWithDetailsAsync(id);
            if (memory == null)
            {
                throw new KeyNotFoundException($"Memory with id {id} not found");
            }

            return await MapToMemoryDtoAsync(memory);
        }

        public async Task<MemoryDto> GetMemoryWithDetailsAsync(int id)
        {
            return await GetMemoryByIdAsync(id);
        }

        public async Task<IEnumerable<MemoryDto>> GetMemoriesByUserIdAsync(int userId)
        {
            var memories = await _unitOfWork.Memories.GetMemoriesByUserIdAsync(userId);
            var memoryDtos = new List<MemoryDto>();

            foreach (var memory in memories)
            {
                memoryDtos.Add(await MapToMemoryDtoAsync(memory));
            }

            return memoryDtos;
        }

        public async Task<IEnumerable<MemoryDto>> SearchMemoriesAsync(MemorySearchDto searchDto)
        {
            IEnumerable<Memory> memories;

            if (!string.IsNullOrEmpty(searchDto.SearchTerm))
            {
                memories = await _unitOfWork.Memories.SearchMemoriesAsync(searchDto.SearchTerm);
            }
            else if (searchDto.Status.HasValue)
            {
                memories = await _unitOfWork.Memories.GetMemoriesByStatusAsync(searchDto.Status.Value);
            }
            else if (searchDto.UserId.HasValue)
            {
                memories = await _unitOfWork.Memories.GetMemoriesByUserIdAsync(searchDto.UserId.Value);
            }
            else
            {
                memories = await _unitOfWork.Memories.GetAllAsync();
            }

            // Apply date filters if provided
            if (searchDto.FromDate.HasValue || searchDto.ToDate.HasValue)
            {
                memories = memories.Where(m =>
                    (!searchDto.FromDate.HasValue || m.OccurredAt >= searchDto.FromDate.Value) &&
                    (!searchDto.ToDate.HasValue || m.OccurredAt <= searchDto.ToDate.Value));
            }

            var memoryDtos = new List<MemoryDto>();
            foreach (var memory in memories)
            {
                memoryDtos.Add(await MapToMemoryDtoAsync(memory));
            }

            return memoryDtos;
        }

        public async Task<MemoryDto> ConfirmMemoryAsync(ConfirmMemoryDto confirmMemoryDto)
        {
            var memory = await _unitOfWork.Memories.GetByIdAsync(confirmMemoryDto.MemoryId);
            if (memory == null)
            {
                throw new KeyNotFoundException($"Memory with id {confirmMemoryDto.MemoryId} not found");
            }

            memory.ConfirmMemory(confirmMemoryDto.ConfirmedByUserId);
            await _unitOfWork.Memories.UpdateAsync(memory);
            await _unitOfWork.SaveChangesAsync();

            return await GetMemoryByIdAsync(memory.Id);
        }

        public async Task DeleteMemoryAsync(int id)
        {
            var memory = await _unitOfWork.Memories.GetByIdAsync(id);
            if (memory == null)
            {
                throw new KeyNotFoundException($"Memory with id {id} not found");
            }

            await _unitOfWork.Memories.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociatePlaceToMemoryAsync(AssociatePlaceToMemoryDto dto)
        {
            var memoryPlace = new MemoryPlace
            {
                MemoryId = dto.MemoryId,
                PlaceId = dto.PlaceId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.MemoryPlaces.AddAsync(memoryPlace);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociateObjectToMemoryAsync(AssociateObjectToMemoryDto dto)
        {
            var memoryObject = new MemoryObject
            {
                MemoryId = dto.MemoryId,
                ObjectId = dto.ObjectId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.MemoryObjects.AddAsync(memoryObject);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociateNoteToMemoryAsync(AssociateNoteToMemoryDto dto)
        {
            var memoryNote = new MemoryNote
            {
                MemoryId = dto.MemoryId,
                NoteId = dto.NoteId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.MemoryNotes.AddAsync(memoryNote);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociatePersonToMemoryAsync(AssociatePersonToMemoryDto dto)
        {
            var memoryPerson = new MemoryPerson
            {
                MemoryId = dto.MemoryId,
                PersonId = dto.PersonId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _unitOfWork.MemoryPeople.AddAsync(memoryPerson);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<PlaceDto>> GetMemoryPlacesAsync(int memoryId)
        {
            var memoryPlaces = await _unitOfWork.MemoryPlaces.FindAsync(mp => mp.MemoryId == memoryId);
            var placeDtos = new List<PlaceDto>();

            foreach (var memoryPlace in memoryPlaces)
            {
                var place = await _unitOfWork.Places.GetByIdAsync(memoryPlace.PlaceId);
                if (place != null)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(place.CreatedByUserId);
                    placeDtos.Add(new PlaceDto
                    {
                        Id = place.Id,
                        Name = place.Name,
                        Description = place.Description,
                        Address = place.Address,
                        Latitude = place.Latitude,
                        Longitude = place.Longitude,
                        CreatedByUserId = place.CreatedByUserId,
                        CreatedByUserName = user?.FullName ?? "Unknown",
                        CreatedAt = place.CreatedAt
                    });
                }
            }

            return placeDtos;
        }

        public async Task<IEnumerable<ObjectDto>> GetMemoryObjectsAsync(int memoryId)
        {
            var memoryObjects = await _unitOfWork.MemoryObjects.FindAsync(mo => mo.MemoryId == memoryId);
            var objectDtos = new List<ObjectDto>();

            foreach (var memoryObject in memoryObjects)
            {
                var obj = await _unitOfWork.Objects.GetByIdAsync(memoryObject.ObjectId);
                if (obj != null)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(obj.CreatedByUserId);
                    objectDtos.Add(new ObjectDto
                    {
                        Id = obj.Id,
                        Name = obj.Name,
                        Description = obj.Description,
                        CreatedByUserId = obj.CreatedByUserId,
                        CreatedByUserName = user?.FullName ?? "Unknown",
                        CreatedAt = obj.CreatedAt
                    });
                }
            }

            return objectDtos;
        }

        public async Task<IEnumerable<NoteDto>> GetMemoryNotesAsync(int memoryId)
        {
            var memoryNotes = await _unitOfWork.MemoryNotes.FindAsync(mn => mn.MemoryId == memoryId);
            var noteDtos = new List<NoteDto>();

            foreach (var memoryNote in memoryNotes)
            {
                var note = await _unitOfWork.Notes.GetByIdAsync(memoryNote.NoteId);
                if (note != null)
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
            }

            return noteDtos;
        }

        public async Task<IEnumerable<PersonDto>> GetMemoryPeopleAsync(int memoryId)
        {
            var memoryPeople = await _unitOfWork.MemoryPeople.FindAsync(mp => mp.MemoryId == memoryId);
            var personDtos = new List<PersonDto>();

            foreach (var memoryPerson in memoryPeople)
            {
                var person = await _unitOfWork.People.GetByIdAsync(memoryPerson.PersonId);
                if (person != null)
                {
                    var user = await _unitOfWork.Users.GetByIdAsync(person.CreatedByUserId);
                    personDtos.Add(new PersonDto
                    {
                        Id = person.Id,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Description = person.Description,
                        CreatedByUserId = person.CreatedByUserId,
                        CreatedByUserName = user?.FullName ?? "Unknown",
                        CreatedAt = person.CreatedAt
                    });
                }
            }

            return personDtos;
        }

        private async Task<MemoryDto> MapToMemoryDtoAsync(Memory memory)
        {
            var createdByUser = await _unitOfWork.Users.GetByIdAsync(memory.CreatedByUserId);
            var confirmedByUser = memory.ConfirmedByUserId.HasValue
                ? await _unitOfWork.Users.GetByIdAsync(memory.ConfirmedByUserId.Value)
                : null;

            return new MemoryDto
            {
                Id = memory.Id,
                Title = memory.Title,
                Description = memory.Description,
                CreatedAt = memory.CreatedAt,
                OccurredAt = memory.OccurredAt,
                Status = memory.Status,
                CreatedByUserId = memory.CreatedByUserId,
                CreatedByUserName = createdByUser?.FullName ?? "Unknown",
                ConfirmedByUserId = memory.ConfirmedByUserId,
                ConfirmedByUserName = confirmedByUser?.FullName,
                ConfirmedAt = memory.ConfirmedAt,
                Places = (await GetMemoryPlacesAsync(memory.Id)).ToList(),
                Objects = (await GetMemoryObjectsAsync(memory.Id)).ToList(),
                Notes = (await GetMemoryNotesAsync(memory.Id)).ToList(),
                People = (await GetMemoryPeopleAsync(memory.Id)).ToList()
            };
        }
    }
}