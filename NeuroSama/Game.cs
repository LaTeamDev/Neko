using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;
using NeuroSama.Gameplay.MainMenu;

namespace NeuroSama;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        SceneManager.LoadScene(new MenuScene());
    }

    public override void Update() {
        base.Update();
        Ease.UpdateAll(Time.DeltaF);
    }
}