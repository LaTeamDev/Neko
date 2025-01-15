namespace Neko.Sdl;

public static class Extensions {
    public static void ThrowIfError(this SDLBool result, string message = "") {
        if (!result) throw new SdlException(message);
    }
}