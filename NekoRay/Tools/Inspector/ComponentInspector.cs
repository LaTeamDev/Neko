using System.Reflection;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomInspector(typeof(Component))]
public class ComponentInspector : ObjectInspector {
    public override void Initialize() {
        base.Initialize();
        
        Members.RemoveAll(Match);
        var target = ((Component) Target);
        if (target is Behaviour) _isBehaviour = true;
    }

    private bool _isBehaviour;

    private bool Match(MemberInfo info) {
        if (info.DeclaringType is null)
            return false;
        if (!(info.DeclaringType.IsAssignableFrom(typeof(Component)) || info.DeclaringType.IsAssignableFrom(typeof(Behaviour))))
            return false;
        return true;
    }
}