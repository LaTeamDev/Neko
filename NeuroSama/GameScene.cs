using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using NeuroSama.CodeData;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama;

public class GameScene : BaseScene {
    private World _world;
    public override void Initialize() {
        _world = this.CreateWorld();
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.BackgroundColor = new Color(203, 219, 252, 255);
        camera.Zoom = 2f;
        
        var player = new Player();

        base.Initialize();
    }
    
    // public override void Update()
    // {
    // }
    
    // public override void FixedUpdate() {
    //     base.FixedUpdate();
    //     _world.Step(Time.FixedDeltaF, 4);
    // }
}