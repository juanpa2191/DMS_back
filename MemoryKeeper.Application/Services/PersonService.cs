using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Entities;
using MemoryKeeper.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Services
{
    public class PersonService : IPersonService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PersonDto> CreatePersonAsync(CreatePersonDto createPersonDto)
        {
            var person = new Person
            {
                FirstName = createPersonDto.FirstName,
                LastName = createPersonDto.LastName,
                Description = createPersonDto.Description,
                CreatedByUserId = createPersonDto.CreatedByUserId,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            var createdPerson = await _unitOfWork.People.AddAsync(person);
            await _unitOfWork.SaveChangesAsync();

            return await GetPersonByIdAsync(createdPerson.Id);
        }

        public async Task<PersonDto> GetPersonByIdAsync(int id)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with id {id} not found");
            }

            var user = await _unitOfWork.Users.GetByIdAsync(person.CreatedByUserId);

            return new PersonDto
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Description = person.Description,
                CreatedByUserId = person.CreatedByUserId,
                CreatedByUserName = user?.FullName ?? "Unknown",
                CreatedAt = person.CreatedAt
            };
        }

        public async Task<IEnumerable<PersonDto>> GetAllPeopleAsync()
        {
            var people = await _unitOfWork.People.GetAllAsync();
            var personDtos = new List<PersonDto>();

            foreach (var person in people)
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

            return personDtos;
        }

        public async Task UpdatePersonAsync(int id, CreatePersonDto updatePersonDto)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with id {id} not found");
            }

            person.FirstName = updatePersonDto.FirstName;
            person.LastName = updatePersonDto.LastName;
            person.Description = updatePersonDto.Description;
            person.UpdatedAt = System.DateTime.UtcNow;

            await _unitOfWork.People.UpdateAsync(person);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeletePersonAsync(int id)
        {
            var person = await _unitOfWork.People.GetByIdAsync(id);
            if (person == null)
            {
                throw new KeyNotFoundException($"Person with id {id} not found");
            }

            await _unitOfWork.People.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AssociatePersonToMemoryAsync(AssociatePersonToMemoryDto dto)
        {
            var memoryPerson = new MemoryPerson
            {
                MemoryId = dto.MemoryId,
                PersonId = dto.PersonId,
                AssociatedByUserId = dto.AssociatedByUserId,
                AssociatedAt = System.DateTime.UtcNow,
                CreatedAt = System.DateTime.UtcNow,
                UpdatedAt = System.DateTime.UtcNow
            };

            await _unitOfWork.MemoryPeople.AddAsync(memoryPerson);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}