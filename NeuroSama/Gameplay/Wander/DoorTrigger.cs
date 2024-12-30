using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.MiniGame;
using ZeroElectric.Vinculum;

namespace NeuroSama.Gameplay.Wander;

public class DoorTrigger : Behaviour {
    private Ease? _fadeEase;
    private Ease? _moveEase;
    
    public Text TextObject;
    public Vector3 TextPosition;
    
    public float AnimationTime = 0.3f;

    public bool InSensor = false;

    public IEasing Easing = new EaseOutQuad();

    void Awake() {
        TextObject = GameObject.AddChild("Text").AddComponent<Text>();
        TextObject.TextString = "Press [SPACE] to Interact";
        TextObject.Font = Data.GetFont("fonts/dialogue_font.ttf");
        TextObject.Color = Raylib.RAYWHITE.Fade(0f);
        TextObject.Origin = new Vector2(0.5f, 0.5f);
        TextPosition.Y -= 300f;
        TextObject.Transform.LocalPosition = TextPosition;
    }
    
    void OnSensorEnter2D(SensorEvents.BeginTouchEvent contact) {
        InSensor = true;
        _fadeEase?.Stop();
        _moveEase?.Stop();
        _fadeEase = Ease.To(() => TextObject.Color.a,
            value => TextObject.Color.a = (byte) value,
            AnimationTime,
            255).SetEasing(Easing);
        _moveEase = Ease.To(() => TextObject.Transform.LocalPosition.Y,
            value => TextObject.Transform.LocalPosition = TextObject.Transform.LocalPosition with {Y = value},
            AnimationTime,
            TextPosition.Y).SetEasing(Easing);
    }
    
    void OnSensorExit2D(SensorEvents.EndTouchEvent contact) {
        InSensor = false;
        _fadeEase?.Stop();
        _moveEase?.Stop();
        _fadeEase = Ease.To(() => TextObject.Color.a,
            value => TextObject.Color.a = (byte) value,
            AnimationTime,
            0).SetEasing(Easing);
        _moveEase = Ease.To(() => TextObject.Transform.LocalPosition.Y,
            value => TextObject.Transform.LocalPosition = TextObject.Transform.LocalPosition with {Y = value},
            AnimationTime,
            TextPosition.Y-32).SetEasing(Easing);
    }
    
    void Update() {
        if (DialogueOrchestrator.IsInDialogue) return;
        if (!InSensor) return;
        if (!Input.IsPressed("next")) return;
        DialogueController.AddStub();
        DialogueController.Add("Neuro", "huh?");
        DialogueController.ChangeScene(new SplashScene(new MiniGameScene(), "thegame"));
        //SceneManager.LoadScene(new SplashScene(new MiniGameScene(), "thegame"));
    }
}