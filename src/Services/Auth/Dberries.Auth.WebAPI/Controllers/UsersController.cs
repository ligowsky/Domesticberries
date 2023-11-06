using Microsoft.AspNetCore.Mvc;

namespace Dberries.Auth.WebAPI;

[Route("[Controller]")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUpAsync([FromBody] AuthRequestDto request)
    {
        var result = await _usersService.SignUpAsync(request);

        return Ok(result);
    }
}