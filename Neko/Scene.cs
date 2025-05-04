using System.Numerics;
using NekoLib.Extra;
using Neko.Physics2D;
using Serilog;

namespace Neko; 

public abstract class Scene : BaseScene {

    public virtual void DrawCameraTexture() {
        if (CliOptions.Instance.DevMode) return;
        if (BaseCamera.Main is null) return;
        if (this != BaseCamera.Main.GameObject.Scene) return;
        
        //var texture = BaseCamera.Main.RenderTexture.Texture;
        //var rect = new Rectangle(0, 0, texture.Width, -texture.Height);
        //var rectDest = new Rectangle(0, 0, texture.Width, texture.Height);
        //texture.Draw(rect, rectDest, Vector2.Zero, 0f, Raylib.WHITE);
    }

    public override void Draw() {
        base.Draw();
        DrawCameraTexture();
    }
}