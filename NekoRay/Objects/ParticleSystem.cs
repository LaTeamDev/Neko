﻿using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace NekoRay;

public class ParticleSystem : NekoObject {
    private readonly ParticlePool _pool;
    
    public ParticleSystem(Texture texture, int poolSize = 1024) {
        _pool = new ParticlePool(poolSize);
    }
    
    private readonly List<int> _activeParticleIndices = new();

    public int PoolSize {
        get => _pool.Size;
        set {
            if (value < _activeParticleIndices.Count)
                throw new InvalidOperationException("Cannot resize pool smaller than the number of active particles.");
            _pool.Resize(value);
        }
    }

    public int Count => _activeParticleIndices.Count;
    
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
        Paused = !Paused;
    }

    public void Reset() {
        _pool.Reset();
        _activeParticleIndices.Clear();
        Stop();
    }
    
    public void Draw() {
        foreach (var idx in _activeParticleIndices) {
            ref var p = ref _pool.GetParticleRef(idx); 
            Raylib.DrawPixelV(p.Position.ToVector2(), p.Color);
        }
    }
    
    private float _emissionAccumulator = 0f;
    
    public void Update(float dt) {
        if (!Active) return;
        foreach (var idx in _activeParticleIndices.ToList()) {
            ref var p = ref _pool.GetParticleRef(idx); 
            UpdateParticle(ref p, dt);
            if (p.Lifetime <= 0) {
                _pool.Return(idx);
                _activeParticleIndices.Remove(idx);
            }
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (_emitterLife == -1) return;
    
        _emitterLife -= dt;
        if (_emitterLife < 0f) return;

        _emissionAccumulator += dt;
        float emissionInterval = 1.0f / EmissionRate;
        while (_emissionAccumulator >= emissionInterval) {
            Emit();
            _emissionAccumulator -= emissionInterval;
        }
    }

    public void Emit(int amount) {
        for (var i = 0; i < amount; i++) {
            Emit();
        }
    }

    public void Emit() {
        var idx = _pool.Rent();
        if (idx == -1)
            return;
        ref var p = ref _pool.GetParticleRef(idx); 
        if (p.IsNull) return;
        p.Lifetime = ParticleLifetime.min+(ParticleLifetime.max-ParticleLifetime.min)*Random.NextSingle();
        p.Color = Color;
        p.Position = Position;
        var direction = Vector3.Transform(Direction, Rotation);
        var spread = new Vector3(
            (Random.NextSingle() - 0.5f) * Spread.X,
            (Random.NextSingle() - 0.5f) * Spread.Y,
            (Random.NextSingle() - 0.5f) * Spread.Z
        );
        p.Velocity = (direction + spread) * Speed;

        _activeParticleIndices.Add(idx);
    }

    private void UpdateParticle(ref Particle p, float dt) {
        p.Color = Color;
        p.Velocity += Acceleration * dt;
        //p.Velocity *= MathF.Pow(LinearDamping, dt);
        p.Position += p.Velocity*dt;
        p.Lifetime -= dt;
        if (TangentialAcceleration != 0) {
            var tangent = Vector3.Cross(p.Velocity, Vector3.UnitZ);
            if (tangent != Vector3.Zero) {
                tangent = Vector3.Normalize(tangent);
                p.Velocity += tangent * (TangentialAcceleration * dt);
            }
        }
    }

    public void Stop() {
        Active = false;
        Paused = false;
        Stopped = true;
        _emitterLife = EmitterLifetime;
    }
    
    public void Start() {
        Paused = false;
        if (Active) return;
        Active = true;
        Stopped = false;
        _emitterLife = EmitterLifetime;
    }

    public override void Dispose() {
        base.Dispose();
        _pool.Dispose();
    }
}

internal class ParticlePool : IDisposable {
    private IMemoryOwner<Particle> _memoryOwner;
    private Memory<Particle> _particles;
    private readonly Stack<int> _freeIndices;
    private int _activeCount;
    private bool _isDisposed;
    private readonly object _lock = new object();

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
        lock(_lock) {
            var span = _particles.Span;
            for (var i = 0; i < span.Length; i++) {
                span[i].Reset();
                span[i].IsActive = false;
                _freeIndices.Push(i);
            }
            _activeCount = 0;
        }
    }

    public int Rent() {
        return Rent(out _);
    }
    
    public int Rent(out Particle? particle) {
        lock(_lock) {
            if (IsFull) {
                particle = null;
                return -1;
            }
            
            var index = _freeIndices.Pop();
            ref var p = ref _particles.Span[index];
            p.IsActive = true;
            _activeCount++;
            particle = p;
            return index;
        }
    }

    public ref Particle GetParticleRef(int index) {
        if (index < 0 || index >= _particles.Length)
            throw new ArgumentOutOfRangeException(nameof(index));
        return ref _particles.Span[index];
    }

    public void Resize(int newSize) {
        lock(_lock) {
            if (newSize <= 0) throw new ArgumentException("New size must be greater than 0", nameof(newSize));
            if (newSize == Size) return;

            var newMemoryOwner = MemoryPool<Particle>.Shared.Rent(newSize);
            var newParticles = newMemoryOwner.Memory;

            var newSpan = newParticles.Span;

            // Initialize new particles
            for (var i = 0; i < newSpan.Length; i++) {
                newSpan[i].Reset();
                newSpan[i].IsActive = false;
                _freeIndices.Push(i);
            }

            // Dispose old memory
            _memoryOwner.Dispose();
            _memoryOwner = newMemoryOwner;
            _particles = newParticles;
            _activeCount = 0;
        }
    }

    public void Return(int index) {
        lock(_lock) {
            if (index < 0 || index >= _particles.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            ref var p = ref _particles.Span[index];
            if (!p.IsActive) return;

            p.Reset();
            p.IsActive = false;
            _freeIndices.Push(index);
            _activeCount--;
        }
    }

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