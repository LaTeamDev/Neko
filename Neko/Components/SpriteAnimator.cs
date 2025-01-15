namespace Neko;

public class SpriteAnimator : Behaviour {
    public string AnimationName;
    public Dictionary<string, SpriteAnimation> Animations = new();
    public float AnimationSpeed = 1f;
    public int CurrentAnimationIndex = 0;
    public SpriteRenderer2D SpriteRenderer;
    private float _animationTime;
    public SpriteAnimation? CurrentAnimation {
        get {
            Animations.TryGetValue(AnimationName, out var value);
                return value;
        }
    }

    public SpriteAnimation.Frame? CurrentFrame => CurrentAnimation?.Frames[CurrentAnimationIndex];

    // public Rectangle? FrameRectangle {
    //     get {
    //         if (CurrentFrame is null) return null;
    //         return new Rectangle(CurrentFrame.X, CurrentFrame.Y, CurrentFrame.Width, CurrentFrame.Height);
    //     }
    // }
    
    void UpdateAnimation() {
        if (CurrentAnimation is null) return;
        if (CurrentAnimationIndex >= CurrentAnimation.Frames.Count) CurrentAnimationIndex = 0;
        if (AnimationSpeed <= 0f) return;
        _animationTime += Time.DeltaF;
        while (_animationTime >= AnimationSpeed) {
            _animationTime -= AnimationSpeed;
            CurrentAnimationIndex++;
            if (CurrentAnimationIndex >= CurrentAnimation.Frames.Count) CurrentAnimationIndex = 0;
        }
        SpriteRenderer.Sprite = CurrentFrame?.Sprite??SpriteRenderer.Sprite;
    }

    void Update() {
        UpdateAnimation();
    }

    public void AddAnimation(SpriteAnimation animation) {
        Animations.Add(animation.Name, animation);
    }

    public void AddAnimations(IEnumerable<SpriteAnimation> animations) {
        foreach (var animation in animations) {
            AddAnimation(animation);
        }
    }
}