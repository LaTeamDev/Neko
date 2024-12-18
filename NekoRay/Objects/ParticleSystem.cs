using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace NekoRay;

public class ParticleSystem : NekoObject {
    private readonly ParticlePool _pool;
    
    public ParticleSystem(Texture texture, int poolSize = 1024) {
        _pool = new ParticlePool(poolSize);
    }
    
    public int PoolSize {
        get => _pool.Size;
        set => _pool.Resize(value);
    }

    public int Count => _pool.ActiveCount;
    
    public Vector3 Direction { get; }
    public Vector3 Position { get; set; }
    public float EmissionRate { get; set; } = 1;
    public float EmitterLifetime { get; set; } = 1;
    private float _emitterLife = 0f;
    public Vector3 Acceleration { get; set; } = Vector3.Zero;
    public float LinearDamping { get; set; } = 1;
    public (float min, float max) ParticleLifetime { get; set; } = new (1, 1);
    public Quaternion Rotation { get; set; } = Quaternion.Identity;
    public float Size { get; set; } = 1;
    public (float min, float max) SizeVariation { get; set; } = new (1, 1);
    public float Speed { get; set; } = 1;
    public Quaternion Spin { get; set; } = Quaternion.Identity;
    public (float min, float max) SpinVariation { get; set; } = new (1, 1);
    public Vector3 Spread { get; set; } = Vector3.Zero;
    public float TangentialAcceleration { get; set; } = 0;
    public bool RelativeRotation = false;
    public Color Color = Raylib.WHITE;
    public Random Random { get; private set; } = new();

    public bool Active { get; private set; } = false;

    public bool Paused { get; private set; } = false;
    
    public bool Stopped { get; private set; } = true;
    
    public void Pause() {
        Paused = true;
    }

    public void Reset() {
        _pool.Reset();
        Stop();
    }
    
    public void Draw() {
        foreach (var p in _pool.GetActiveParticles()) {
            Raylib.DrawPixelV(p.Position.ToVector2(), p.Color);
        }
    }
    
    public void Update(float dt) {
        if (!Active) return;
        foreach (ref var p in _pool.GetActiveParticles()) {
            UpdateParticle(ref p, dt);
            if (p.Lifetime <= 0) {
                _pool.Return(ref p);
            }
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_emitterLife == -1) return;
        
        var meow = _emitterLife;
        _emitterLife -= dt;
        if (_emitterLife < 0f) return;
        while (meow >= _emitterLife) {
            meow -= EmissionRate;
            Emit();
        }
    }

    public void Emit(int amount) {
        for (var i = 0; i < amount; i++) {
            Emit();
        }
    }

    public void Emit() {
        var p = _pool.Rent();
        if (p.IsNull) return;
        p.Lifetime = ParticleLifetime.min+(ParticleLifetime.max-ParticleLifetime.min)*Random.NextSingle();
        p.Color = Color;
        p.Position = Position;
    }

    private void UpdateParticle(ref Particle p, float dt) {
        p.Color = Color;
        p.Velocity += Acceleration * dt;
        p.Velocity *= LinearDamping * dt;
        p.Position += p.Velocity*dt;
        p.Lifetime -= dt;
    }

    public void Stop() {
        Active = false;
        Paused = false;
        Stopped = true;
        _emitterLife = EmitterLifetime;
    }
    
    public void Start() {
        if (Active) return;
        Active = true;
        if (Paused) {
            Paused = false;
            return;
        }
        Stopped = false;
        _emitterLife = EmitterLifetime;
    }

    public override void Dispose() {
        base.Dispose();
        _pool.Dispose();
    }
}

internal class ParticlePool {
    private IMemoryOwner<Particle> _memoryOwner;
    private Memory<Particle> _particles;
    private readonly Stack<int> _freeIndices;
    
    private int _activeCount;
    private bool _isDisposed;

    public int Size => _particles.Length;
    public int ActiveCount => _activeCount;
    public bool IsFull => _freeIndices.Count == 0;

    public ParticlePool(int initialSize) {
        var memoryPool = MemoryPool<Particle>.Shared;
        _memoryOwner = memoryPool.Rent(initialSize);
        _particles = _memoryOwner.Memory;
        _freeIndices = new Stack<int>(initialSize);
        _activeCount = 0;
        
        Reset();
    }

    public void Reset() {
        var span = _particles.Span;
        for (var i = 0; i < span.Length; i++) {
            span[i].Reset();
            span[i].IsActive = false;
            _freeIndices.Push(i);
        }
    }

    private Particle NullParticle = new Particle {IsNull = true};

    public ref Particle Rent() {
        if (_freeIndices.Count == 0)
            return ref NullParticle;
        
        var index = _freeIndices.Pop();
        ref var particle = ref _particles.Span[index];
        particle.IsActive = true;
        _activeCount++;
        return ref particle;
    }
    
    private int GetParticleIndex(ref Particle particle) {
        var span = _particles.Span;
        for (var i = 0; i < span.Length; i++) {
            if (span[i] == particle)
                return i;
        }
        return -1;
    }
    
    public void Resize(int newSize) {
        if (newSize <= 0) throw new ArgumentException("New size must be greater than 0", nameof(newSize));
        if (newSize == Size) return;

        var newMemoryOwner = MemoryPool<Particle>.Shared.Rent(newSize);
        var newParticles = newMemoryOwner.Memory;
        
        var oldSpan = _particles.Span;
        var newSpan = newParticles.Span;
        var activeParticles = new List<Particle>();
        
        for (var i = 0; i < oldSpan.Length; i++) {
            if (oldSpan[i].IsActive) {
                activeParticles.Add(oldSpan[i]);
            }
        }
        
        _freeIndices.Clear();

        // Mark all new particles as inactive initially
        for (var i = newSize - 1; i >= 0; i--) {
            newSpan[i].IsActive = false;
            _freeIndices.Push(i);
        }

        // Copy active particles to new array
        foreach (var particle in activeParticles) {
            if (_freeIndices.Count == 0) break; // No more space in new array

            var newIndex = _freeIndices.Pop();
            newSpan[newIndex] = particle;
        }

        // Dispose old memory and update references
        _memoryOwner.Dispose();
        _memoryOwner = newMemoryOwner;
        _particles = newParticles;
        _activeCount = Math.Min(_activeCount, newSize);
    }

    public void Return(ref Particle particle) {
        if (!particle.IsActive) return;

        var index = GetParticleIndex(ref particle);
        if (index == -1) return;

        particle.Reset();
        particle.IsActive = false;
        _freeIndices.Push(index);
        _activeCount--;
    }
    
    public Span<Particle> GetActiveParticles() => _particles.Span;

    public void Dispose() {
        if (_isDisposed) return;
        
        _memoryOwner.Dispose();
        _freeIndices.Clear();
        _isDisposed = true;
    }
}

internal record struct Particle {
    public bool IsNull;
    public Vector3 Position;
    public Quaternion Rotation;
    public Vector3 Velocity;
    public RayColor Color;
    public float Lifetime;
    public bool IsActive;

    public void Reset(Vector3 position = default, Quaternion rotation = default, Vector3 velocity = default, RayColor color = default) {
        Position = position;
        Rotation = rotation;
        Velocity = velocity;
        Color = color;
    }
}