using System.Diagnostics;
using System.Reflection;
using CommandLine;
using Neko.Sdl;
using NekoLib.Filesystem;
using Serilog;
using Tomlyn;

namespace Neko;

public static class Bootstrapper {
    private static NekoRayConf ReadConf(string id) {
        Stream fileStream;
        var path = $"{id}/conf.toml";
        if (File.Exists(path))
            fileStream = new FileStream($"{id}/conf.toml", FileMode.Open, FileAccess.Read);
        else
            throw new FileNotFoundException($"{id}/conf.toml");
        var streamReader = new StreamReader(fileStream);
        return Toml.ToModel<NekoRayConf>(streamReader.ReadToEnd());
    }

    private static GameBase GetGame(List<string> searchPath, string gameId) {
        GameBase gameBase;
        foreach (var path in searchPath) {
            var dllPath = Path.Combine(path.Replace("{{this}}", gameId), "client.dll");
            if (!File.Exists(dllPath)) continue; 
            var gameDll = Assembly.LoadFrom(dllPath);
            var gameType = gameDll.GetTypes().FirstOrDefault(type => typeof(GameBase).IsAssignableFrom(type));
            if (gameType is null)
                continue;
            return (GameBase)Activator.CreateInstance(gameType);
        }
        return new NoGame();
    }

    public static void Mount(string path) {
        if (File.Exists(path)) {
            //TODO: add support for archive by checking its extension
            Log.Warning("File mounting does not supported yet! skipping mounting...", path);
            return;
        }
        if (Directory.Exists(path)) {
            new FolderFilesystem(path).Mount();
            Log.Debug("Successfully mounted {Path} as Folder", path);
            return;
        }
        Log.Warning("Path {Path}, does not exist! skipping mounting...", path);
    }

    private static void MountPaths(NekoRayConf conf, string gameId) {
        foreach (var rawPath in conf.Filesystem.Bin) {
            var path = rawPath.Replace("{{this}}",gameId);
            Program.AddPath(path);
        }
        foreach (var rawPath in conf.Filesystem.Mount) {
            var path = rawPath.Replace("{{this}}",gameId); //does toml have metadata like yaml we can use?
            if (!path.EndsWith("*")) {
                Mount(path);
                continue;
            }
            if (!Directory.Exists(path.Replace("*", ""))) {
                Log.Warning("Path {Path}, defined in conf does not exist! skipping...", path);
                continue;
            }
            var allMountPoints = Directory.EnumerateDirectories(path).ToArray();
            for (var j = allMountPoints.Length - 1; j >= 0; j--) {
                Mount(path);
            }
        }
    }
    
    [DebuggerHidden]
    public static int Start(string[] args) {
        NekoSDL.Init(InitFlags.Video);
        ThreadSafety.MarkThisThreadMain();
        new AssemblyFilesystem(typeof(Bootstrapper).Assembly).Mount();
        Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        var gameAttr = typeof(Bootstrapper).Assembly.GetCustomAttribute<DefaultGameIdAttribute>();
        
        var parser = Parser.Default;
        var success = false;
        parser.ParseArguments<CliOptions>(args).WithParsed(opt => {
            CliOptions._instance = opt;
            success = true;
        });
        if (!success) return -1;
        
        var gameId = CliOptions.Instance.Game??gameAttr?.GameId??"default";

        GameBase game; 
        try {
            var conf = ReadConf(gameId);
            MountPaths(conf, gameId);
            game = GetGame(conf.Filesystem.Bin, gameId);
            game.Initlogging();
            AppMetadata.Set(conf.Name, game.GetType().Assembly.GetName().Version?.ToString(), null);
            GameWindow.Init(WindowSettings.Instance.Width, WindowSettings.Instance.Height, conf.Name, game);
        }
        catch (Exception e) when (!Debugger.IsAttached) {
            Console.WriteLine("Abort loading game due to {0}", e);
            game = new NoGame();
            game.Initlogging();
            AppMetadata.Set("Neko", typeof(Texture).Assembly.GetName().Version?.ToString(), null);
            GameWindow.Init(800, 600, "Neko", game);
        }

        Tools.Console.Init();
        GameWindow.Instance?.Run(args);
        GameWindow.Instance?.Dispose();
        NekoSDL.Quit();
        return 0;
    }
}