using MemoryKeeper.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemoryKeeper.Application.Interfaces
{
    public interface IPersonService
    {
        Task<IEnumerable<PersonDto>> GetAllPeopleAsync();
        Task<PersonDto> GetPersonByIdAsync(int id);
        Task<PersonDto> CreatePersonAsync(CreatePersonDto createPersonDto);
        Task AssociatePersonToMemoryAsync(AssociatePersonToMemoryDto dto);
    }
}