using CarAPI.DAL.EFCore;
using CarAPI.Entities;
using CarAPI.Entities.Dtos.Car;
using CarAPI.Entities.Dtos.Color;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace CarAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("GetCar/{id}")]
        public async Task<IActionResult> GetCar(int id)
        {
            var result = await _context.Cars.Where(b => b.Id == id).FirstOrDefaultAsync();
            if (result is null)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpGet]
        [Route("GetCars")]
        public async Task<IActionResult> GetCars()
        {
            var result = await _context.Cars.ToListAsync();
            if (result.Count == 0)
            {
                return NotFound();
            }
            return StatusCode((int)HttpStatusCode.OK, result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _context.Cars.FindAsync(id);
            if (result is null) return BadRequest(new
            {
                StatusCode = 201,
                Message = "bele bir sey yoxdur"
            });
            _context.Cars.Remove(result);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateCarDto cardto)
        {

            Car car = new Car
            {
               BrandId = cardto.BrandId,
               ColorId = cardto.ColorId,
               Description= cardto.Description,
               DailyPrice = cardto.DailyPrice,
               ModelYear = cardto.ModelYear

            };
            await _context.Cars.AddAsync(car);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateCarDto cardto)
        {
            var result = await _context.Cars.FindAsync(id);
            if (result is null) return NotFound();
            result.Description = cardto.Description;
            result.DailyPrice = cardto.DailyPrice;
            result.ModelYear = cardto.ModelYear;
            result.BrandId = cardto.BrandId;
            result.ColorId = cardto.ColorId;

            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
