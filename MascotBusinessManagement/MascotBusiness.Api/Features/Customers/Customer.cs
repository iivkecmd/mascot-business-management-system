using System.ComponentModel.DataAnnotations;

namespace MascotBusiness.Api.Features.Customers
{
    public class Customer
    {
        public int Id { get; set; }

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

        [Required][MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

    }
}
