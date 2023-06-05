using CarAPI.DAL.EFCore;
using CarAPI.Entities;
using CarAPI.Entities.Dtos.Brand;
using CarAPI.Entities.Dtos.Color;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BrandsController(AppDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [Route("GetBrands")]
        public async Task<IActionResult> GetBrands()
        {
            var result = await _context.Brands.ToListAsync();
            if (result == null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }


        [HttpGet]
        [Route("GetBrand/{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            var result = await _context.Brands.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (result is null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDto brandDto)
        {

            Brand brand = new Brand
            {
                Name = brandDto.Name,

            };
            await _context.Brands.AddAsync(brand);
            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateBrandDto brandDto)
        {
            var result = await _context.Brands.FindAsync(id);
            if (result is null) return NotFound();
            result.Name = brandDto.Name;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Brands.FindAsync(id);
            if (result is null) return BadRequest(new
            {
                StatusCode = 201,
                Message = "bele bir sey yoxdur"
            });
            _context.Brands.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
        }

    }
}
