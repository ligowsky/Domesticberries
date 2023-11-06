namespace Dberries.Auth.Infrastructure;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenService _tokenService;

    public UsersService(IUsersRepository usersRepository, IPasswordService passwordService, ITokenService tokenService)
    {
        _usersRepository = usersRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<AuthResponseDto> SignUpAsync(AuthRequestDto request)
    {
        await _usersRepository.ThrowIfExistsByEmailAsync(request.Email!);

        var passwordHash = _passwordService.GenerateHash(request.Password!);

        var user = new User(request.Email, passwordHash);
        user = _usersRepository.Add(user);
        await _usersRepository.SaveChangesAsync();

        var accessToken = _tokenService.GenerateAccessToken(user.Id!.Value);

        return new AuthResponseDto(accessToken);
    }

    public async Task<AuthResponseDto> SignInAsync(AuthRequestDto request)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email!);

        _passwordService.Validate(request.Password!, user.PasswordHash!);

        var accessToken = _tokenService.GenerateAccessToken(user.Id!.Value);

        return new AuthResponseDto(accessToken);
    }
}