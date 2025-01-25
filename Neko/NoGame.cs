using Neko.Sdl;
using Neko.Sdl.Input;
using SDL;

namespace Neko; 

public class NoGame : GameBase {
    
    public override void Draw() {
        var r = GameWindow.Instance.Renderer;
        r.DrawColor = new Color(255, 255, 255, 255);
        r.DebugText(Mouse.State.Position.ToString());
    }
}