using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryKeeper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ObjectsController : ControllerBase
    {
        private readonly IObjectService _objectService;

        public ObjectsController(IObjectService objectService)
        {
            _objectService = objectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllObjects()
        {
            try
            {
                var objects = await _objectService.GetAllObjectsAsync();
                return Ok(objects);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetObjectById(int id)
        {
            try
            {
                var obj = await _objectService.GetObjectByIdAsync(id);
                return Ok(obj);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Object with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateObject([FromBody] CreateObjectDto createObjectDto)
        {
            try
            {
                var obj = await _objectService.CreateObjectAsync(createObjectDto);
                return CreatedAtAction(nameof(GetObjectById), new { id = obj.Id }, obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("associate")]
        public async Task<IActionResult> AssociateObjectToMemory([FromBody] AssociateObjectToMemoryDto dto)
        {
            try
            {
                await _objectService.AssociateObjectToMemoryAsync(dto);
                return Ok("Object associated to memory successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}