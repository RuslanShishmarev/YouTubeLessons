namespace SimpleWebFramework.Lib;

internal class DIScope
{
    private readonly Dictionary<Type, object> _scopedInstance = new();

    public bool TryGetInstace(Type type, out object instance) =>
        _scopedInstance.TryGetValue(type, out instance);

    public void AddInstance(Type type, object instance) => 
        _scopedInstance.Add(type, instance);
}
