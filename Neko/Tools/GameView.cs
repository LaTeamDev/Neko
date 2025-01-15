using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using JetBrains.Annotations;

namespace Neko.Tools; 

public class GameView : ToolBehaviour {
    private static int _renderWidth;
    private static int _renderHeight;
    private static bool _autoSize = true;

    [ConVariable("r_gameview_width")]
    public static int DisplayWidth {
        get => _renderWidth;
        set {
            _autoSize = false;
            _renderWidth = value;
        }
    }

    [ConVariable("r_gameview_height")]
    public static int DisplayHeight {
        get => _renderHeight;
        set {
            _autoSize = false;
            _renderHeight = value;
        }
    }

    private bool DrawSize(string id, ref int width, ref int height) {
        ImGui.SetNextItemWidth(64f);
        if (ImGui.InputInt($"##{id}-width", ref width, 0)) {
            return true;
        }
        ImGui.SameLine();
        ImGui.Text("x");
        ImGui.SameLine();
        ImGui.SetNextItemWidth(64f);
        if (ImGui.InputInt($"##{id}-height", ref height, 0)) {
            return true;
        }

        return false;
    }

    private Vector2 _prevWinSize = Vector2.Zero;
    [UsedImplicitly]
    void DrawGui() {
        //TODO: make it use different fill methods
        _renderWidth = BaseCamera.Main?.RenderWidth ?? DisplayWidth;
        _renderHeight = BaseCamera.Main?.RenderHeight ?? DisplayHeight;
        if (ImGui.Begin("Game View", ImGuiWindowFlags.MenuBar | ImGuiWindowFlags.HorizontalScrollbar)) {
            var size = ImGui.GetWindowContentRegionMax()-ImGui.GetWindowContentRegionMin();
            if (ImGui.BeginMenuBar()) {
                if (ImGui.BeginMenu("Display Settings")) {
                    ImGui.SeparatorText("Render Size");
                    //ImGui.InputInt2("##render-size", ref lol.a);
                    if (DrawSize("render", ref _renderWidth, ref _renderHeight)) {
                        BaseCamera.Main.RenderWidth = _renderWidth;
                        BaseCamera.Main.RenderHeight = _renderHeight;
                        BaseCamera.Main.RecreateRenderTexture();
                        _autoSize = false;
                    }

                    ImGui.Checkbox("Auto-size", ref _autoSize);
                    
                    ImGui.EndMenu();
                }
                ImGui.Text("FPS:"+Time.Fps);
                ImGui.EndMenuBar();
            }
            ImGui.BeginChild("GameRenderer");
            //var aspectRatio = _renderWidth/_renderHeight;
            var wsize = new Vector2(_renderWidth, _renderHeight);
            var mousePosScalingFactor = _renderWidth / wsize.X;
            var startPos = ImGui.GetCursorScreenPos();
            if (BaseCamera.Main is not null) {
                if (_autoSize) {
                    if ((int)size.X != _renderWidth || (int)size.Y != _renderHeight) {
                        BaseCamera.Main.RenderWidth = (int)size.X;
                        BaseCamera.Main.RenderHeight = (int)size.Y;
                        BaseCamera.Main.RecreateRenderTexture();
                    }
                }
                //GraphicsReferences.OpenGl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                //ImGui.Image((nint) Camera.MainCamera.RenderTexture.OpenGlHandle, wsize with {Y = wsize.X/aspectRatio}, new(0, 1), new(1, 0));
                //if (w > wsize.X) wsize
                ImGui.SetCursorPos((size - wsize)/2);
                ImGui.Image((nint)BaseCamera.Main.RenderTexture.Texture._texture.id,wsize, new(0, 1), new(1, 0));
            }
            //else
            //ImGui.Image((nint)Texture.Missing.OpenGlHandle, wsize, new(0, 1), new(1, 0));
            if (ImGui.IsItemHovered(ImGuiHoveredFlags.DelayNone)) {
                Input.ForceUpdate = true;
                Input._lastMousePos = (ImGui.GetMousePos() - startPos) * mousePosScalingFactor;
            }
            else {
                Input.ForceUpdate = false;
            }
            ImGui.EndChild();
        }
        ImGui.End();
    }

    void OnDisable() {
        if (BaseCamera.Main is null) return;
        BaseCamera.Main.RenderWidth = -1;
        BaseCamera.Main.RenderHeight = -1;
    }

    [ConCommand("game_view")]
    public static void OpenGameView() => ToolsShared.ToggleTool<GameView>();
}