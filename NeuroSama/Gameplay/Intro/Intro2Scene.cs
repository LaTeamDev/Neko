using NeuroSama.Gameplay.Dialogue;
using NeuroSama.Gameplay.MiniGame;

namespace NeuroSama.Gameplay.Intro;

public class Intro2Scene : Intro1Scene {
    public override void ShowDialogue() {
        DialogueController.Add("THIS IS WORKAROUND", "FIXME");
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
        DialogueController.ChangeScene(new SplashScene(new MiniGameScene(), "thegame"));
    }
}