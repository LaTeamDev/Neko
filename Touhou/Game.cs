using NekoLib.Scenes;
using NekoRay;
using Touhou.Gameplay;

namespace Touhou;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        PlayerInfo.PreloadAll();
        if (DevMode) {
            NekoLib.Extra.Console.ExecFile("autoexec_dev");
            SceneManager.LoadScene(new TestScene());
            return;
        }
    }
}