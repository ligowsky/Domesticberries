namespace Dberries.Store;

public static class ItemAvailabilityMappingExtensions
{
    public static ItemAvailabilityDto ToDto(this ItemAvailability model)
    {
        return new ItemAvailabilityDto
        {
            Location = model.Location!.ToDto(),
            Quantity = model.Quantity
        };
    }

    public static ItemAvailability ToModel(this ItemAvailabilityDto dto)
    {
        return new ItemAvailability
        {
            Location = dto.Location!.ToModel(),
            Quantity = dto.Quantity
        };
    }
}