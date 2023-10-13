namespace Dberries;

public static class HttpMethodExtensions
{
    public static ActionType ToActionType(this string method)
    {
        return method switch
        {
            "GET" => ActionType.Get,
            "POST" => ActionType.Create,
            "PUT" => ActionType.Update,
            "PATCH" => ActionType.Patch,
            "OPTIONS" => ActionType.Options,
            "DELETE" => ActionType.Delete,
            _ => throw new ArgumentException("Unexpected httpMethod " + method)
        };
    }
}