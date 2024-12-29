using NekoLib.Scenes;
using NekoRay;
using NekoRay.Easings;
using NeuroSama.Gameplay.MainMenu;

namespace NeuroSama;

public class Game : GameBase {
    public override void Load(string[] args) {
        base.Load(args);
        if (DevMode) {
            SceneManager.LoadScene(new GameScene());
        }
        SceneManager.LoadScene(new SplashScene(new MenuScene()));
    }

    public override void Update() {
        base.Update();
        Ease.UpdateAll(Time.DeltaF);
    }
}