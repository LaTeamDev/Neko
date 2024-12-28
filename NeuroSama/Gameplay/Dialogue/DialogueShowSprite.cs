using NekoRay;

namespace NeuroSama.Gameplay.Dialogue;

public class DialogueShowSprite(Sprite sprite, DialoguePosition position) {
    public Sprite Sprite = sprite;
    public DialoguePosition Position = position;
}

public enum DialoguePosition {
    Left,
    Center,
    Position
}