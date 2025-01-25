using NekoLib.Core;
using NekoRay;
using NeuroSama.UI;

namespace NeuroSama.Gameplay.MainMenu;

public class PlayButtonAction : Behaviour {
    void OnButtonPressed(Button sender) { //TODO: click
        NekoLib.Extra.Console.Submit("newgame");
    }
}