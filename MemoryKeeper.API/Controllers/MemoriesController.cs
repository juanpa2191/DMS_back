using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using MemoryKeeper.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryKeeper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MemoriesController : ControllerBase
    {
        private readonly IMemoryService _memoryService;

        public MemoriesController(IMemoryService memoryService)
        {
            _memoryService = memoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMemories([FromQuery] MemorySearchDto searchDto)
        {
            try
            {
                var memories = await _memoryService.SearchMemoriesAsync(searchDto);
                return Ok(memories);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMemoryById(int id)
        {
            try
            {
                var memory = await _memoryService.GetMemoryByIdAsync(id);
                return Ok(memory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Memory with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMemory([FromBody] CreateMemoryDto createMemoryDto)
        {
            try
            {
                var memory = await _memoryService.CreateMemoryAsync(createMemoryDto);
                return CreatedAtAction(nameof(GetMemoryById), new { id = memory.Id }, memory);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMemory(int id, [FromBody] UpdateMemoryDto updateMemoryDto)
        {
            try
            {
                updateMemoryDto.Id = id;
                var memory = await _memoryService.UpdateMemoryAsync(updateMemoryDto);
                return Ok(memory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Memory with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("confirm")]
        public async Task<IActionResult> ConfirmMemory([FromBody] ConfirmMemoryDto confirmMemoryDto)
        {
            try
            {
                var memory = await _memoryService.ConfirmMemoryAsync(confirmMemoryDto);
                return Ok(memory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Memory with id {confirmMemoryDto.MemoryId} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMemory(int id)
        {
            try
            {
                await _memoryService.DeleteMemoryAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Memory with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}