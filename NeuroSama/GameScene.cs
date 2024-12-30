using System.Numerics;
using NekoLib.Core;
using NekoLib.Filesystem;
using NekoRay;
using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.MiniGame;
using NeuroSama.UI;
using SoLoud;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Font = NekoRay.Font;

namespace NeuroSama;

public class GameScene : BaseScene {
    private Voice _voice;
    public override void Initialize() {
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.BackgroundColor = Raylib.WHITE;

        var dg = gameObject.AddChild("Dialogue").AddComponent<DialogueOrchestrator>();
        
        using var stream = Files.GetFile("sounds/music/neuro3.mp3").GetStream();
        var musicStream = WavStream.LoadFromStream(stream);
        _voice = Audio.SoLoud.Play(musicStream);
        _voice.Loop = true;
        
        base.Initialize();
    }

    public override void Dispose() {
        base.Dispose();
        _voice.Stop();
    }

    public override void Update() {
        base.Update();
        if (Input.IsPressed("forward")) {
            DialogueController.Add("Stream", "is on");
            DialogueController.AddImage("neuro_vtube", "neutral_vedal", DialoguePosition.Center);
            DialogueController.Add("Neuro", "Vedal.");
            DialogueController.Add("Vedal", "Huh?");
            DialogueController.Add("Neuro", "Did i ever had...");
            DialogueController.Add("Neuro", "... a childhood?");
            DialogueController.RemoveImage(DialoguePosition.Center);
            DialogueController.AddImage("neuro", "neutral", DialoguePosition.Left);
            DialogueController.Add("Neuro", "Vedal.");
            DialogueController.AddImage("vedal", "neutral", DialoguePosition.Right);
            DialogueController.Add("Vedal", "yes?");
            DialogueController.Add("Neuro", "If you say i can't have a childhood because i am an AI");
            DialogueController.Add("Neuro", "What could it be?");
            DialogueController.Add("Vedal", "Well..");
            DialogueController.Add("Vedal", "Since you are my creation...");
            DialogueController.Add("Neuro", "You would be definetly a small turtle");
            DialogueController.Add("Neuro", "With a childhood typical for small tuetles");
            DialogueController.Add("Neuro", "This does indeed sound logical.");
            
        }
    }

    public override void OnWindowResize() {
        base.OnWindowResize();
        ((Camera2D) (BaseCamera.Main)).Zoom = 1280 / BaseCamera.Main.RenderWidth;
    }
}