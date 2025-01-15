using System.Drawing;
using System.Numerics;
using System.Text.Json;
using NekoLib.Filesystem;

namespace Neko.Data;

public class SpriteFile {
    public string Texture { get; set; }
    public Rectangle Bounds { get; set; }
    public Vector2 Origin { get; set; }
    public float PixelSize { get; set; }
    public Dictionary<string, Vector2> AttachmentPoints { get; set; }

    public static SpriteFile? Load(string path) {
        using var stream = Files.GetFile(path).GetStream();
        return JsonSerializer.Deserialize<SpriteFile>(stream, SourceGenerationContext.Default.SpriteFile);
    }
}