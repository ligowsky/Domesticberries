namespace Dberries.Warehouse;

public record LocationUpdatedMessage
{
    public LocationDto Location { get; set; }

    public LocationUpdatedMessage(LocationDto location)
    {
        Location = location;
    }

    public LocationUpdatedMessage()
    {
    }
};