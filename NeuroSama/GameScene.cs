using Box2D;
using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using NeuroSama.Gameplay;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.UI;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Font = NekoRay.Font;

namespace NeuroSama;

public class GameScene : BaseScene {
    private World _world;
    public override void Initialize() {
        _world = this.CreateWorld();
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.BackgroundColor = new Color(252, 240, 196, 255);
        camera.Zoom = 2f;
        
        var player = new Player();

        var dg = gameObject.AddChild("Dialogue").AddComponent<DialogueOrchestrator>();


        base.Initialize();
    }

    public override void OnWindowResize() {
        base.OnWindowResize();
        ((Camera2D) (BaseCamera.Main)).Zoom = 1280 / BaseCamera.Main.RenderWidth;
    }
    
    
    public override void FixedUpdate() {
        base.FixedUpdate();
        _world.Step(Time.FixedDeltaF, 4);
    }
}