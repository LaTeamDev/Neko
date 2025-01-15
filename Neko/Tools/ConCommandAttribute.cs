using JetBrains.Annotations;

namespace Neko.Tools; 

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class ConCommandAttribute : Attribute {
    private string _name;
    public string Name => _name;

    public ConCommandAttribute(string name) {
        _name = name;
    }
}