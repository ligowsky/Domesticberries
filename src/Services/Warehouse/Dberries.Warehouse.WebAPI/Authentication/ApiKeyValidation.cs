namespace Dberries.Warehouse.Authentication;

public class ApiKeyValidation : IApiKeyValidation
{
    private readonly IConfiguration _configuration;

    public ApiKeyValidation(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsValidApiKey(string userApiKey)
    {
        if (string.IsNullOrWhiteSpace(userApiKey))
            return false;

        var apiKey = _configuration.GetValue<string>(AuthConstants.ApiKeySectionName);

        return apiKey != null && apiKey == userApiKey;
    }
}