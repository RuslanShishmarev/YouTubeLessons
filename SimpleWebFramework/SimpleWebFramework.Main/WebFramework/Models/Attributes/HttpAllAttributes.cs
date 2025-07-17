namespace SimpleWebFramework.Main.WebFramework.Models.Attributes;

public class HttpGetAttribute : HttpMethodAttribute
{
    public HttpGetAttribute() : base(string.Empty, HttpMethodType.Get) { }
    public HttpGetAttribute(string route) : base(route, HttpMethodType.Get) { }
}

public class HttpPostAttribute : HttpMethodAttribute
{
    public HttpPostAttribute() : base(string.Empty, HttpMethodType.Post) { }
    public HttpPostAttribute(string route) : base(route, HttpMethodType.Post) { }
}

public class HttpPatchAttribute : HttpMethodAttribute
{
    public HttpPatchAttribute() : base(string.Empty, HttpMethodType.Patch) { }
    public HttpPatchAttribute(string route) : base(route, HttpMethodType.Patch) { }
}

public class HttpDeleteAttribute : HttpMethodAttribute
{
    public HttpDeleteAttribute() : base(string.Empty, HttpMethodType.Delete) { }
    public HttpDeleteAttribute(string route) : base(route, HttpMethodType.Delete) { }
}
