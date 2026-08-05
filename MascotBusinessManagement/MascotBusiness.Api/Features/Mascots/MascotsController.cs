

using Microsoft.AspNetCore.Mvc;
using MascotBusiness.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace MascotBusiness.Api.Features.Mascots
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

        /*
         [HttpPost]
        public async Task<IActionResult> CreateMascot(Mascot mascot)
        {
            _context.Mascots.Add(mascot);
            await _context.SaveChangesAsync();

            return StatusCode(StatusCodes.Status201Created, mascot);
        }
        */


        [HttpPost]
        public async Task<IActionResult> CreateMascot(CreateMascotRequest request) {
            // pretvaranje DTO-a u Mascot
            var mascot = new Mascot
            {

                Name = request.Name,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                RentalPrice = request.RentalPrice,
                SalePrice = request.SalePrice,
                IsAvailableForRent = request.IsAvailableForRent,
                IsAvailableForSale = request.IsAvailableForSale,
                StockQuantity = request.StockQuantity


            };


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
        public async Task<IActionResult> UpdateMascot(
            int id,
            UpdateMascotRequest request)
        {
            var existingMascot = await _context.Mascots.FindAsync(id);

            if (existingMascot == null)
            {
                return NotFound();
            }

            existingMascot.Name = request.Name;
            existingMascot.Description = request.Description;
            existingMascot.ImageUrl = request.ImageUrl;
            existingMascot.RentalPrice = request.RentalPrice;
            existingMascot.SalePrice = request.SalePrice;
            existingMascot.IsAvailableForRent = request.IsAvailableForRent;
            existingMascot.IsAvailableForSale = request.IsAvailableForSale;
            existingMascot.StockQuantity = request.StockQuantity;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMascot(int id)
        {
            var mascot = await _context.Mascots.FindAsync(id);

            if (mascot == null)
            {
                return NotFound();
            }

            _context.Mascots.Remove(mascot);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
