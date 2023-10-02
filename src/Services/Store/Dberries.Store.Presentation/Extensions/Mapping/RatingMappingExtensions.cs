using Dberries.Store.Core;

namespace Dberries;

public static class RatingMappingExtensions
{
    public static RatingDto ToDto(this Rating model)
    {
        return new RatingDto
        {
            UserId = model.UserId,
            ItemId = model.ItemId,
            Value = model.Value,
        };
    }

    public static Rating ToModel(this RatingDto dto)
    {
        return new Rating
        {
            UserId = dto.UserId,
            ItemId = dto.ItemId,
            Value = dto.Value
        };
    }
}