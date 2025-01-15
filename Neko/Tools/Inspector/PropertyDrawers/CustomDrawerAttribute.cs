using JetBrains.Annotations;

namespace Neko.Tools;

[MeansImplicitUse]
public class CustomDrawerAttribute(Type drawerType) : Attribute {
    public Type DrawerType = drawerType;
}