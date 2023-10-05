using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.WebAPI;

[ApiController]
[Route("[Controller]")]
public class LocationsController : ControllerBase
{
    [HttpGet(Name = "GetLocations")]
    public async Task<IActionResult> GetLocationsAsync()
    {
        return Ok();
    }

    [HttpGet("{id:guid}", Name = "GetLocation")]
    public async Task<IActionResult> GetLocationByIdAsync([FromRoute] Guid id)
    {
        return Ok();
    }

    [HttpPost(Name = "CreateLocation")]
    public async Task<IActionResult> CreateLocationAsync([FromBody] LocationDto input)
    {
        var location = input.ToModel();
        return Ok();
    }

    [HttpPut("{id:guid}", Name = "UpdateLocation")]
    public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid id, [FromBody] LocationDto input)
    {
        var location = input.ToModel();
        return Ok();
    }

    [HttpDelete("{id:guid}", Name = "DeleteLocation")]
    public async Task<IActionResult> DeleteLocationByIdAsync([FromRoute] Guid id)
    {
        return Ok();
    }

    [HttpGet("{id:guid}/stock", Name = "GetLocationStock")]
    public async Task<IActionResult> GetLocationStockAsync([FromRoute] Guid id)
    {
        return Ok();
    }

    [HttpGet("{locationId:guid}/stock/{itemId:guid}", Name = "GetLocationStockByItemId")]
    public async Task<IActionResult> GetLocationStockByItemIdAsync([FromRoute] Guid locationId, [FromRoute] Guid itemId)
    {
        return Ok();
    }

    [HttpPut("{locationId:guid}/stock/{itemId:guid}", Name = "UpdateLocationStockByItemId")]
    public async Task<IActionResult> UpdateLocationStockByItemIdAsync([FromRoute] Guid locationId,
        [FromRoute] Guid itemId, [FromBody] StockDto input)
    {
        var stock = input.ToModel();
        return Ok();
    }
}