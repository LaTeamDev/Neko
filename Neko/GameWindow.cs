using System.Diagnostics;
using Neko.Sdl.Video;

namespace Neko;

public class GameWindow : Window {
    public static GameWindow? Instance { get; private set; }
    public static GameBase Game { get; private set; }
    
    private GameWindow(int width, int height, string title) : 
        base(width, height, title, WindowSettings.Instance._flags) { }
    

    public static void Init(int width, int height, string title, GameBase game) {
        if (Instance is not null) return;
        var win = new GameWindow(width, height, title);
        Instance = win;
        Game = game;
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