using BitzArt.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Store.WebAPI;

[Route("[Controller]")]
[TokenAuthorize]
public class ItemsController : ControllerBase
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
        var item = await _itemsService.GetAsync(id);
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

    [HttpPost("{itemId:guid}/rate", Name = "UpdateRating")]
    public async Task<IActionResult> UpdateRatingAsync([FromRoute] Guid itemId, [FromBody] byte value)
    {
        var userId = HttpContext.GetUserId();
        var updatedItem = await _itemsService.UpdateRatingAsync(itemId, userId, value);
        var result = updatedItem.ToDto();

        return Ok(result);
    }
}