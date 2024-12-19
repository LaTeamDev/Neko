using System.Numerics;
using ImGuiNET;

namespace NekoRay.Tools;

[CustomInspector(typeof(ParticleSystemComponent))]
public class ParticleSystemInspector : Inspector {
    public override void DrawGui() {
        var target = (ParticleSystemComponent) Target;
        var ps = target.ParticleSystem;
        
        ImGui.Text($"Active: {ps.Count}/{ps.PoolSize}");
        
        if (ps.Active) ImGui.BeginDisabled();
        if (ImGui.Button("Start")) ps.Start();
        if (ps.Active) ImGui.EndDisabled();
        ImGui.SameLine();
        if (ps.Stopped) ImGui.BeginDisabled();
        if (ImGui.Button("Stop")) ps.Stop();
        if (ps.Stopped) ImGui.EndDisabled();
        ImGui.SameLine();
        if (ps.Paused) ImGui.BeginDisabled();
        if (ImGui.Button("Pause")) ps.Pause();
        if (ps.Paused) ImGui.EndDisabled();
        ImGui.SameLine();
        if (ImGui.Button("Reset")) ps.Reset();
        ImGui.SameLine();
        if (ImGui.Button("Emit")) ps.Emit();
        
        var emitterLifetime = ps.EmitterLifetime;
        if (ImGui.DragFloat("Emitter Lifetime", ref emitterLifetime)) {
            ps.EmitterLifetime = emitterLifetime;
        }
        
        var emissionRate = ps.EmissionRate;
        if (ImGui.DragFloat("Emission Rate", ref emissionRate)) {
            ps.EmissionRate = emissionRate;
        }
        
        var linearDamping = ps.LinearDamping;
        if (ImGui.DragFloat("Linear Damping", ref linearDamping)) {
            ps.LinearDamping = linearDamping;
        }
        
        var acceleration = ps.Acceleration;
        if (ImGui.DragFloat3("Acceleration", ref acceleration)) {
            ps.Acceleration = acceleration;
        }
        
        var particleLifetime = ps.ParticleLifetime;
        var paricleLifetimeV = new Vector2(particleLifetime.min, particleLifetime.max);
        if (ImGui.DragFloat2("Particle Lifetime", ref paricleLifetimeV)) {
            ps.ParticleLifetime = new(paricleLifetimeV.X, paricleLifetimeV.Y);
        }
        
        var color = ps.Color.ToVector4();
        if (ImGui.ColorEdit4("Color", ref color)) {
            ps.Color = color.ToColor();
        }
    }
}