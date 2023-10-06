using BitzArt.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.WebAPI;

[ApiController]
[Route("[Controller]")]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;
    private readonly IStockService _stockService;

    public LocationsController(ILocationsService locationsService, IStockService stockService)
    {
        _locationsService = locationsService;
        _stockService = stockService;
    }

    [HttpGet(Name = "GetLocations")]
    public async Task<IActionResult> GetLocationsAsync([FromQuery] PageRequest pageRequest)
    {
        var locations = await _locationsService.GetLocationsPageAsync(pageRequest);
        var result = locations.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetLocation")]
    public async Task<IActionResult> GetLocationByIdAsync([FromRoute] Guid id)
    {
        var location = await _locationsService.GetLocationAsync(id);
        var result = location.ToDto();

        return Ok(result);
    }

    [HttpPost(Name = "CreateLocation")]
    public async Task<IActionResult> CreateLocationAsync([FromBody] LocationDto input)
    {
        var location = input.ToModel();
        var createdLocation = await _locationsService.CreateLocationAsync(location);
        var result = createdLocation.ToDto();

        return CreatedAtRoute("GetLocation", new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}", Name = "UpdateLocation")]
    public async Task<IActionResult> UpdateLocationAsync([FromRoute] Guid id, [FromBody] LocationDto input)
    {
        var location = input.ToModel();
        var updatedLocation = await _locationsService.UpdateLocationAsync(id, location);
        var result = updatedLocation.ToDto();

        return Ok(result);
    }

    [HttpDelete("{id:guid}", Name = "DeleteLocation")]
    public async Task<IActionResult> DeleteLocationAsync([FromRoute] Guid id)
    {
        await _locationsService.DeleteLocationAsync(id);

        return Ok();
    }

    [HttpGet("{locationId:guid}/stock", Name = "GetStock")]
    public async Task<IActionResult> GetStockAsync([FromRoute] Guid locationId,
        [FromRoute] PageRequest pageRequest)
    {
        var stock = await _stockService.GetStockPageAsync(locationId, pageRequest);
        var result = stock.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{locationId:guid}/stock/{itemId:guid}", Name = "GetStockForItem")]
    public async Task<IActionResult> GetStockForItemAsync([FromRoute] Guid locationId, [FromRoute] Guid itemId)
    {
        var stock = await _stockService.GetStockForItemAsync(locationId, itemId);
        var result = stock?.ToDto();

        return Ok(result);
    }

    [HttpPut("{locationId:guid}/stock/{itemId:guid}", Name = "UpdateStockForItem")]
    public async Task<IActionResult> UpdateStockForItemAsync([FromRoute] Guid locationId,
        [FromRoute] Guid itemId, [FromBody] StockDto input)
    {
        var stock = input.ToModel();
        var updatedStock = await _stockService.UpdateStockForItemAsync(locationId, itemId, stock);
        var result = updatedStock.ToDto();

        return Ok(result);
    }
}