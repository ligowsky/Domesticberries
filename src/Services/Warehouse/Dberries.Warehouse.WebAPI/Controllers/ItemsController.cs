using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.WebAPI;

[ApiController]
[Route("[Controller]")]
public class ItemsController : ControllerBase
{
    [HttpGet(Name = "GetItems")]
    public async Task<IActionResult> GetItemsAsync()
    {
        return Ok();
    }

    [HttpGet("{id:guid}", Name = "GetItem")]
    public async Task<IActionResult> GetItemByIdAsync([FromRoute] Guid id)
    {
        return Ok();
    }

    [HttpPost(Name = "CreateItem")]
    public async Task<IActionResult> CreateItemAsync([FromBody] ItemDto input)
    {
        var item = input.ToModel();
        return Ok();
    }

    [HttpPut("{id:guid}", Name = "UpdateItem")]
    public async Task<IActionResult> UpdateItemByIdAsync([FromRoute] Guid id, [FromBody] ItemDto input)
    {
        var item = input.ToModel();
        return Ok();
    }

    [HttpDelete("{id:guid}", Name = "DeleteItem")]
    public async Task<IActionResult> DeleteItemByIdAsync([FromRoute] Guid id)
    {
        return Ok();
    }
}