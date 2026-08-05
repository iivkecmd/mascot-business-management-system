

using Microsoft.AspNetCore.Mvc;
using MascotBusiness.Api.Data;
using MascotBusiness.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MascotBusiness.Api.Controllers
{
   

    [ApiController]
    [Route("api/[controller]")]
    public class MascotsController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MascotsController(ApplicationDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMascots() {

            var mascots = await _context.Mascots.ToListAsync();
            return Ok(mascots);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMascot(Mascot mascot)
        {
            _context.Mascots.Add(mascot);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, mascot);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMascot(int id) {

            var mascot = await _context.Mascots.FindAsync(id);

            if (mascot == null) { return NotFound(); }


            return Ok(mascot);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMascot(int id, Mascot mascot) {

            if (id != mascot.Id) {

                return BadRequest();

            }

            var existingMascot = await _context.Mascots.FindAsync(id);

            if (existingMascot == null)
            {
                return NotFound();
            }

            existingMascot.Name = mascot.Name;
            existingMascot.Description = mascot.Description;
            existingMascot.ImageUrl = mascot.ImageUrl;
            existingMascot.RentalPrice = mascot.RentalPrice;
            existingMascot.SalePrice = mascot.SalePrice;
            existingMascot.IsAvailableForRent = mascot.IsAvailableForRent;
            existingMascot.IsAvailableForSale = mascot.IsAvailableForSale;
            existingMascot.StockQuantity = mascot.StockQuantity;


            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
