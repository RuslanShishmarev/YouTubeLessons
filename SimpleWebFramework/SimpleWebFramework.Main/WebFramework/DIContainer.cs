using SimpleWebFramework.Main.WebFramework.Models;

namespace SimpleWebFramework.Main.WebFramework;

internal class DIContainer
{
    private Dictionary<Type, (Type implementation, LifeTime life)> _registeration = new();

    private readonly Dictionary<Type, object> _singletones = new();

    public void Add<TIn, TOut>(LifeTime life)
    {
        _registeration.Add(typeof(TIn), (typeof(TOut), life));
    }

    public object Resolve(Type type, DIScope scope = null)
    {
        if (!_registeration.TryGetValue(type, out var register))
        {
            throw new InvalidOperationException($"Type {type.Name} is not registered");
        }

        (Type implementation, LifeTime life) = register;

        switch (life)
        {
            case LifeTime.Singletone:
                if (!_singletones.TryGetValue(type, out var singletonInstance))
                {
                    singletonInstance = CreateInstance(implementation, scope);
                    _singletones.Add(type, singletonInstance);
                }
                return singletonInstance;
            case LifeTime.Scoped:
                if (!scope.TryGetInstace(type, out var scopeInstance))
                {
                    scopeInstance = CreateInstance(implementation, scope);
                    scope.AddInstance(type, scopeInstance);
                }
                return scopeInstance;
            default:
                return CreateInstance(implementation, scope);
        }
    }

    public object CreateInstance(Type type, DIScope scope)
    {
        var ctor = type.GetConstructors().First();
        var parameters = ctor.GetParameters();
        var args = parameters.Select(x => Resolve(x.ParameterType, scope)).ToArray();
        var instance = ctor.Invoke(args);
        return instance;
    }
}
