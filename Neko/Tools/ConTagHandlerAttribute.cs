using JetBrains.Annotations;

namespace Neko.Tools; 

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Method)]
public class ConTagHandlerAttribute : Attribute {
    public string Tag;

    public ConTagHandlerAttribute(string tag) {
        Tag = tag;
    }
}