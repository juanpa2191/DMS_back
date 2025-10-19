using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryKeeper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlacesController : ControllerBase
    {
        private readonly IPlaceService _placeService;

        public PlacesController(IPlaceService placeService)
        {
            _placeService = placeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPlaces()
        {
            try
            {
                var places = await _placeService.GetAllPlacesAsync();
                return Ok(places);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlaceById(int id)
        {
            try
            {
                var place = await _placeService.GetPlaceByIdAsync(id);
                return Ok(place);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Place with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlace([FromBody] CreatePlaceDto createPlaceDto)
        {
            try
            {
                var place = await _placeService.CreatePlaceAsync(createPlaceDto);
                return CreatedAtAction(nameof(GetPlaceById), new { id = place.Id }, place);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("associate")]
        public async Task<IActionResult> AssociatePlaceToMemory([FromBody] AssociatePlaceToMemoryDto dto)
        {
            try
            {
                await _placeService.AssociatePlaceToMemoryAsync(dto);
                return Ok("Place associated to memory successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}