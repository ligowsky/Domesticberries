using BitzArt.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Dberries.Warehouse.WebAPI;

[Route("[Controller]")]
public class LocationsController : DberriesController
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPageAsync([FromQuery] PageRequest pageRequest)
    {
        var locations = await _locationsService.GetPageAsync(pageRequest);
        var result = locations.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetLocation")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var location = await _locationsService.GetAsync(id);
        var result = location.ToDto();

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddAsync([FromBody] LocationDto input)
    {
        Validate(input);
        var location = input.ToModel();
        var createdLocation = await _locationsService.AddAsync(location);
        var result = createdLocation.ToDto();

        return CreatedAtRoute("GetLocation", new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] LocationDto input)
    {
        Validate(input);
        var location = input.ToModel();
        var updatedLocation = await _locationsService.UpdateAsync(id, location);
        var result = updatedLocation.ToDto();

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> RemoveAsync([FromRoute] Guid id)
    {
        await _locationsService.RemoveAsync(id);

        return Ok();
    }

    [HttpGet("{locationId:guid}/stock")]
    public async Task<IActionResult> GetStockPageAsync([FromRoute] Guid locationId,
        [FromQuery] PageRequest pageRequest)
    {
        var stock = await _locationsService.GetStockPageAsync(locationId, pageRequest);
        var result = stock.Convert(x => x.ToDto());

        return Ok(result);
    }

    [HttpGet("{locationId:guid}/stock/{itemId:guid}")]
    public async Task<IActionResult> GetStockAsync([FromRoute] Guid locationId, [FromRoute] Guid itemId)
    {
        var stock = await _locationsService.GetStockAsync(locationId, itemId);
        var result = stock?.ToDto();

        return Ok(result);
    }

    [HttpPut("{locationId:guid}/stock/{itemId:guid}")]
    public async Task<IActionResult> UpdateStockAsync([FromRoute] Guid locationId,
        [FromRoute] Guid itemId, [FromBody] StockDto input)
    {
        Validate(input);
        var stock = input.ToModel();
        var updatedStock = await _locationsService.UpdateStockAsync(locationId, itemId, stock);
        var result = updatedStock?.ToDto();

        return Ok(result);
    }
}