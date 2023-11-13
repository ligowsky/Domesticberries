using BitzArt.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Store.WebAPI;

[Route("[Controller]")]
public class ItemsController : DberriesController
{
    private readonly IItemsService _itemsService;

    public ItemsController(IItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPageAsync([FromQuery] PageRequest pageRequest)
    {
        var items = await _itemsService.GetPageAsync(pageRequest);
        var result = items.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetItem")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var filter = new ItemFilterSet { Id = id };

        var item = await _itemsService.GetAsync(filter);
        var result = item.ToDto();

        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchAsync([FromQuery] PageRequest pageRequest,
        [FromBody] SearchRequestDto searchRequest)
    {
        var items = await _itemsService.SearchAsync(pageRequest, searchRequest);
        var result = items.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{id:guid}/availability", Name = "GetAvailability")]
    public async Task<IActionResult> GetAvailabilityAsync([FromRoute] Guid id)
    {
        var itemAvailability = await _itemsService.GetAvailabilityAsync(id);
        var result = itemAvailability.ToDto();

        return Ok(result);
    }

    [HttpPost("{itemId:guid}/rating", Name = "UpdateRating")]
    [TokenAuthorize]
    public async Task<IActionResult> UpdateRatingAsync([FromRoute] Guid itemId, [FromBody] UpdateRatingRequestDto input)
    {
        Validate(input);

        var itemFilter = new ItemFilterSet { Id = itemId };
        var userFilter = new UserFilterSet { ExternalId = HttpContext.GetUserId() };
        var value = (byte)input.Value!;

        var updatedItem = await _itemsService.UpdateRatingAsync(itemFilter, userFilter, value);
        var result = updatedItem.ToDto();

        return Ok(result);
    }

    [HttpDelete("{itemId:guid}/rating", Name = "DeleteRating")]
    [TokenAuthorize]
    public async Task<IActionResult> RemoveRatingAsync([FromRoute] Guid itemId)
    {
        var itemFilter = new ItemFilterSet { Id = itemId };
        var userFilter = new UserFilterSet { ExternalId = HttpContext.GetUserId() };

        var updatedItem = await _itemsService.RemoveRatingAsync(itemFilter, userFilter);
        var result = updatedItem.ToDto();

        return Ok(result);
    }
}