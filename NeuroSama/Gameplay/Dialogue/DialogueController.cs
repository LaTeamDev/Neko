using NekoRay.Tools;

namespace NeuroSama.Gameplay.Dialogue;

public static class DialogueController {

    public static DialogueTracker DialogueObservable = new();

    [ConCommand("dg_msg")]
    public static void Add(string name, params string[] text) {
        Add(name, string.Join(' ', text));
    }
    
    public static void Add(string name, string text) {
        var entry = new DialogueEntry(name, text);
        DialogueObservable.Notify(entry);
    }
    
    [ConCommand("dg_next")]
    public static void Next() {
        DialogueObservable.Notify(new DialogueNext());
    }
    
    [ConCommand("dg_image")]
    public static void AddImage() {
        DialogueObservable.Notify(new DialogueNext());
    }
}