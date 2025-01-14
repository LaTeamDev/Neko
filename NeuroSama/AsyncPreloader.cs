using NekoLib.Core;
using NekoLib.Filesystem;
using NekoLib.Scenes;
using NekoRay;
using Serilog;
using ZeroElectric.Vinculum;
using Font = ZeroElectric.Vinculum.Font;
using Texture = NekoRay.Texture;

namespace NeuroSama;

public class AsyncPreloader(IScene next) : IScene {
    private static HashSet<string> GetFilesFlatInDirectory(string dir) {
        var files = new HashSet<string>();
        
        foreach(var directory in Files.ListDirectories(dir)) {
            files.UnionWith(GetFilesFlatInDirectory(directory));
        }
        
        foreach (var file in Files.ListFiles(dir)) {
            files.Add(file);
        }

        return files;
    }

    public string Name => "Preloader";
    public bool DestroyOnLoad => true;
    public int Index { get; set; }
    public List<GameObject> GameObjects => new();
    public List<string> Textures;
    public List<string> Fonts;
    public string LastLoaded;
    private Task textureLoadingTask;
    private int idx;
    public void Initialize() {
        Textures = GetFilesFlatInDirectory("textures").ToList();
        Fonts = GetFilesFlatInDirectory("fonts").ToList();
        textureLoadingTask = LoadTextures();
    }

    public async Task LoadTextures() {
        var textureLoad = new List<Task<Texture>>();
        foreach (var texture in Textures) {
            textureLoad.Add(Texture.LoadAsync(texture));
        }

        await Task.WhenAll(textureLoad);
    }

    public void Update() {
        if (!textureLoadingTask.IsCompletedSuccessfully) {
            return;
        }
        LastLoaded = Fonts[idx++];
        Data.GetFont(LastLoaded);
        if (idx >= Fonts.Count) return;
        SceneManager.LoadScene(next);
    }

    public void Draw() {
        Raylib.DrawText("Loaded "+LastLoaded+" ", 0, 0, 16, Raylib.WHITE);
    }
}