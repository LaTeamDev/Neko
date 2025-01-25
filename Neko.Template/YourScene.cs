using System.Drawing;
using NekoLib.Core;
using Neko;
using NekoLib.Extra;

namespace NekoRay.Template;

public class YourScene : BaseScene {
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        //var camera = gameObject.AddComponent<Camera2D>();
        //camera.BackgroundColor = Color.White;
        //camera.IsMain = true;

        var text = new GameObject("Text").AddComponent<YourBehaviour>();
        
        base.Initialize();
    }
}