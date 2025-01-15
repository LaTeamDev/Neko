using System.Drawing;
using System.Runtime.CompilerServices;

namespace Neko.Sdl.Video;

public static unsafe class Display {
    public static DisplayMode GetClosestFullscreenMode(uint display, int width, int height, float refreshRate, bool includeHighDensityModes) {
        var displayMode = new SDL_DisplayMode();
        SDL_GetClosestFullscreenDisplayMode(
            (SDL_DisplayID)display,
            width, 
            height, 
            refreshRate, 
            includeHighDensityModes,
            (SDL_DisplayMode*)Unsafe.AsPointer(ref displayMode)).ThrowIfError();
        return new DisplayMode(ref displayMode);
    }

    public static DisplayMode GetCurrentMode(uint display) =>
        SDL_GetCurrentDisplayMode((SDL_DisplayID)display);

    public static DisplayOrientation GetCurrentOrientation(uint display) =>
        (DisplayOrientation)(int)SDL_GetCurrentDisplayOrientation((SDL_DisplayID)display);

    public static Rectangle GetBounds(uint display) {
        var rect = new SDL_Rect();
        SDL_GetDisplayBounds((SDL_DisplayID)display,(SDL_Rect*)Unsafe.AsPointer(ref rect)).ThrowIfError();
        return new Rectangle(rect.x, rect.y, rect.w, rect.h);
    }

    public static Rectangle GetUsableBounds(uint display) {
        var rect = new SDL_Rect();
        SDL_GetDisplayUsableBounds((SDL_DisplayID)display,(SDL_Rect*)Unsafe.AsPointer(ref rect)).ThrowIfError();
        return new Rectangle(rect.x, rect.y, rect.w, rect.h);
    }

    public static uint[] GetIds() {
        var count = 0;
        var ptr = SDL_GetDisplays((int*)Unsafe.AsPointer(ref count));
        if (ptr is null) throw new SdlException("");
        var span = new Span<uint>(ptr, count);
        var arr = span.ToArray();
        SDL_free(ptr);
        return arr;
    }

    public static float GetContentScale(uint display) =>
        SDL_GetDisplayContentScale((SDL_DisplayID)display);

    public static uint GetForPoint(Point point) {
        var sdlpoint = new SDL_Point {
            x = point.X,
            y = point.Y,
        };
        return (uint)SDL_GetDisplayForPoint((SDL_Point*)Unsafe.AsPointer(ref sdlpoint));
    }
    
    public static uint GetForRect(Rectangle rectangle) {
        var rect = new SDL_Rect {
            x = rectangle.X,
            y = rectangle.Y,
            w = rectangle.Width,
            h = rectangle.Height,
        };
        return (uint)SDL_GetDisplayForRect((SDL_Rect*)Unsafe.AsPointer(ref rect));
    }

    public static string GetName(uint display) {
        var name = SDL_GetDisplayName((SDL_DisplayID)display);
        if (name is null) throw new SdlException("");
        return name;
    }

    public static void GetProperties(uint display) => throw new NotImplementedException();

    public static DisplayMode[] GetFullscreenModes(uint display) {
        var count = 0;
        var ptrptr = SDL_GetFullscreenDisplayModes((SDL_DisplayID)display, (int*)Unsafe.AsPointer(ref count));
        if (ptrptr is null) throw new SdlException("");
        var span = new Span<IntPtr>(ptrptr, count);
        var arr = new DisplayMode[count];
        int counter = 0;
        foreach (SDL_DisplayMode* ptr in span) {
            var value = *ptr;
            arr[counter++] = new DisplayMode(ref value);
        }
        SDL_free(ptrptr);
        return arr;
    }

    public static DisplayOrientation GetNaturalOrientation(uint display)  =>
        (DisplayOrientation)(int)SDL_GetNaturalDisplayOrientation((SDL_DisplayID)display);

    public static uint PrimaryDisplay {
        get {
            var display = (uint)SDL_GetPrimaryDisplay();
            if (display == 0) throw new SdlException("");
            return display;
        }
    }

    public static SystemTheme CurrentTheme() => (SystemTheme)(int)SDL_GetSystemTheme();

    public static bool ScreenSaverEnabled {
        get => SDL_ScreenSaverEnabled();
        set {
            if (value) SDL_EnableScreenSaver();
            else SDL_DisableScreenSaver();
        }
    }
}