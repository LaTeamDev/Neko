using NekoLib.Scenes;
using NekoRay;

namespace NeuroSama;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        SceneManager.LoadScene(new GameScene());
    }
}