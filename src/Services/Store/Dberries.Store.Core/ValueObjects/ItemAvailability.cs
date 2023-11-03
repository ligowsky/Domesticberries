namespace Dberries.Store;

public class ItemAvailability
{
    public IEnumerable<ItemAvailabilityDetails> Details { get; set; }

    public ItemAvailability(IEnumerable<ItemAvailabilityDetails> details)
    {
        Details = details;
    }
}