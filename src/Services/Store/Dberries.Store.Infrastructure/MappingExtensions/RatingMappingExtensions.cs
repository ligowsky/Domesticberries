namespace Dberries.Store;

public static class RatingMappingExtensions
{
    public static Rating ToModel(this UpdateRatingRequestDto dto, Guid? userId)
    {
        return new Rating(userId, dto.Value);
    }
}