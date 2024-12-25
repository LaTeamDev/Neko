using System.Numerics;

namespace NekoRay;

public class AudioListener : Behaviour {
    private Vector3 _velocity = Vector3.Zero;
    private Vector3 _prevPosition = Vector3.Zero;
    private Vector3 _dampVelocity = Vector3.Zero;
    private float _smooth = 0.01f;
    
    void Update() {
        _velocity = NekoMath.Damp((Transform.Position - _prevPosition)/Time.DeltaF,_velocity, ref _dampVelocity, _smooth);
        _prevPosition = Transform.Position;
        Audio.SoLoud.Set3dListenerParameters(Transform.Position, Transform.Forward, Vector3.UnitZ, _velocity);
    }
}