﻿using System.Numerics;
using Box2D;

namespace Neko.Physics2D; 

public class Rigidbody2D : Behaviour {
    private World _world;
    private BodyDef _bodyDef = new();
    private Body? _body;

    private IBody CurrentStuff {
        get {
            if (IsReady) return _body;
            return _bodyDef;
        }
    }
    public World World => _world;
    
    void Awake() {
        _world = GameObject.Scene.GetWorld();
    }
    
    public bool IsReady => _body is not null;
    
    public BodyType Type {
        get => CurrentStuff.Type;
        set => CurrentStuff.Type = value;
    }

    public Vector2 Position {
        get => CurrentStuff.Position;
        set => CurrentStuff.Position = value;
    }

    public Rotation Rotation {
        get => CurrentStuff.Rotation;
        set => CurrentStuff.Rotation = value;
    }

    public Vector2 LinearVelocity {
        get => CurrentStuff.LinearVelocity;
        set => CurrentStuff.LinearVelocity = value;
    }

    public float AngularVelocity {
        get => CurrentStuff.AngularVelocity;
        set => CurrentStuff.AngularVelocity = value;
    }

    public float LinearDamping {
        get => CurrentStuff.LinearDamping;
        set => CurrentStuff.LinearDamping = value;
    }

    public float AngularDamping {
        get => CurrentStuff.AngularDamping;
        set => CurrentStuff.AngularDamping = value;
    }

    public float GravityScale {
        get => CurrentStuff.GravityScale;
        set => CurrentStuff.GravityScale = value;
    }

    public float SleepThreshold {
        get => CurrentStuff.SleepThreshold;
        set => CurrentStuff.SleepThreshold = value;
    }

    public bool EnableSleep {
        get => CurrentStuff.EnableSleep;
        set => CurrentStuff.EnableSleep = value;
    }

    public bool IsBodyAwake {
        get => CurrentStuff.IsAwake;
        set => CurrentStuff.IsAwake = value;
    }

    public bool FixedRotation {
        get => CurrentStuff.FixedRotation;
        set => CurrentStuff.FixedRotation = value;
    }

    public bool IsBullet {
        get => CurrentStuff.IsBullet;
        set => CurrentStuff.IsBullet = value;
    }

    void OnEnable() {
        CurrentStuff.IsEnabled = true;
    }

    void OnDisable() {
        CurrentStuff.IsEnabled = false;
    }

    public bool AutomaticMass {
        get => CurrentStuff.AutomaticMass;
        set => CurrentStuff.AutomaticMass = value;
    }

    public Vector2 GetLocalPoint(Vector2 worldPoint)
        => _body.GetLocalPoint(worldPoint);

    public Vector2 GetWorldPoint(Vector2 localPoint) 
        => _body.GetWorldPoint(localPoint);

    public Vector2 GetLocalVector(Vector2 worldVector)
        => _body.GetLocalVector(worldVector);
    
    public Vector2 GetWorldVector(Vector2 localVector)
        => _body.GetWorldVector(localVector);
    
    public void ApplyForce(Vector2 force, Vector2 point, bool wake = true) =>
        _body.ApplyForce(force, point, wake);

    public void ApplyForce(Vector2 force, bool wake = true) =>
        _body.ApplyForce(force, wake);

    public void ApplyTorque(float torque, bool wake = true) =>
        _body.ApplyTorque(torque, wake);
    
    public void ApplyLinearImpulse(Vector2 impulse, Vector2 point, bool wake = true) =>
        _body.ApplyLinearImpulse(impulse, point, wake);
    
    public void ApplyLinearImpulse(Vector2 impulse, bool wake = true) =>
        _body.ApplyLinearImpulse(impulse, wake);
    
    public void ApplyAngularImpulse(float impulse, bool wake = true) =>
        _body.ApplyAngularImpulse(impulse, wake);
    
    public float Mass => _body.Mass;
    public float InertiaTensor => _body.InertiaTensor;
    public Vector2 LocalCenterOfMass => _body.LocalCenterOfMass;
    public Vector2 WorldCenterOfMass => _body.WorldCenterOfMass;
    
    public void ApplyMassFromShapes() =>
        _body.ApplyMassFromShapes();

    void Start() {
        _bodyDef.Position = Transform.Position.ToVector2();
        _bodyDef.UserData = this;
        _body = _world.CreateBody(_bodyDef);
        var colliders = GameObject.GetComponentsInChildren().Where(
            component => component.GetType().IsAssignableTo(typeof(Collider))
        ).Cast<Collider>();
        foreach (var collider in colliders) {
            var shape = collider.CreateShape(_body);
            shape.UserData = collider;
        }
    }

    void OnEnabled() {
        CurrentStuff.IsEnabled = true;
    }

    void OnDisabled() {
        CurrentStuff.IsEnabled = true;
    }

    void Update() {
        var position = Vector2.Zero;
        var rotation = 0f;
        _interpolateTime += Time.DeltaF;
        switch (InterpolationMode) {
            case PhysicsInterpolationMode.None:
                position = Position;
                rotation = Rotation.Angle;
                break;
            case PhysicsInterpolationMode.Interpolate:
                position = _interpolatedPosition;
                rotation = _interpolatedRotation;
                break;
            case PhysicsInterpolationMode.Extrapolate:
                position = _extrapolatedPosition;
                rotation = _extrapolatedRotation;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Transform.Position = new Vector3(position, Transform.Position.Z);
        var old = Transform.Rotation.GetEulerAngles(); 
        Transform.Rotation = Quaternion.CreateFromYawPitchRoll(old.X, old.Y, rotation);
    }

    public PhysicsInterpolationMode InterpolationMode = PhysicsInterpolationMode.None;
    private Vector2 _interpolatedPosition => _prevPosition+(Position-_prevPosition)*_interpolateFactor;
    private Vector2 _extrapolatedPosition => Position+(_prevPosition-Position)*_interpolateFactor;
    private Vector2 _prevPosition;
    private float _interpolatedRotation=> _prevRotation+(Rotation.Angle-_prevRotation)*_interpolateFactor;
    private float _extrapolatedRotation => Rotation.Angle+(_prevRotation-Rotation.Angle)*_interpolateFactor;
    private float _prevRotation;
    private float _interpolateFactor => _interpolateTime/Time.FixedDeltaF;
    private float _interpolateTime;
    void FixedUpdate() {
        _interpolateTime = 0;
        _prevPosition = Transform.Position.ToVector2();
        _prevRotation  = Transform.Rotation.GetEulerAngles().Z; 
    }

    public override void Dispose() {
        base.Dispose();
        _body.Dispose();
        _body = null;
    }
}