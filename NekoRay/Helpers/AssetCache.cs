using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using NekoLib.Extra;
using NekoRay.Tools;

namespace NekoRay;

public static class AssetCache {
    internal static Dictionary<string, IAsset> Cache = new();
    
    [ConCommand("asset_reload_all")]
    [ConDescription("Reloads all loaded assets from disk")]
    public static void ReloadAll() {
        foreach (var asset in Cache) {
            asset.Value.Reload();
        }
    }
    
    [ConCommand("asset_reload")]
    [ConDescription("Reloads asset from disk")]
    public static void Reload(string path) {
        if (!Cache.TryGetValue(path, out var sprite)) 
            throw new ArgumentException("The specified asset was not loaded", nameof(path));
        sprite.Reload();
    }

    public static IAsset Add(IAsset asset) {
        if (asset.Path is null or "") 
            throw new ArgumentException("Attempt to cache asset with empty path", nameof(asset));
        Cache.Add($"{asset.GetType()}@{asset.Path}", asset);
        return asset;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TAsset Add<TAsset>(TAsset asset) where TAsset : IAsset{
        return (TAsset)Add((IAsset)asset);
    }

    public static bool TryGet(string path, [MaybeNullWhen(false)] out IAsset asset) {
        asset = Cache.FirstOrDefault(pair => pair.Key.Split('@')[1] == path).Value;
        return asset is not null;
    }

    public static IAsset Get(string path) => Cache[path];

    public static bool TryGet<TAsset>(string path, [MaybeNullWhen(false)] out TAsset asset) where TAsset : class?, IAsset? {
        if (Cache.TryGetValue($"{typeof(TAsset)}@{path}", out var asset1)) {
            if (asset1 is not TAsset asset2) 
                throw new Exception("the cached asset of correct type contains object of wrong type?? how??");
            asset = asset2;
            return true;
        }
        asset = null;
        return false;
    }

    public static TAsset Get<TAsset>(string path) where TAsset : class, IAsset {
        if (Get(path) is TAsset asset)
            return asset;
        throw new ArgumentException("Attempt to get cached asset with wrong type", nameof(TAsset));
    }

    public static void Remove(IAsset asset) => Cache.Remove(asset.Path);
}