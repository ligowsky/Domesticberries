namespace Dberries.Store;

public class Rating
{
    public Guid? UserId { get; set; }
    public Guid? ItemId { get; set; }
    public byte? Value { get; set; }
}