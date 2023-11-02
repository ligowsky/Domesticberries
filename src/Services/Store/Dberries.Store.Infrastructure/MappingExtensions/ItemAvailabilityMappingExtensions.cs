namespace Dberries.Store;

public static class ItemAvailabilityMappingExtensions
{
    public static ItemAvailabilityDetailsDto ToDto(this ItemAvailabilityDetails model)
    {
        return new ItemAvailabilityDetailsDto
        {
            LocationId = model.LocationId,
            LocationName = model.LocationName,
            ItemQuantity = model.Quantity
        };
    }
}