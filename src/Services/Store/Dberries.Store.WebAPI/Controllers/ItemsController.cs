using BitzArt.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Store.WebAPI;

[Route("[Controller]")]
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

    [HttpGet("{id:guid}/availability", Name = "GetAvailability")]
    public async Task<IActionResult> GetAvailabilityAsync([FromQuery] PageRequest pageRequest, [FromRoute] Guid id)
    {
        var locations = await _itemsService.GetAvailabilityAsync(pageRequest, id);
        var result = locations.Convert(x => x.ToDto());

        return Ok(result);
    }
}