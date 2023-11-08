namespace Dberries;

public class AccessTokenData
{
    public Guid? UserId { get; set; }

    public AccessTokenData(Guid userId)
    {
        UserId = userId;
    }
}