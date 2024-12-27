using NekoLib.Core;
using NekoRay;
using NeuroSama.Gameplay.Dialogue;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama;

public class GameScene : BaseScene {
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;

        gameObject.AddChild("Dialogue Box").AddComponent<DialogueDrawerComponent>();

        base.Initialize();
    }
}