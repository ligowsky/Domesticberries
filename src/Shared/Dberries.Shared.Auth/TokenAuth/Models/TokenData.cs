namespace Dberries;

public class TokenData
{
    public Guid? UserId { get; set; }

    public TokenData(Guid userId)
    {
        UserId = userId;
    }
}