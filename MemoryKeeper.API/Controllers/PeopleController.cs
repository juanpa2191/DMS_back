using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryKeeper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IPersonService _personService;

        public PeopleController(IPersonService personService)
        {
            _personService = personService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPeople()
        {
            try
            {
                var people = await _personService.GetAllPeopleAsync();
                return Ok(people);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonById(int id)
        {
            try
            {
                var person = await _personService.GetPersonByIdAsync(id);
                return Ok(person);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Person with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePerson([FromBody] CreatePersonDto createPersonDto)
        {
            try
            {
                var person = await _personService.CreatePersonAsync(createPersonDto);
                return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("associate")]
        public async Task<IActionResult> AssociatePersonToMemory([FromBody] AssociatePersonToMemoryDto dto)
        {
            try
            {
                await _personService.AssociatePersonToMemoryAsync(dto);
                return Ok("Person associated to memory successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}