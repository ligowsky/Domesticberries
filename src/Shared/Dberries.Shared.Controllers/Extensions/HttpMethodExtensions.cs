namespace Dberries;

public static class HttpMethodExtensions
{
  public static ActionType ToActionType(this HttpMethod httpMethod) => httpMethod.Method.ToActionType();

  public static ActionType ToActionType(this string method)
  {
    switch (method)
    {
      case "GET":
        return ActionType.Get;
      case "POST":
        return ActionType.Create;
      case "PUT":
        return ActionType.Update;
      case "PATCH":
        return ActionType.Patch;
      case "OPTIONS":
        return ActionType.Options;
      case "DELETE":
        return ActionType.Delete;
      default:
        throw new ArgumentException("Unexpected httpMethod " + method);
    }
  }
}