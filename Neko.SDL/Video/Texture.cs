using System.Drawing;
using System.Runtime.CompilerServices;
using CommunityToolkit.HighPerformance;

namespace Neko.Sdl.Video;

public unsafe partial class Texture : SdlWrapper<SDL_Texture> {
    private Renderer? _renderer;

    public Renderer Renderer {
        get {
            if (_renderer is null)
                _renderer = Renderer.GetRendererFromTexture(this);
            return _renderer;
        }
    }

    public Texture(Renderer renderer, PixelFormat format, TextureAccess access, int width, int height) {
        _renderer = renderer;
        Handle = renderer.CreateTexture(format, access, width, height);
    }

    public Texture(Renderer renderer, Surface surface) {
        _renderer = renderer;
        Handle = renderer.CreateTextureFromSurface(surface);
    }

    public Texture(Renderer renderer, Properties properties) {
        _renderer = renderer;
        Handle = renderer.CreateTextureWithProperties(properties);
    }

    public byte Alpha {
        get {
            byte al = 0;
            SDL_GetTextureAlphaMod(this, (byte*)Unsafe.AsPointer(ref al)).ThrowIfError();
            return al;
        }
        set => SDL_SetTextureAlphaMod(this, value);
    }
    
    public float AlphaF {
        get {
            var al = 0f;
            SDL_GetTextureAlphaModFloat(this, (float*)Unsafe.AsPointer(ref al)).ThrowIfError();
            return al;
        }
        set => SDL_SetTextureAlphaModFloat(this, value);
    }
    
    public BlendMode BlendMode {
        get {
            BlendMode al = 0;
            SDL_GetTextureBlendMode(this, (SDL_BlendMode*)Unsafe.AsPointer(ref al)).ThrowIfError();
            return al;
        }
        set => SDL_SetTextureBlendMode(this, (SDL_BlendMode)value);
    }
    
    public Color Color {
        get {
            var a = new Color();
            SDL_GetTextureColorMod(this, 
                (byte*)Unsafe.AsPointer(ref a.R),
                (byte*)Unsafe.AsPointer(ref a.G),
                (byte*)Unsafe.AsPointer(ref a.B)
            ).ThrowIfError();
            return a with {A = Alpha};
        }
        set => SDL_SetTextureColorMod(Handle, value.R, value.G, value.B).ThrowIfError();
    }
    
    public ColorF ColorF {
        get {
            var a = new ColorF();
            SDL_GetTextureColorModFloat(this, 
                (float*)Unsafe.AsPointer(ref a.R),
                (float*)Unsafe.AsPointer(ref a.G),
                (float*)Unsafe.AsPointer(ref a.B)
            ).ThrowIfError();
            return a with {A = AlphaF};
        }
        set => SDL_SetTextureColorModFloat(Handle, value.R, value.G, value.B).ThrowIfError();
    }
    
    public Properties Properties => (Properties)SDL_GetTextureProperties(this);
    
    public ScaleMode ScaleMode {
        get {
            ScaleMode al = 0;
            SDL_GetTextureScaleMode(this, (SDL_ScaleMode*)Unsafe.AsPointer(ref al)).ThrowIfError();
            return al;
        }
        set => SDL_SetTextureScaleMode(this, (SDL_ScaleMode)value);
    }

    public SizeF Size {
        get {
            var size = new SizeF();
            SDL_GetTextureSize(this, 
                (float*)Unsafe.AsPointer(ref size), 
                (float*)((IntPtr)Unsafe.AsPointer(ref size)+Unsafe.SizeOf<float>())).ThrowIfError();
            return size;
        }
    }

    public Span2D<Color> Lock(Rectangle rect) {
        //should we free memory here?
        var ptrptr = (IntPtr*)0;
        var pitch = 0;
        SDL_LockTexture(this, (SDL_Rect*)Unsafe.AsPointer(ref rect), ptrptr, (int*)Unsafe.AsPointer(ref pitch)).ThrowIfError();
        var span = new Span2D<Color>(ptrptr, rect.Height, rect.Width, 0);
        return span;
    }

    public Span2D<Color> Lock() {
        var ptrptr = (IntPtr*)0;
        var pitch = 0;
        SDL_LockTexture(this, null, ptrptr, (int*)Unsafe.AsPointer(ref pitch)).ThrowIfError();
        var span = new Span2D<Color>(ptrptr, (int)Size.Height, (int)Size.Width, 0);
        return span;
    }

    public Surface LockToSurface(Rectangle rect) {
        var surfacePtr = (SDL_Surface**)0;
        SDL_LockTextureToSurface(this, (SDL_Rect*)Unsafe.AsPointer(ref rect), surfacePtr).ThrowIfError();
        return *surfacePtr;
    }

    public Surface LockToSurface() {
        var surfacePtr = (SDL_Surface**)0;
        SDL_LockTextureToSurface(this, null, surfacePtr).ThrowIfError();
        return *surfacePtr;
    }
    
    public void UnlockTexture() => SDL_UnlockTexture(this);
    
    public static void UpdateNVTexture() => throw new NotImplementedException();
    public static void UpdateTexture() => throw new NotImplementedException();
    public static void UpdateYUVTexture() => throw new NotImplementedException();

    public override void Dispose() {
        base.Dispose();
        SDL_DestroyTexture(this);
    }
}