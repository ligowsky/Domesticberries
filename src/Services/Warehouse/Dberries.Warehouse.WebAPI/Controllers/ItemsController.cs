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

    [HttpGet(Name = "GetItems")]
    public async Task<IActionResult> GetItemsAsync()
    {
        var items = await _itemsService.GetItemsAsync();

        return Ok(items);
    }

    [HttpGet("{id:guid}", Name = "GetItem")]
    public async Task<IActionResult> GetItemAsync([FromRoute] Guid id)
    {
        var item = await _itemsService.GetItemAsync(id);
        
        return Ok(item);
    }

    [HttpPost(Name = "CreateItem")]
    public async Task<IActionResult> CreateItemAsync([FromBody] ItemDto input)
    {
        var item = input.ToModel();
        var createdItem = await _itemsService.CreateItemAsync(item);
        
        return Ok(createdItem);
    }

    [HttpPut("{id:guid}", Name = "UpdateItem")]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id, [FromBody] ItemDto input)
    {
        var item = input.ToModel();
        var updatedItem = await _itemsService.UpdateItemAsync(id, item);
        
        return Ok(updatedItem);
    }

    [HttpDelete("{id:guid}", Name = "DeleteItem")]
    public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id)
    {
        await _itemsService.DeleteItemAsync(id); 
            
        return Ok();
    }
}