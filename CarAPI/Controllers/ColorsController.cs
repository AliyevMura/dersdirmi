using CarAPI.DAL.EFCore;
using CarAPI.Entities;
using CarAPI.Entities.Dtos.Color;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using System.Net;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ColorsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("GetColors")]
        public async Task<IActionResult> GetColors()
        {
            var result = await _context.Colors.ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }


        [HttpGet]
        [Route("GetColor/{id}")]
        public async Task<IActionResult> GetColor(int id)
        {
            var result = await _context.Colors.Where(b=>b.Id==id).FirstOrDefaultAsync();
            if (result is null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateColorDto colordto)
        {

            Color color = new Color
            {
               Name= colordto.Name,

            };
            await _context.Colors.AddAsync(color);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateColorDto colordto)
        {
            var result = await _context.Colors.FindAsync(id);
            if (result is null) return NotFound();
            result.Name = colordto.Name;
            
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Colors.FindAsync(id);
            if (result is null) return BadRequest(new
            {
                StatusCode = 201,
                Message = "bele bir sey yoxdur"
            });
            _context.Colors.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
