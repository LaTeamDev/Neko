using System.Numerics;
using NekoLib.Filesystem;
using NekoRay.Data;
using NekoRay.Tools;

namespace NekoRay; 

public class Sprite : NekoObject, IAsset {
    public Texture Texture { get; set; }
    public Rectangle Bounds;
    public Vector2 Origin;
    public float Width => Bounds.Width;
    public float Height => Bounds.Height;
    public Sprite(Texture texture, Rectangle bounds) {
        Texture = texture;
        Bounds = bounds;
    }

    public string Path { get; private set; } = "";
    
    internal static Dictionary<string, Sprite> _cache = new();

    [ConCommand("sprite_reload_all")]
    [ConDescription("Reloads all sprites from disk")]
    public static void ReloadTextures() {
        foreach (var sprite in _cache) {
            sprite.Value.Reload();
        }
    }
    
    [ConCommand("sprite_reload")]
    [ConDescription("Reloads sprite from disk")]
    public static void ReloadTexture(string name) {
        if (!_cache.TryGetValue(name, out var sprite)) throw new ArgumentException("The specified texture was not loaded", nameof(name));
        sprite.Reload();
    }

    public static Sprite NoSprite {
        get {
            if (_cache.TryGetValue("@__nosprite", out var sprite)) 
                return sprite;
            var texture = Texture.NoTexture;
            return _cache["@__nosprite"] = new Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height));
        }
    }

    public static Sprite Load(string path) {
        path = path.Replace('\\', '/');
        if (_cache.TryGetValue(path, out var sprite)) return sprite;
        if (!Files.FileExists(path)) return NoSprite;
        sprite = LoadUncached(path);
        _cache.Add(path, sprite);
        return sprite;
    }

    public static Sprite LoadUncached(string path) {
        var spriteFile = SpriteFile.Load(path);
        var sprite = new Sprite(Texture.Load(spriteFile.Texture), spriteFile.Bounds) {
            Origin = spriteFile.Origin,
            Path = path,
        };
        return sprite;
    }

    public void Reload() {
        
    }
    
    public void Draw(Vector2 destination, Vector2? scale = null, float rotation = 0f, Color? color = null) {
        scale ??= Vector2.One;
        color ??= Raylib.WHITE;
        var source = new Rectangle(
            Bounds.X, Bounds.Y,
            Bounds.Width * Math.Sign(scale.Value.X), Bounds.Height * Math.Sign(scale.Value.Y));
        var dest = new Rectangle(
            destination.X, destination.Y,
            Bounds.Width * Math.Abs(scale.Value.X), Bounds.Height * Math.Abs(scale.Value.Y));

        Raylib.DrawTexturePro(
            Texture._texture, 
            source, 
            dest, 
            new Vector2(Origin.X * Math.Abs(scale.Value.X), Origin.Y * Math.Abs(scale.Value.Y)), 
            rotation, 
            color.Value);
    }
}