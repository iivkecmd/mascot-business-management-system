using MascotBusiness.Api.Data;
using MascotBusiness.Api.Features.Customers;
using Microsoft.AspNetCore.Mvc;

namespace MascotBusiness.Api.Features.Reservations;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ReservationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateReservation(
        CreateReservationRequest request)
    {
        var mascot = await _context.Mascots.FindAsync(request.MascotId);

        if (mascot is null)
        {
            return NotFound(new
            {
                message = "Maskota nije pronađena."
            });
        }

        if (!mascot.IsAvailableForRent)
        {
            return BadRequest(new
            {
                message = "Maskota nije dostupna za iznajmljivanje."
            });
        }

        if (request.EndAt <= request.StartAt)
        {
            return BadRequest(new
            {
                message = "Kraj termina mora biti posle početka termina."
            });
        }

        if (request.StartAt <= DateTime.UtcNow)
        {
            return BadRequest(new
            {
                message = "Početak termina mora biti u budućnosti."
            });
        }

        var customer = new Customer
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Phone = request.Phone.Trim(),
            Email = request.Email.Trim()
        };

        var publicNumber =
            $"RES-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpperInvariant()}";

        var reservation = new Reservation
        {
            PublicNumber = publicNumber,
            MascotId = mascot.Id,
            Customer = customer,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            EventLocation = request.EventLocation.Trim(),
            Note = request.Note?.Trim(),
            Status = ReservationStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _context.Reservations.Add(reservation);

        await _context.SaveChangesAsync();

        var response = new CreateReservationResponse
        {
            PublicNumber = reservation.PublicNumber,
            Status = reservation.Status
        };

        return StatusCode(StatusCodes.Status201Created, response);
    }
}
