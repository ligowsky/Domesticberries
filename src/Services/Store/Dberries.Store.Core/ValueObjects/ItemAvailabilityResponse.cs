namespace Dberries.Store;

public class ItemAvailabilityResponse
{
    public IEnumerable<ItemAvailabilityInLocation> AvailableInLocations { get; set; }
    
    public ItemAvailabilityResponse(IEnumerable<ItemAvailabilityInLocation> availableInLocations)
    {
        AvailableInLocations = availableInLocations;
    }
}