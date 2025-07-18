using SimpleWebFramework.Lib.Models.Attributes;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleWebFramework.Lib.Models;

public class HttpFrameMethod
{
    public Func<object[], object?> Func { get; }

    public string Route { get; }

    public HttpMethodType MethodType { get; }

    public MethodInfo MethodInfo { get; }

    public bool WithPattern { get; }

    private List<(string name, Type type)> _parametersTypes = new();

    private (string name, Type type)? _fromBodyParameter;

    public HttpFrameMethod(
        MethodInfo methodInfo,
        Func<object[], object?> func,
        string route,
        HttpMethodType methodType,
        bool withPattern)
    {
        MethodInfo = methodInfo;
        Func = func;
        Route = route;
        MethodType = methodType;
        WithPattern = withPattern;

        foreach (var parameter in methodInfo.GetParameters())
        {
            var fromBodyAttribute = parameter.GetCustomAttribute<FromBodyAttribute>();
            if (fromBodyAttribute != null)
            {
                _fromBodyParameter = (parameter.Name, parameter.ParameterType);
                continue;
            }
            _parametersTypes.Add((parameter.Name, parameter.ParameterType));
        }
    }

    public object? Invoke(Dictionary<string, string> parameters, string? body = null)
    {
        var args = new object[_parametersTypes.Count];

        int offset = 0;

        if (_fromBodyParameter != null)
        {
            offset = 1;
            args = new object[_parametersTypes.Count + offset];
            
            if (_fromBodyParameter.Value.type == typeof(string) || body is null)
            {
                args[0] = body;
            }
            else
            {
                args[0] = JsonSerializer.Deserialize(body, _fromBodyParameter.Value.type);
            }
        }

        for (int i = 0; i < _parametersTypes.Count; i++)
        {
            (var paramName, var paramType) = _parametersTypes[i];

            string value = parameters[paramName];
            args[i + offset] = ConvertStringToType(value, paramType);
        }
        return Func.Invoke(args);
    }

    public object ConvertStringToType(string str, Type targetType)
    {
        if (targetType == typeof(string)) return str;

        if (string.IsNullOrWhiteSpace(str))
        {
            // Если тип Nullable, возвращаем null
            if (Nullable.GetUnderlyingType(targetType) != null)
                return null;
        }

        try
        {
            Type nonNullableType = Nullable.GetUnderlyingType(targetType) ?? targetType;
            return Convert.ChangeType(str, nonNullableType);
        }
        catch
        {
            return null; // или выбросить исключение
        }
    }
}
