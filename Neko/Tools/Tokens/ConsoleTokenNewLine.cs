using ImGuiNET;

namespace Neko.Tools; 

public class ConsoleTokenNewLine : IConsoleToken {
    public string Text => "\n";
    
    public virtual void Render() {
        ImGui.Text(Text);
    }
}