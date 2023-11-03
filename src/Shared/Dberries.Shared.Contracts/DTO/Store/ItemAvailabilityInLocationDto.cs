namespace Dberries;

public class ItemAvailabilityInLocationDto
{
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public int? ItemQuantity { get; set; }
}