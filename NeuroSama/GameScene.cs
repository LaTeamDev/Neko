using System.Numerics;
using NekoLib.Core;
using NekoRay;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.UI;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Font = NekoRay.Font;

namespace NeuroSama;

public class GameScene : BaseScene {
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;

        var dg = gameObject.AddChild("Dialogue").AddComponent<DialogueOrchestrator>();


        base.Initialize();
    }

    public override void OnWindowResize() {
        base.OnWindowResize();
        ((Camera2D) (BaseCamera.Main)).Zoom = 1280 / BaseCamera.Main.RenderWidth;
    }
}