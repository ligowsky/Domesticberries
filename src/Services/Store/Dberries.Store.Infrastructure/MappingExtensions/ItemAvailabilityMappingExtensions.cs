namespace Dberries.Store;

public static class ItemAvailabilityMappingExtensions
{
    public static ItemAvailabilityInLocationDto ToDto(this ItemAvailabilityInLocation model)
    {
        return new ItemAvailabilityInLocationDto
        {
            LocationId = model.LocationId,
            LocationName = model.LocationName,
            ItemQuantity = model.Quantity
        };
    }
    
    public static ItemAvailabilityResponseDto ToDto(this ItemAvailabilityResponse model)
    {
        return new ItemAvailabilityResponseDto
        {
            AvailableInLocations = model.AvailableInLocations.Select(x => x.ToDto())
        };
    }
}