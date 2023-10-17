namespace Dberries.Warehouse;

public record LocationAddedMessage
{
    public LocationDto Location { get; set; }

    public LocationAddedMessage(LocationDto location)
    {
        Location = location;
    }

    public LocationAddedMessage()
    {
    }
};