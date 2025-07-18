namespace SimpleWebFramework.Lib.Models;

public abstract class BaseController
{
    protected string GetHTMLPage(string name)
    {
        string controllerName = GetType().Name.Replace("Controller", string.Empty);
        return HttpServerListener.GetHTMLPage($"{controllerName}\\{name}");
    }

    protected string GetHTMLPage(string name, Dictionary<string, object> context)
    {
        string controllerName = GetType().Name.Replace("Controller", string.Empty);
        return HttpServerListener.GetHTMLPage($"{controllerName}\\{name}", context);
    }
}
