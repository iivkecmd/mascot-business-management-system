using System.ComponentModel.DataAnnotations;

namespace MascotBusiness.Api.Features.Mascots;


//DTO
public class CreateMascotRequest
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [MaxLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal RentalPrice { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? SalePrice { get; set; }

    public bool IsAvailableForRent { get; set; }

    public bool IsAvailableForSale { get; set; }

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }
}
