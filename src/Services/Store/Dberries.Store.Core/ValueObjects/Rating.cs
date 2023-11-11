namespace Dberries.Store;

public class Rating
{
    public Guid? UserId { get; set; }
    public User? User { get; set; }
    public byte? Value { get; set; }

    public static byte MinValue = 0;
    public static byte MaxValue = 5;

    public Rating(Guid? userId, byte? value)
    {
        UserId = userId;
        Value = value;
    }

    public Rating()
    {
    }
}