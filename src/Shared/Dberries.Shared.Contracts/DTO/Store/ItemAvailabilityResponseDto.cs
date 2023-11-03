namespace Dberries;

public class ItemAvailabilityResponseDto
{
    public IEnumerable<ItemAvailabilityInLocationDto> AvailableInLocations { get; set; }
}