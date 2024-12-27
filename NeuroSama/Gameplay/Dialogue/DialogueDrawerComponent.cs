using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueDrawerComponent : Behaviour {
    void Render() {
        if (!DialogueController.HasMessages) return;
        var xStart = -Camera2D.CurrentCamera.RenderWidth / 2 + (int)Transform.Position.X;
        var yStart = -128 + (int)Transform.Position.Y;
        Raylib.DrawRectangle(xStart+8, yStart, Camera2D.CurrentCamera.RenderWidth-16, 256, Raylib.WHITE);
        Raylib.DrawRectangle(xStart+16, yStart+8, Camera2D.CurrentCamera.RenderWidth-32, 256-16, Raylib.BLACK);
        Raylib.DrawText(DialogueController.Queue.Peek().Name, xStart+32, yStart+16, 16f, Raylib.WHITE);
        Raylib.DrawText(DialogueController.Queue.Peek().Text, xStart+32, yStart+48, 16f, Raylib.WHITE);
    }
}