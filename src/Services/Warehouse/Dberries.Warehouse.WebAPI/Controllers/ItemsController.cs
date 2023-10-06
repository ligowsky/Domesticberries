using BitzArt.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.WebAPI;

[ApiController]
[Route("[Controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsService _itemsService;

    public ItemsController(IItemsService itemsService)
    {
        _itemsService = itemsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetItemsAsync([FromQuery] PageRequest pageRequest)
    {
        var items = await _itemsService.GetItemsPageAsync(pageRequest);
        var result = items.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetItem")]
    public async Task<IActionResult> GetItemAsync([FromRoute] Guid id)
    {
        var item = await _itemsService.GetItemAsync(id);
        var result = item.ToDto();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateItemAsync([FromBody] ItemDto input)
    {
        var item = input.ToModel();
        var createdItem = await _itemsService.CreateItemAsync(item);
        var result = createdItem.ToDto();

        return CreatedAtRoute("GetItem", new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id, [FromBody] ItemDto input)
    {
        var item = input.ToModel();
        var updatedItem = await _itemsService.UpdateItemAsync(id, item);
        var result = updatedItem.ToDto();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id)
    {
        await _itemsService.DeleteItemAsync(id);

        return Ok();
    }
}