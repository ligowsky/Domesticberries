namespace Dberries.Store;

public static class ItemAvailabilityMappingExtensions
{
    public static ItemAvailabilityDto ToDto(this ItemAvailability model)
    {
        return new ItemAvailabilityDto
        {
            LocationId = model.LocationId,
            LocationName = model.LocationName,
            ItemQuantity = model.Quantity
        };
    }
}