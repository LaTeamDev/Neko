using JetBrains.Annotations;

namespace Neko.Tools; 

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Property)]
public class ConVariableAttribute : Attribute {
    private string _name;
    public string Name => _name;

    public ConVariableAttribute(string name) {
        _name = name;
    }
}