using System.Diagnostics;
using Neko.Sdl.ImGuiBackend;
using Neko.Sdl.Video;
using SDL;
using Serilog;

namespace Neko;

public class GameWindow : Window {
    public static GameWindow? Instance { get; private set; }
    public static GameBase Game { get; private set; }
    
    private GameWindow(int width, int height, string title) : 
        base(width, height, title, WindowSettings.Instance._flags) { }
    

    public static void Init(int width, int height, string title, GameBase game) {
        if (Instance is not null) return;
        Gl.Attributes[GlAttr.ContextProfileMask] = (int)SDL_GLProfile.SDL_GL_CONTEXT_PROFILE_CORE;
        Gl.Attributes[GlAttr.ContextMajorVersion] = 4;
        Gl.Attributes[GlAttr.ContextMinorVersion] = 0;
        var win = new GameWindow(width, height, title);
        Instance = win;
        Game = game;
        win.CreateRenderer();
    }

    protected override unsafe void HandleEvent(SDL_Event e) {
        ImGuiSdl.ProcessEvent(&e);
        
        base.HandleEvent(e);
    }

    public void Run(string[] args) {
        try {
            var loopFunction = Game.Run(args);
            while (!ShouldQuit) {
                PollEvents();
                loopFunction();
            }
        }
        catch (Exception e) when (!Debugger.IsAttached) {
            var loopFunction = Game.ErrorHandler(e);
            while (!ShouldQuit) {
                PollEvents();
                loopFunction();
            }
        }
        finally {
            Game.Shutdown();
        }
    }
}