namespace Dberries.Store;

public class ItemAvailabilityInLocation
{
    public Guid? LocationId { get; set; }
    public string? LocationName { get; set; }
    public int? Quantity { get; set; }
}