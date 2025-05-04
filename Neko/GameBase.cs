using ImGuiNET;
using Neko.Tools;
using NekoLib.Extra;
using NekoLib.Filesystem;
using Neko.Sdl;
using Neko.Sdl.ImGuiBackend;
using NekoLib.Tools;
using Serilog;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using Console = NekoLib.Extra.Console;

namespace Neko; 

public abstract class GameBase {
    private static GameBase? _instance;
    public static GameBase Instance {
        get {
            ArgumentNullException.ThrowIfNull(_instance, nameof(_instance));
            return _instance;
        }
    }

    public GameBase() {
        _instance = this;
    }

    public static ILogger Log;
    public unsafe virtual void Initlogging() {
        //logger.LogInformation("Hello World! Logging is {Description}.", "fun");
        var loggingCfg = ConfigureLogger(new LoggerConfiguration());
        Serilog.Log.Logger = loggingCfg
            .CreateLogger()
            .ForContext("Name", "Neko");
        Log = Serilog.Log.Logger.ForContext("Name", GetType().Name);

        //Raylib.SetTraceLogCallback(&RaylibCallback);
    }

    public virtual LoggerConfiguration ConfigureLogger(LoggerConfiguration configuration) {
        const string outputTemplate = "{Timestamp:HH:mm:ss} [{Level}] {Name}: {Message}{Exception}{NewLine}";
        return configuration
#if DEBUG
            .MinimumLevel.Verbose()
#else
            .MinimumLevel.Information()
#endif
            .Enrich.FromLogContext()
            .WriteTo.GameConsole()
            .WriteTo.Console(LogEventLevel.Verbose, outputTemplate)
            .WriteTo.File($"logs/nekoray{DateTime.Now:yy.MM.dd-hh.MM.ss}.log", LogEventLevel.Verbose, outputTemplate);
    }

    [ConVariable("devmode_enabled")] 
    public static bool DevMode => CliOptions.Instance.DevMode;
    
    public ConsoleWindow ConsoleWindow;

    public virtual void Load(string[] args) {
        new AssemblyFilesystem(GetType().Assembly, GetType().Namespace).Mount();
        Console.ExecFile("autoexec");
        InitConsoleWindow(CliOptions.Instance.ConsoleOnStart);
    }

    public virtual void InitConsoleWindow(bool enable) {
        SceneManager.LoadScene(new PersistantScene());
        ConsoleWindow = ConsoleWindow.ToggleConsole();
        ConsoleWindow.Enabled = false;
        ConsoleWindow.Enabled |= DevMode;
        ConsoleWindow.Enabled |= enable;
        
        if (!(DevMode || enable)) return;
        //Input.BindCommand(KeyboardKey.KEY_F5, "toggleconsole");
    }
    
    public virtual void Update() {
        SceneManager.Update();
    }

    public virtual void Draw() {
        SceneManager.Draw();
        if (Time.DrawFps) throw new NotImplementedException();
    }

    public virtual void FixedUpdate() {
        SceneManager.InvokeScene("FixedUpdate");
    }

    public delegate void LoopFunction();

    public unsafe virtual void SetupImGuiFonts(ImGuiIOPtr io) {
        var cfg = new ImFontConfigPtr(ImGuiNative.ImFontConfig_ImFontConfig());
        cfg.OversampleH = 1;
        cfg.OversampleV = 1;
        io.ConfigWindowsMoveFromTitleBarOnly = true;
        var firaFont = io.Fonts.AddFontFromFilesystemTTF("fonts/Lpix.ttf", 7, cfg);
    }
    public virtual LoopFunction Run(string[] args) {
        Audio.Init();
        //rlImGui.SetupUserFonts = SetupImGuiFonts;
        //rlImGui.Setup(true, true);
        ImGui.CreateContext();
        var io = ImGui.GetIO();
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard;     // Enable Keyboard Controls
        io.ConfigFlags |= ImGuiConfigFlags.NavEnableGamepad;      // Enable Gamepad Controls
        //io.ConfigFlags |= ImGuiConfigFlags.DockingEnable;         // Enable Docking

        // Setup Dear ImGui style
        ImGui.StyleColorsDark();
        //ImGui::StyleColorsLight();

        // Setup Platform/Renderer backends
        var r = GameWindow.Instance.Renderer;
        ImGuiSdl.InitForSDLRenderer(GameWindow.Instance, r);
        ImGuiSdlRenderer.Init(r);
        
        // Our state
        bool show_demo_window = true;
        bool show_another_window = false;
        //ImVec4 clear_color = ImVec4(0.45f, 0.55f, 0.60f, 1.00f);
        Load(args);
        return () => {
            Time.Step();
            NekoLib.Core.Timer.Global.Update(Time.DeltaF);
            Input.Update();
            Update();
            while (Time._fixedTime > Time.FixedDeltaF) {
                Time.IsInFixed = true;
                FixedUpdate();
                Time.IsInFixed = false;
                Time._fixedTime -= Time.FixedDeltaF;
            }
            //Raylib.BeginDrawing();
            r.DrawColor = new Color(0xFF000000);
            r.Clear();
            Draw();
            ImGuiSdlRenderer.NewFrame();
            ImGuiSdl.NewFrame();
            ImGui.NewFrame();
            ImGui.ShowDemoWindow();
            if (DevMode) ImGui.DockSpaceOverViewport(0, ImGui.GetMainViewport());
            DrawGui();
            ImGui.Render();
            ImGuiSdlRenderer.RenderDrawData(ImGui.GetDrawData(), r);
            r.Present();
        };
    }

    public virtual void Shutdown() {
        //rlImGui.Shutdown();
        Audio.Close();
    }

    public virtual void DrawGui() {
        SceneManager.InvokeScene("DrawGui");
    }

    public virtual LoopFunction ErrorHandler(Exception msg) {
        var error = msg.ToString();
        Log.Fatal(msg, "An error occured");
        var r = GameWindow.Instance.Renderer;
        void Draw() {
            r.DrawColor = new Color(0, 0, 0, 0);
            r.Clear();
            r.DrawColor = new Color(255, 255, 255, 255);
            var offset = 0f;
            foreach (var line in msg.ToString().Split("\n")) {
                r.DebugText(line, 0f, offset+=12f);
            }
            //Raylib.ClearBackground(Raylib.RAYWHITE);
            //Raylib.DrawText(error, 70, 70, 10, Raylib.GRAY);
            r.Present();
        }

        return () => {
            Time.Step();
            //Raylib.BeginDrawing();
            Draw();
            //Raylib.EndDrawing();
            Thread.Sleep(100);
        };
    }
}