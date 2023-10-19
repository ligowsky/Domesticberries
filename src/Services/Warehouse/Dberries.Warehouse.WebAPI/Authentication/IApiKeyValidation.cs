namespace Dberries.Warehouse.Authentication;

public interface IApiKeyValidation
{
    bool IsValidApiKey(string userApiKey);
}