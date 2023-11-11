namespace Dberries.Store;

public static class RatingMappingExtensions
{
    public static Rating ToModel(this RatingDto dto, Guid? userId)
    {
        return new Rating(userId, dto.Value);
    }
}