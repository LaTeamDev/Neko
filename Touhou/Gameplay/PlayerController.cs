using System.Numerics;
using NekoLib.Core;
using NekoRay;

namespace Touhou.Gameplay;

public class PlayerController : Behaviour {
    public float BaseSpeed;
    public float FocusedSpeed;

    public bool IsFocused;
    private Vector2 _inputDirection;
    private Vector2 _normalizedInput;

    public float CurrentSpeed => IsFocused ? BaseSpeed : FocusedSpeed;
    
    void Update() {
        if (Input.IsDown("focus")) OnFocused();
        else OnNormal();
        Move();
    }

    void OnFocused() {
        IsFocused = true;
    }

    void OnNormal() {
        IsFocused = false;
    }
    
    void UpdateInputDirection() {
        _inputDirection = new Vector2(
            (Input.IsDown("right") ? 1f : 0f) + (Input.IsDown("left") ? -1f : 0f),
            (Input.IsDown("down") ? 1f : 0f) + (Input.IsDown("up") ? -1f : 0f)
        );
        _normalizedInput = Vector2.Normalize(_inputDirection);
        if (!float.IsNaN(_normalizedInput.X)) return;
        _normalizedInput.Y = 0f;
        _normalizedInput.X = 0f;
    }
    void Move() {
        UpdateInputDirection();
        Transform.Position += new Vector3(_normalizedInput*CurrentSpeed*Time.DeltaF, Transform.Position.Z);
    }
    
    void Awake() {
        
    }
}