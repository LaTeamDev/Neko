using NekoLib.Core;
using NekoRay;

namespace Touhou.Gameplay;

public class TestScene : BaseScene {
    public override void Initialize() {
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.IsMain = true;
        new GameObject("reimu").AddComponent<SpriteRenderer2D>().Sprite = Sprite.Load("sprites/characters/reimu/idle.nrs");
        
        base.Initialize();
    }
}