using ImGuiNET;

namespace Neko.Tools; 

public class ImguiDemoWindow : ToolBehaviour {
    void DrawGui() {
        ImGui.ShowDemoWindow();
    }

    [ConCommand("imgui_demo_toggle")]
    public static void ToggleImGuiDemoWindow() => ToolsShared.ToggleTool<ImguiDemoWindow>();
}