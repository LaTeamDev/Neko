using Box2D;
using ImGuiNET;
using NekoLib.Tools;

namespace Neko.Tools;

[CustomDrawer(typeof(Rotation))]
public class PhysicsRotationDrawer : SimpleDrawer<Rotation> {
    protected override bool DrawInput(string label, ref Rotation value) {
        var angle = float.RadiansToDegrees(value.Angle);
        if (ImGui.DragFloat(label, ref angle, 1f)) {
            value = new Rotation(float.DegreesToRadians(angle));
            return true;
        }
        return false;
    }
}