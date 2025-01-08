using System.Numerics;
using NekoLib.Filesystem;
using NekoRay.Tools;
using ZeroElectric.Vinculum.Extensions;

namespace NekoRay; 

public class Texture : NekoObject, IAsset {
    internal RayTexture _texture;
    internal Texture() { }

    public static Texture NoTexture => Load("textures/__notexture.png");
    
    public static Texture Load(string path) {
        path = path.Replace('\\', '/');
        if (AssetCache.TryGet<Texture>(path, out var texture)) return texture;
        if (!Files.FileExists(path)) return NoTexture;
        texture = LoadUncached(path);
        return AssetCache.Add(texture);
    }

    public static Texture LoadUncached(string file) {
        using var image = Image.Load(file);
        var texture = FromImage(image);
        texture.Path = file;
        return texture;
    }

    public string? Path { get; set; }
    

    public static Texture FromImage(Image image) {
        return new Texture() {
            _texture = Raylib.LoadTextureFromImage(image._image)
        };
    }

    public void Reload() {
        if (Path is null or "") {
            throw new FileNotFoundException("Attempt to load texture without a path");
        }
        Raylib.UnloadTexture(_texture);
        using var image = Image.Load(Path);
        _texture = Raylib.LoadTextureFromImage(image._image);
    }

    public bool IsReady => Raylib.IsTextureReady(_texture);
    public uint Id => _texture.id;
    public int Width => _texture.width;
    public int Height => _texture.height;

    public override void Dispose() {
        Raylib.UnloadTexture(_texture);
        if (Path != null) AssetCache.Remove(this);
    }

    //TODO: no pointers in public api
    public unsafe void Update(void* pixels) {
        Raylib.UpdateTexture(_texture, pixels);
    }
    
    public unsafe void Update(void* pixels, Rectangle rectangle) {
        Raylib.UpdateTextureRec(_texture, rectangle, pixels);
    }

    public unsafe void GenMipmaps() {
        var pin = _texture.GcPin();
        Raylib.GenTextureMipmaps((RayTexture*)pin.AddrOfPinnedObject());
        pin.Free();
    }

    private TextureFilter _filter; 
    public TextureFilter Filter {
        get => _filter;
        set {
            _filter = value;
            Raylib.SetTextureFilter(_texture, _filter);
        }
    }
    
    private TextureWrap _wrap; 
    public TextureWrap Wrap {
        get => _wrap;
        set {
            _wrap = value;
            Raylib.SetTextureWrap(_texture, _wrap);
        }
    }

    public void Draw(Vector2 position, Color tint) {
        Raylib.DrawTextureV(_texture, position, tint);
    }

    public void Draw(Vector2 position, float rotation, float scale, Color tint) {
        Raylib.DrawTextureEx(_texture, position, rotation, scale, tint);
    }

    public void Draw(Rectangle source, Rectangle dest, Vector2 origin, float rotation, Color color) {
        Raylib.DrawTexturePro(_texture, source, dest, origin, rotation, color);
    }
    
    
    //TODO: implement left
}