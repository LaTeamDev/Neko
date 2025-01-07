namespace NekoRay.Tools;

public class RequireComponentAttribute(params Type[] types) : Attribute {
    public Type[] ComponentTypes = types;
}