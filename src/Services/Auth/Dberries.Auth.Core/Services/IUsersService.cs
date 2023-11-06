namespace Dberries.Auth;

public interface IUsersService
{
    public Task<AuthResponseDto> SignUpAsync(AuthRequestDto request);
    public Task<AuthResponseDto> SignInAsync(AuthRequestDto request);
}