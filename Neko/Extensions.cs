using System.Drawing;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Box2D.Interop;
using ImGuiNET;
using NekoLib.Filesystem;

namespace Neko; 

public static class Extensions {
    public static GameObject AddChild(this GameObject gameObject, string name = "GameObject") {
        var scene = SceneManager.ActiveScene;
        SceneManager.SetSceneActive(gameObject.Scene);
        var go = new GameObject(name);
        go.Transform.Parent = gameObject.Transform;
        SceneManager.SetSceneActive(scene);
        return go;
    }

    public static Vector2 ToVector2(this Vector3 vector) => new(vector.X, vector.Y);
    
    public static void RemoveByValue<T, T2>(this Dictionary<T, T2> dictionary, T2 value) where T : notnull {
        foreach (var kv in dictionary) {
            if (kv.Value == null || !kv.Value.Equals(value)) continue;
            dictionary.Remove(kv.Key);
        }
    }

    public static unsafe ImFontPtr AddFontFromFilesystemTTF(this ImFontAtlasPtr fontAtlas, string filename, float size_pixels) {
        var file = Files.GetFile(filename).ReadBinary();
        var ptr = Marshal.AllocHGlobal(file.Length);
        var span = new Span<byte>((void*)ptr, file.Length);
        file.CopyTo(span);
        return fontAtlas.AddFontFromMemoryTTF(ptr, span.Length, size_pixels);
    }
    
    public static unsafe ImFontPtr AddFontFromFilesystemTTF(this ImFontAtlasPtr fontAtlas, string filename, float size_pixels, ImFontConfigPtr font_cfg) {
        var file = Files.GetFile(filename).ReadBinary();
        var ptr = Marshal.AllocHGlobal(file.Length);
        var span = new Span<byte>((void*)ptr, file.Length);
        file.CopyTo(span);
        return fontAtlas.AddFontFromMemoryTTF(ptr, span.Length, size_pixels, font_cfg);
    }

    public static Vector2 ToVector2(this Point point) => new(point.X, point.Y);
    public static Vector2 ToVector2(this Size point) => new(point.Width, point.Height);
    public static Point ToPoint(this Vector2 vector) => new((int)vector.X, (int)vector.Y);
    public static Size ToSize(this Vector2 vector) => new((int)vector.X, (int)vector.Y);
    

    // public static Sprite ToSprite(this Texture texture) =>
    //     new (texture, new Rectangle(0, 0, texture.Width, texture.Height));
    
    public static Quaternion YawPitchRollAsQuaternion(this Vector3 rotation) =>
        Quaternion.CreateFromYawPitchRoll(rotation.X, rotation.Y, rotation.Z);


    //private static Stack<BlendMode> _blendModes = new();
    /*public static AttachMode Attach(this BlendMode blendMode) {
        Raylib.BeginBlendMode(blendMode);
        _blendModes.Push(blendMode);
        return new AttachMode(()=> {
            Raylib.EndBlendMode();
            _blendModes.Pop();
            if (_blendModes.TryPeek(out var prev))
                Raylib.BeginBlendMode(prev);
        });
    }
    */
}