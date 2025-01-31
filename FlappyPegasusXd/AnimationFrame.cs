using System.Numerics;
using NekoRay;

namespace FlappyPegasus; 

public class AnimationFrame {
    public Sprite Sprite;
    public float Time;

    
    public AnimationFrame(Sprite sprite, float time = 0.1f) {
        if (time <= 0f) throw new ArgumentOutOfRangeException(nameof(time));
        Sprite = sprite;
        sprite.Origin = new Vector2(sprite.Width/2, sprite.Height/2);
        Time = time;
    }
}