namespace Dberries;

public class RefreshTokenData
{
    public Guid? UserId { get; set; }

    public RefreshTokenData(Guid userId)
    {
        UserId = userId;
    }
}