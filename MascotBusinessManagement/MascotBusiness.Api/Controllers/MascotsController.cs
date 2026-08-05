

using Microsoft.AspNetCore.Mvc;
using MascotBusiness.Api.Data;
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
    }
}
