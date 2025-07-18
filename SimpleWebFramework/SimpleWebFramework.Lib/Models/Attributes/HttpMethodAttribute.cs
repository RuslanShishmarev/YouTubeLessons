namespace SimpleWebFramework.Lib.Models.Attributes;

public abstract class HttpMethodAttribute : Attribute
{
    public string Route { get; } = string.Empty;

    public HttpMethodType MethodType { get; }

    public HttpMethodAttribute(string route, HttpMethodType methodType)
    {
        Route = route;
        MethodType = methodType;
    }
}
