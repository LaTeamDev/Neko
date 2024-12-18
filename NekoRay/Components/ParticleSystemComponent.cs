namespace NekoRay;

public class ParticleSystemComponent : Behaviour {
    public ParticleSystem ParticleSystem { get; } = new(null);

    void Update() {
        ParticleSystem.Position = Transform.Position;
        ParticleSystem.Update(Time.DeltaF);
    }

    void Render() {
        ParticleSystem.Draw();
    }
}