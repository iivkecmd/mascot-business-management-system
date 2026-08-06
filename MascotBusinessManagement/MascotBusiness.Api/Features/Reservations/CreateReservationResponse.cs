namespace MascotBusiness.Api.Features.Reservations;

public class CreateReservationResponse
{
    public string PublicNumber { get; set; } = string.Empty;

    public ReservationStatus Status { get; set; }
}
