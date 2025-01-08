using System.Text.Json;
using System.Text.Json.Serialization;
using NekoLib.Filesystem;
using NekoRay;

namespace Touhou;

public sealed class PlayerInfo : IAsset, IDisposable {
    public required string DisplayName { get; set; }
    public required string Type { get; set; }
    public required float BaseSpeed { get; set; }
    public required float FocusedSpeed { get; set; }

    [JsonInclude]
    private string[] Animations {
        get => LoadedAnimations.Select(animation => animation.Path).ToArray();
        set {
            LoadedAnimations.Clear();
            foreach (var str in value) {
                LoadedAnimations.Add(SpriteAnimation.Load(str));
            }
        }
    }

    [JsonIgnore]
    public List<SpriteAnimation> LoadedAnimations = new();
    public string Path { get; private set; }

    public static PlayerInfo LoadUncached(string path) {
        using var stream = Files.GetFile(path).GetStream();
        var plr = JsonSerializer.Deserialize<PlayerInfo>(stream, JsonSerializerOptions.Web);
        plr.Path = path;
        return plr;
    }
    
    public static PlayerInfo Load(string path) {
        path = path.Replace('\\', '/');
        if (AssetCache.TryGet<PlayerInfo>(path, out var playerInfo)) return playerInfo;
        if (!Files.FileExists(path)) throw new FileNotFoundException();
        playerInfo = LoadUncached(path);
        AssetCache.Add(playerInfo);
        All.Add(playerInfo);
        return playerInfo;
    }

    public static HashSet<PlayerInfo> All = [];

    public static void PreloadAll() {
        foreach (var charpath in Files.ListFiles("data/characters")) {
            if (!charpath.EndsWith(".plr")) continue;
            Load(charpath);
        }
    }
    
    public void Reload() {
        using var stream = Files.GetFile(Path).GetStream();
        var newInfo = JsonSerializer.Deserialize<PlayerInfo>(stream, JsonSerializerOptions.Web);
        DisplayName = newInfo.DisplayName;
        Type = newInfo.Type;
        BaseSpeed = newInfo.BaseSpeed;
        FocusedSpeed = newInfo.FocusedSpeed;
        Animations = newInfo.Animations;
    }
    
    public void Dispose() {
        if (Path == "") AssetCache.Remove(this);
    }
}