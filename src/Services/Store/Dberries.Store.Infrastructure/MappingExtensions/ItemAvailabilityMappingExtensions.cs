namespace Dberries.Store;

public static class ItemAvailabilityMappingExtensions
{
    public static ItemAvailabilityDto ToDto(this ItemAvailability model)
    {
        return new ItemAvailabilityDto
        {
            LocationId = model.Location!.Id,
            LocationName = model.Location.Name,
            ItemQuantity = model.Quantity
        };
    }
}