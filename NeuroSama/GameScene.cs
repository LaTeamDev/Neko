using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama;

public class GameScene : BaseScene {
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;

        base.Initialize();
    }
}