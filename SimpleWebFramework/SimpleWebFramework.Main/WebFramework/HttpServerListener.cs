using Scriban;
using SimpleWebFramework.Main.WebFramework.Models;
using SimpleWebFramework.Main.WebFramework.Models.Attributes;

using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SimpleWebFramework.Main.WebFramework;

public class HttpServerListener
{
    private readonly string _host;
    private readonly HttpListener _listener;
    private bool _isRunning;

    private static Assembly _currentAssembly;
    private readonly string _indexHtml;
    private readonly string _errorHtml;

    private const string MAIN_PAGE_RESOURCE_NAME = "MainPage.html";
    private const string ERROR_RESOURCE_NAME = "Error404.html";

    private Dictionary<string, Dictionary<HttpMethodType, HttpFrameMethod>> _endPoints = new();

    private List<(string route, HttpFrameMethod method)> _endPointsWithPatterns = new();

    private Dictionary<string, HttpFrameMethod> _endPointsWithPatternCache = new();

    private DIContainer _container = new();

    public HttpServerListener(string host)
    {
        _host = host;
        _listener = new HttpListener();
        _listener.Prefixes.Add(_host);

        _currentAssembly = Assembly.GetExecutingAssembly();

        _indexHtml = GetHTMLPage(MAIN_PAGE_RESOURCE_NAME);
        _errorHtml = GetHTMLPage(ERROR_RESOURCE_NAME);

        foreach (var commonType in _currentAssembly.GetExportedTypes())
        {
            var attributeTop = commonType.GetCustomAttribute<ApiControllerAttribute>();
            if (attributeTop is null) continue;

            MethodInfo[] allPoints = commonType.GetMethods();

            foreach (MethodInfo point in allPoints)
            {
                var methodAttribure = point.GetCustomAttribute<HttpMethodAttribute>();
                if (methodAttribure is null) continue;

                HttpFrameMethod httpMethod = GetHttpMethod(
                    controllerRoute: attributeTop.Route,
                    point: point,
                    methodAttribure: methodAttribure,
                    controllerType: commonType);

                if (httpMethod.WithPattern)
                {
                    _endPointsWithPatterns.Add((httpMethod.Route, httpMethod));
                    continue;
                }

                if (_endPoints.TryGetValue(httpMethod.Route, out var methodsByRouteAndType))
                {
                    if (methodsByRouteAndType.TryGetValue(httpMethod.MethodType, out var methods))
                    {
                        throw new Exception("Method is already created");
                    }
                    methodsByRouteAndType[httpMethod.MethodType] = httpMethod;
                }
                else
                {
                    _endPoints[httpMethod.Route] = new()
                    {
                        { httpMethod.MethodType, httpMethod }
                    };
                }
            }
        }
    }

    public static string GetHTMLPage(string name)
    {
        var folderOfSRC = Directory.GetParent(_currentAssembly.Location) + "\\src\\html";
        var indexHtmlPath = $"{folderOfSRC}\\{name}";
        return File.ReadAllText(indexHtmlPath);
    }

    public static string GetHTMLPage(string name, Dictionary<string, object> context)
    {
        var folderOfSRC = Directory.GetParent(_currentAssembly.Location) + "\\src\\html";
        var indexHtmlPath = $"{folderOfSRC}\\{name}";
        string result = File.ReadAllText(indexHtmlPath);

        var template = Template.Parse(result);
        result = template.Render(context);
        return result;
    }

    public async Task Start()
    {
        _listener.Start();
        _isRunning = true;
        Process.Start("explorer.exe", _host);

        while (_isRunning)
        {
            try
            {
                HttpListenerContext context = await _listener.GetContextAsync();
                var result = ProcessRequest(context);

                var buffer = Encoding.UTF8.GetBytes(result);

                context.Response.ContentLength64 = buffer.Length;
                await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                context.Response.OutputStream.Close();
            }
            catch (HttpListenerException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _listener.Stop();
        _listener.Close();
    }

    public void Add<TIn, TOut>(LifeTime life) => _container.Add<TIn, TOut>(life);

    private HttpFrameMethod GetHttpMethod(
        string controllerRoute,
        MethodInfo point,
        HttpMethodAttribute methodAttribure,
        Type controllerType)
    {
        string route = $"/{controllerRoute}" +
            (string.IsNullOrEmpty(methodAttribure.Route) ? string.Empty : $"/{methodAttribure.Route}");
        
        return new(
            methodInfo: point,
            route: route,
            methodType: methodAttribure.MethodType,
            func: (args) =>
            {
                var scope = new DIScope();
                var controllerContext = _container.CreateInstance(controllerType, scope);
                var result = point.Invoke(controllerContext, args);
                return result;
            },
            withPattern: route.Contains("{") && route.Contains('}'));

    }

    private bool IsMatchRoute(string template, string path)
    {
        // Преобразуем шаблон в регулярное выражение
        string pattern = "^" + Regex.Replace(template, @"\{(\w+)\}", @"[^/]+") + "$";
        return Regex.IsMatch(path, pattern);
    }

    private string ProcessRequest(HttpListenerContext context)
    {
        SetResponseType(context.Response, HttpResponseType.HTML);
        if (string.IsNullOrEmpty(context.Request.RawUrl) ||
            context.Request.RawUrl.Length == 1)
        {
            context.Response.StatusCode = 200;
            return _indexHtml;
        }

        string route = context.Request.RawUrl;

        if (route.Contains('?'))
        {
            route = route.Substring(0, route.IndexOf('?'));
        }

        var parameters = new Dictionary<string, string>();
        if (context.Request.QueryString.Count > 0)
        {
            foreach (string key in context.Request.QueryString.AllKeys)
            {
                parameters[key] = context.Request.QueryString[key];
            }
        }

        var func = GetFuncFromRequest(route, context.Request.HttpMethod);

        string? body = null;
        // get body from request
        if (context.Request.ContentLength64 > 0)
        {
            using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
            body = reader.ReadToEnd();
        }

        if (func != null)
        {
            if (func.WithPattern)
            {
                string regexPattern = "^" + Regex.Replace(
                    func.Route, @"\{(\w+)\}",
                    m => $"(?<{m.Groups[1].Value}>[^/]+)") + "$";
                var match = Regex.Match(route, regexPattern);
                if (match.Success)
                {
                    foreach (var groupName in match.Groups.Keys.Where(k => k != "0"))
                    {
                        parameters[groupName] = match.Groups[groupName].Value;
                    }
                }
            }
            var result = func?.Invoke(parameters, body);

            if (result is ActionResult actionResult)
            {
                SetResponseType(context.Response, actionResult.ResponseType);

                return actionResult.Result?.ToString() ?? string.Empty;
            }

            SetResponseType(context.Response, HttpResponseType.JSON);
            if (result is string)
                return result?.ToString() ?? string.Empty;
            return JsonSerializer.Serialize(result);
        }
        context.Response.StatusCode = 404;
        return _errorHtml;
    }

    private HttpFrameMethod GetFuncFromRequest(string route, string method)
    {
        if (_endPoints.TryGetValue(route, out var pointsByHttpType))
        {
            return pointsByHttpType[ParseByName<HttpMethodType>(method)];
        }
        else
        {
            if (_endPointsWithPatternCache.TryGetValue(route, out var cachedMethod))
            {
                return cachedMethod;
            }
            foreach (var routePattern in _endPointsWithPatterns)
            {
                if (IsMatchRoute(routePattern.route, route))
                {
                    _endPointsWithPatternCache.Add(route, routePattern.method);
                    return routePattern.method;
                }
            }
        }
        return null;
    }

    private TEnum ParseByName<TEnum>(string value) where TEnum : struct, Enum
    {
        foreach (var name in Enum.GetNames(typeof(TEnum)))
        {
            if (name.Equals(value, StringComparison.OrdinalIgnoreCase))
            {
                return (TEnum)Enum.Parse(typeof(TEnum), name);
            }
        }
        throw new ArgumentException($"Значение '{value}' не найдено в перечислении {typeof(TEnum).Name}");
    }

    private void SetResponseType(
        HttpListenerResponse response,
        HttpResponseType responseType)
    {
        switch (responseType)
        {
            case HttpResponseType.HTML:
                response.ContentType = "text/html; charset=utf-8";
                break;
            case HttpResponseType.JSON:
                response.ContentType = "application/json; charset=utf-8";
                break;
            case HttpResponseType.XML:
                response.ContentType = "application/xml; charset=utf-8";
                break;
            default:
                break;
        }
    }
}
