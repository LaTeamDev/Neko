using System.Numerics;
using Box2D;
using ImGuiNET;
using NekoLib;
using NekoLib.Core;

namespace NekoRay.Tools;

[CustomInspector(typeof(NekoLib.Core.Transform))]
public class TransformInspector : Inspector {
    public override void DrawGui() {
        var target = (NekoLib.Core.Transform) Target;
        var pos = target.LocalPosition;
        if (ImGui.DragFloat3("Position", ref pos))
            if (pos != target.LocalPosition) target.LocalPosition = pos;
        var scale = target.LocalScale;
        if (ImGui.DragFloat3("Scale", ref scale))
            if (scale != target.LocalScale) target.LocalScale = scale;
        var rot = target.LocalRotation;
        var rotvec = rot.GetEulerAngles();
        var rotPretty = new Vector3(
            float.RadiansToDegrees(rotvec.X),
            float.RadiansToDegrees(rotvec.Y),
            float.RadiansToDegrees(rotvec.Z)
        );
        if (ImGui.SliderFloat3("Rotation", ref rotPretty, 0, 360f)) {
            rot = Quaternion.CreateFromYawPitchRoll(
                float.RadiansToDegrees(rotvec.X), 
                float.RadiansToDegrees(rotvec.Y), 
                float.RadiansToDegrees(rotvec.Z)
                );
            target.LocalRotation = rot;
        }
    }
}