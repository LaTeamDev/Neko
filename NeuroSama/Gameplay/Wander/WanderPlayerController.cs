using System.Numerics;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;
using NeuroSama.Gameplay;
using NeuroSama.Gameplay.Dialogue;
using ZeroElectric.Vinculum;

namespace NeuroSama.Gameplay.Wander;

public class WanderPlayerController : Behaviour
{
    public Rigidbody2D rb;
    private List<Sprite> WalkSprites = new();
    private SpriteRenderer2D Renderer2D;
    private int _frame;
    private float _frametime;
    [Range(0.01f, 1f)]
    public float AnimationSpeed = 0.1f;
    private Sprite StandSprite;

    void Awake() {
        Renderer2D = GameObject.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        foreach (var file in Files.ListFiles("textures/characters/neuro")) {
            if (Path.GetFileName(file).StartsWith("walk"))
                WalkSprites.Add(Data.GetSprite(file));
        }

        StandSprite = Data.GetSprite("textures/characters/neuro/stand.png");
        Renderer2D.Sprite = WalkSprites[_frame];
        Renderer2D.ProportionallyScaleByWidth(486);
        Renderer2D.Transform.LocalPosition = new Vector3(0, -44f, 0);
        Renderer2D.Color = new Color(200, 200, 200, 255);
    }
    
    [Range(0.01f, 0.5f)] public float DampingSize = 0.05f;
    
    [ShowInInspector] private float _inputDirection;
    [ShowInInspector] private float _normalizedInput;
    private float _dampingVelocity;
    
    void UpdateInputDirection() {
        _inputDirection = (Input.IsDown("right") ? 1f : 0f) +(Input.IsDown("left") ? -1f : 0f);

        _normalizedInput = NekoMath.Damp(_normalizedInput, _inputDirection, ref _dampingVelocity, DampingSize);
    }

    public void UpdateAnimation() {
        if (_normalizedInput > 0f) {
            Renderer2D.FlipX = false;
        }
        else {
            Renderer2D.FlipX = true;
        }
        if (MathF.Abs(_normalizedInput) <= 0.01f) {
            Renderer2D.Sprite = StandSprite;
            return;
        };
        if (WalkSprites.Count < 1) return;
        _frametime += Time.DeltaF;
        while (_frametime > AnimationSpeed) {
            _frametime -= AnimationSpeed;
            _frame++;
            if (_frame >= WalkSprites.Count) _frame = 0;
        }

        Renderer2D.Sprite = WalkSprites[_frame];
    }

    public void Update() {
        if (DialogueOrchestrator.IsInDialogue) return;
        UpdateInputDirection();
        UpdateAnimation();
        rb.LinearVelocity = new Vector2(_normalizedInput * Speed, rb.LinearVelocity.Y);
    }

    public float Speed = 200f;
}