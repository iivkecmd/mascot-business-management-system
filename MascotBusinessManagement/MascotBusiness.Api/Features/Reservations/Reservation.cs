using MascotBusiness.Api.Features.Customers;
using MascotBusiness.Api.Features.Mascots;
using System.ComponentModel.DataAnnotations;

namespace MascotBusiness.Api.Features.Reservations
{
    public class Reservation
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string PublicNumber {  get; set; } = string.Empty;
        public int MascotId  {  get; set; }

        public Mascot Mascot { get; set; } = null!;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public DateTime StartAt { get; set; }   
        public DateTime EndAt { get; set; }



        // lokacija ?? kasnije srediti
        [Required]
        [MaxLength(300)]
        public string EventLocation { get; set; } = string.Empty;


        // napomena -> korisnik moze da ne unese nista
        [MaxLength(1000)]
        public string? Note {  get; set; }



        //??
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        //??
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
