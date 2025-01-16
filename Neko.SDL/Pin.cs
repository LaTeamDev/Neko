using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Neko.Sdl;

public class Pin<T> : IDisposable {
    private GCHandle _handle;
    public IntPtr Addr => _handle.AddrOfPinnedObject();
    public IntPtr Pointer => GCHandle.ToIntPtr(_handle);
    
    public bool IsAllocated => _handle.IsAllocated;
    public bool IsOwner { get; }
    
    public Pin(T obj, GCHandleType type = GCHandleType.Pinned) {
        _handle = GCHandle.Alloc(obj, type);
        IsOwner = true;
    }

    public Pin(IntPtr ptr, bool takeOwnership = false) {
        _handle = GCHandle.FromIntPtr(ptr);
        IsOwner = takeOwnership;
    }
    
    public bool TryGetTarget([MaybeNullWhen(false)] out T target)
    {
        if (!IsAllocated) {
            target = default;
            return false;
        }

        try {
            if (_handle.Target is T value) {
                target = value;
                return true;
            }
        }
        catch (InvalidOperationException) { }

        target = default;
        return false;
    }

    
    public void Dispose() {
        if (IsOwner && IsAllocated)
            _handle.Free();
    }
}

public static class PintExtension {
    public static Pin<T> Pin<T>(this T obj, GCHandleType type = GCHandleType.Pinned) => new(obj, type);
}