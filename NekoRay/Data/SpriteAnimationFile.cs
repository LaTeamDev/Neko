using System.Numerics;
using System.Text.Json;
using NekoLib.Filesystem;

namespace NekoRay.Data;

public class SpriteAnimationFile {
    public class Frame {
        public string Sprite { get; set; }
        public float Time { get; set; }
        public float Rotation { get; set; }
        public Vector2 Scale { get; set; } = Vector2.One;
        public Vector2 Offset { get; set; } = Vector2.One;
        public bool Interpolation { get; set; } = false;
    }
    
    public string Name { get; set; }
    public Frame[] Frames { get; set; }
    public float Speed { get; set; }
    public bool Loop { get; set; }
    
    public static SpriteAnimationFile? Load(string path) {
        using var stream = Files.GetFile(path).GetStream();
        return JsonSerializer.Deserialize<SpriteAnimationFile>(stream, SourceGenerationContext.Default.SpriteAnimationFile);
    }
}