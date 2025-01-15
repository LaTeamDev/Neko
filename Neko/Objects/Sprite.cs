using System.Numerics;
using Neko.Data;
using NekoLib.Filesystem;
using Neko.Tools;

namespace Neko; 

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

    public static Sprite NoSprite {
        get {
            if (AssetCache.TryGet<Sprite>("@__nosprite", out var sprite)) 
                return sprite;
            var texture = Texture.NoTexture;
            return AssetCache.Add(new Sprite(texture, new Rectangle(0, 0, texture.Width, texture.Height)));
        }
    }

    public static Sprite Load(string path) {
        path = path.Replace('\\', '/');
        if (AssetCache.TryGet<Sprite>(path, out var sprite)) return sprite;
        if (!Files.FileExists(path)) return NoSprite;
        sprite = LoadUncached(path);
        return AssetCache.Add(sprite);
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
        var spriteFile = SpriteFile.Load(Path);
        Texture = Texture.Load(spriteFile.Texture);
        Bounds = spriteFile.Bounds;
        Origin = spriteFile.Origin;
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

    public override void Dispose() {
        base.Dispose();
        if (Path != null) AssetCache.Remove(this);
    }
}