using Microsoft.AspNetCore.Mvc;

namespace Dberries.Auth.WebAPI;

[Route("")]
public class UsersController : DberriesController
{
    private readonly IUsersService _usersService;

    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpPost("signup", Name = "SignUp")]
    public async Task<IActionResult> SignUpAsync([FromBody] AuthRequestDto request)
    {
        Validate(request);
        var result = await _usersService.SignUpAsync(request);

        return Ok(result);
    }

    [HttpPost("signin", Name = "SignIn")]
    public async Task<IActionResult> SignInAsync([FromBody] AuthRequestDto request)
    {
        var result = await _usersService.SignInAsync(request);

        return Ok(result);
    }

    [HttpPost("refresh", Name = "Refresh")]
    public IActionResult Refresh([FromBody] RefreshTokenRequestDto request)
    {
        var result = _usersService.RefreshTokenAsync(request);

        return Ok(result);
    }
}