using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;

namespace NeuroSama;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        SceneManager.LoadScene(new GameScene());
    }

    public override void Update() {
        base.Update();
        Ease.UpdateAll(Time.DeltaF);
    }
}