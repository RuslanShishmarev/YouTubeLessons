namespace SimpleWebFramework.Main.WebFramework.Models.Attributes;

public class ApiControllerAttribute : Attribute
{
    public string Route { get; }

    public ApiControllerAttribute(string route)
    {
        Route = route;
    }
}
