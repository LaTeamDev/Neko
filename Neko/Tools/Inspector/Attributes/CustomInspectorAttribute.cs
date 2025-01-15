using JetBrains.Annotations;

namespace Neko.Tools; 

[MeansImplicitUse]
public class CustomInspectorAttribute : Attribute {
    public Type InspectorType;

    public CustomInspectorAttribute(Type inspectorType) {
        InspectorType = inspectorType;
    }
}