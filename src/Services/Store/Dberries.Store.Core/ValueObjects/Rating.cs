namespace Dberries.Store;

public class Rating
{
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public byte? Value { get; set; }

    public static byte MinValue = 0;
    public static byte MaxValue = 5;

    public Rating(Guid? userId, int? value)
    {
        UserId = userId;
        Value = value is not null ? (byte)value : null;
    }

    public Rating()
    {
    }
}