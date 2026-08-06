using System.ComponentModel.DataAnnotations;

namespace MascotBusiness.Api.Features.Reservations
{
    public class CreateReservationRequest
    {
        [Range(1, int.MaxValue)]
        public int MascotId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        [Required]
        [MaxLength(300)]
        public string EventLocation { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Note { get; set; }
    }
}
