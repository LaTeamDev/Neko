using NekoRay;

namespace Touhou;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        if (DevMode) {
            NekoRay.Tools.Console.ExecFile("autoexec_dev");
            return;
        }
    }
}