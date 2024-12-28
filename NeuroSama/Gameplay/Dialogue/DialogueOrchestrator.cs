using System.Numerics;
using GTweens.Extensions;
using NekoLib.Core;
using NekoRay;
using NekoRay.Easings;
using NeuroSama.UI;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueOrchestrator : Behaviour, IObserver<DialogueEvent> {
    public Rect TopBox;
    public Rect BottomBox;
    public Rect Fader;
    public Text CurrentText;
    public Text PrevText;
    public Text NextText;
    
    public Queue<DialogueEvent> Queue = new();
    private IDisposable _trackerHandle;

    void Awake() {
        var fader = GameObject.AddChild("Fader").AddComponent<Rect>();
        var topBox = GameObject.AddChild("Top Box").AddComponent<Rect>();
        var bottomBox = GameObject.AddChild("Bottom Box").AddComponent<Rect>();
        Transform.LocalPosition = new Vector3(-640, -360, 0);
        fader.Width = 1280;
        fader.Height = 720;
        fader.Color = Raylib.BLACK.Fade(0.25f);
        topBox.Width = 1280;
        topBox.Height = 128;
        topBox.Color = Raylib.BLACK;
        bottomBox.Width = 1280;
        bottomBox.Height = 128;
        bottomBox.Color = Raylib.BLACK;
        bottomBox.Origin = new Vector2(0, 1f);
        bottomBox.Transform.LocalPosition = new Vector3(0, 720, 0);
        var text = bottomBox.GameObject.AddChild("Text").AddComponent<Text>();
        var font = NekoRay.Font.Load("fonts/dialogue_font.ttf", 16, 255);
        text.Font = font;
        text.TextString = "Neuro: 1234 pls help";
        text.Origin = new Vector2(0.5f, 0.5f);
        text.Transform.LocalPosition = new Vector3(640, -64, 0);
        text.Transform.LocalScale = new Vector3(2f);
        
        TopBox = topBox;
        BottomBox = bottomBox;
        Fader = fader;
        CurrentText = text;
        
        NextText = bottomBox.GameObject.AddChild("Next Text").AddComponent<Text>();
        NextText.Font = font;
        NextText.TextString = "";
        NextText.Origin = new Vector2(0.5f, 0.5f);
        NextText.Transform.LocalPosition = new Vector3(640, -32, 0);
        NextText.Transform.LocalScale = new Vector3(2f);
        NextText.Color = Raylib.WHITE.Fade(0f);
        
        PrevText = bottomBox.GameObject.AddChild("Previous Text").AddComponent<Text>();
        PrevText.Font = font;
        PrevText.TextString = "";
        PrevText.Origin = new Vector2(0.5f, 0.5f);
        PrevText.Transform.LocalPosition = new Vector3(640, -96, 0);
        PrevText.Color = Raylib.WHITE.Fade(0f);

        _trackerHandle = DialogueController.DialogueObservable.Subscribe(this);
    }

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(DialogueEvent value) {
        if (value.GetType() != typeof(DialogueNext))
            Queue.Enqueue(value);
        else {
            PrevEvent = Queue.Dequeue();
            Next();
        }
    }

    private DialogueEvent? PrevEvent;

    private DialogueEvent? NextEvent {
        get {
            Queue.TryPeek(out var val);
            return val;
        }
    }

    public override void Dispose() {
        base.Dispose();
        _trackerHandle.Dispose();
    }

    void Update() {
        switch (Queue.Count) {
            case > 0 when !_shown:
                _shown = true;
                _hidden = false;
                Show();
                break;
            case <= 0 when !_hidden:
                _shown = false;
                _hidden = true;
                Hide();
                break;
        }
    }

    private bool _shown;
    private bool _hidden;

    public IEasing Easing = new EaseInOutCubic();
    public float Time = 0.3f;
    public void Show() {
        if (NextEvent is DialogueEntry dialog) {
            CurrentText.TextString = $"{dialog.Name}:{dialog.Text}";
        }
        Ease.To(() => TopBox.Transform.LocalPosition.Y, 
            value => TopBox.Transform.LocalPosition = TopBox.Transform.LocalPosition with {Y =value}, 
            Time,
            0).SetEasing(Easing);
        Ease.To(() => BottomBox.Transform.LocalPosition.Y, 
            value => BottomBox.Transform.LocalPosition = BottomBox.Transform.LocalPosition with {Y = value}, 
            Time,
            720).SetEasing(Easing);
        Ease.To(() => CurrentText.Transform.LocalPosition.Y, 
            value => CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = value}, 
            Time,
            -64).SetEasing(Easing);
        Ease.To(() => (float)Fader.Color.a,
            value => Fader.Color = Fader.Color with {a = (byte)value}, 
            Time,
            69).SetEasing(Easing);
        Ease.To(() => (float)CurrentText.Color.a,
            value => CurrentText.Color = CurrentText.Color with {a = (byte)value}, 
            Time,
            255).SetEasing(Easing);
    }

    public void Next() {
        if (Queue.Count <= 0) return;
        if (NextEvent is not DialogueEntry dialog) return;
        NextText.TextString = $"{dialog.Name}:{dialog.Text}";
        Ease.To(() => CurrentText.Transform.LocalPosition.Y, 
            value => CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = value}, 
            Time,
            -96).SetEasing(Easing).After(
            ()=>CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = -64}
        );;
        Ease.To(() => (float)CurrentText.Color.a,
            value => CurrentText.Color = CurrentText.Color with {a = (byte)value}, 
            Time,
            96).SetEasing(Easing).After(
            ()=>CurrentText.Color = CurrentText.Color with {a = 255}
        );
        Ease.To(() => CurrentText.Transform.LocalScale.X, 
            value => CurrentText.Transform.LocalScale = new(value), 
            Time,
            1).SetEasing(Easing).After(
            ()=>CurrentText.Transform.LocalScale = new(2)
        );
        Ease.To(() => NextText.Transform.LocalPosition.Y, 
            value => NextText.Transform.LocalPosition = NextText.Transform.LocalPosition with {Y = value}, 
            Time,
            -64).SetEasing(Easing).After(
            ()=>NextText.Transform.LocalPosition = NextText.Transform.LocalPosition with {Y = -32}
        );
        Ease.To(() => (float)PrevText.Color.a,
            value => PrevText.Color = PrevText.Color with {a = (byte)value}, 
            Time,
            0).SetEasing(Easing).After(
            ()=> {
                PrevText.Color = PrevText.Color with {a = 96};
            });
        
        Ease.To(() => (float)NextText.Color.a,
            value => NextText.Color = NextText.Color with {a = (byte)value}, 
            Time,
            255).SetEasing(Easing).After(
            ()=> {
                NextText.Color = NextText.Color with {a = 0};
                PrevText.TextString = CurrentText.TextString;
                CurrentText.TextString = NextText.TextString;
            });
        
    }
    
    public void Hide() {
        NextText.TextString = "";
        Ease.To(() => TopBox.Transform.LocalPosition.Y, 
            value => TopBox.Transform.LocalPosition = TopBox.Transform.LocalPosition with {Y =value}, 
            Time,
            -128).SetEasing(Easing);
        Ease.To(() => BottomBox.Transform.LocalPosition.Y, 
            value => BottomBox.Transform.LocalPosition = BottomBox.Transform.LocalPosition with {Y = value}, 
            Time,
            848).SetEasing(Easing);
        Ease.To(() => CurrentText.Transform.LocalPosition.Y, 
            value => CurrentText.Transform.LocalPosition = CurrentText.Transform.LocalPosition with {Y = value}, 
            Time,
            -192).SetEasing(Easing);
        Ease.To(() => (float)Fader.Color.a,
            value => Fader.Color = Fader.Color with {a = (byte)value}, 
            Time,
            0).SetEasing(Easing);
        Ease.To(() => (float)CurrentText.Color.a,
            value => CurrentText.Color = CurrentText.Color with {a = (byte)value}, 
            Time,
            0).SetEasing(Easing);
    }
}