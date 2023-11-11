using MassTransit;

namespace Dberries.Auth.Infrastructure;

public class UsersService : IUsersService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IPasswordService _passwordService;
    private readonly ITokenProviderService _tokenProviderService;
    private readonly IPublishEndpoint _publishEndpoint;

    public UsersService(IUsersRepository usersRepository, IPasswordService passwordService,
        ITokenProviderService tokenProviderService, IPublishEndpoint publishEndpoint)
    {
        _usersRepository = usersRepository;
        _passwordService = passwordService;
        _tokenProviderService = tokenProviderService;
        _publishEndpoint = publishEndpoint;

    }

    public async Task<AuthResponseDto> SignUpAsync(AuthRequestDto request)
    {
        await _usersRepository.ThrowIfExistsByEmailAsync(request.Email!);

        var passwordHash = _passwordService.GenerateHash(request.Password!);
        var user = new User(request.Email, passwordHash);
        user = _usersRepository.Add(user);
        await _usersRepository.SaveChangesAsync();

        var message = new UserAddedMessage(user.ToDto());
        await _publishEndpoint.Publish(message);

        var accessToken = _tokenProviderService.GenerateAccessToken(user.Id!.Value);

        return new AuthResponseDto(accessToken);
    }

    public async Task<AuthResponseDto> SignInAsync(AuthRequestDto request)
    {
        var user = await _usersRepository.GetByEmailAsync(request.Email!);
        _passwordService.Validate(request.Password!, user.PasswordHash!);

        var accessToken = _tokenProviderService.GenerateAccessToken(user.Id!.Value);

        return new AuthResponseDto(accessToken);
    }
}