using NekoRay.Tools;

namespace NeuroSama.Gameplay.Dialogue;

public static class DialogueController {
    public static Queue<DialogueEntry> Queue = new();
    public static bool HasMessages => Queue.Count != 0;

    [ConCommand("dg_add")]
    public static void Add(string name, string text) {
        Queue.Enqueue(new DialogueEntry(name, text));
    }
    
    [ConCommand("dg_next")]
    public static DialogueEntry Next() {
        return Queue.Dequeue();
    }
}