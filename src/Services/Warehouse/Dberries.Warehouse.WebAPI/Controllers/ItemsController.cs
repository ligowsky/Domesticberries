using BitzArt.Pagination;
using Dberries.Warehouse.Presentation;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.WebAPI;

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

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] ItemDto input, [FromServices] ItemValidator validator)
    {
        validator.ValidateDto(input, ActionType.Create);
        var item = input.ToModel();
        var createdItem = await _itemsService.AddAsync(item);
        var result = createdItem.ToDto();

        return CreatedAtRoute("GetItem", new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] ItemDto input,
        [FromServices] ItemValidator validator)
    {
        validator.ValidateDto(input, ActionType.Update);
        var item = input.ToModel();
        var updatedItem = await _itemsService.UpdateAsync(id, item);
        var result = updatedItem.ToDto();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid id)
    {
        await _itemsService.RemoveAsync(id);

        return Ok();
    }
}