namespace MascotBusiness.Api.Features.Mascots.DTOs;

public class MascotDetailsResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal RentalPrice { get; set; }
    public decimal? SalePrice { get; set; }
    public bool IsAvailableForRent { get; set; }
    public bool IsAvailableForSale { get; set; }
}
