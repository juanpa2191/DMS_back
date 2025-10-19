using MemoryKeeper.Application.DTOs;
using MemoryKeeper.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MemoryKeeper.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NotesController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotes()
        {
            try
            {
                var notes = await _noteService.GetAllNotesAsync();
                return Ok(notes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNoteById(int id)
        {
            try
            {
                var note = await _noteService.GetNoteByIdAsync(id);
                return Ok(note);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"Note with id {id} not found");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateNote([FromBody] CreateNoteDto createNoteDto)
        {
            try
            {
                var note = await _noteService.CreateNoteAsync(createNoteDto);
                return CreatedAtAction(nameof(GetNoteById), new { id = note.Id }, note);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("associate")]
        public async Task<IActionResult> AssociateNoteToMemory([FromBody] AssociateNoteToMemoryDto dto)
        {
            try
            {
                await _noteService.AssociateNoteToMemoryAsync(dto);
                return Ok("Note associated to memory successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}