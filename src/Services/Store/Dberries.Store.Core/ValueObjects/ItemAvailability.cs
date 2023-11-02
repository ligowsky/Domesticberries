namespace Dberries.Store;

public class ItemAvailability
{
    public IEnumerable<ItemAvailabilityDetails> ItemAvailabilityDetailsList { get; set; }

    public ItemAvailability(IEnumerable<ItemAvailabilityDetails> itemAvailabilityDetailsList )
    {
        ItemAvailabilityDetailsList = itemAvailabilityDetailsList;
    }
}