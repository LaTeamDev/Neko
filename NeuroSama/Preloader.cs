using NekoLib.Filesystem;
using NekoRay;
using Serilog;

namespace NeuroSama;

public static class Preloader {
    public static void Preload() {
        foreach(var a in GetFilesFlatInDirectory("textures")) {
            Data.GetTexture(a).GenMipmaps();
        }
        foreach(var a in GetFilesFlatInDirectory("fonts")) {
            Data.GetFont(a);
        }
    }

    private static HashSet<string> GetFilesFlatInDirectory(string dir) {
        var files = new HashSet<string>();
        
        foreach(var directory in Files.ListDirectories(dir)) {
            files.UnionWith(GetFilesFlatInDirectory(directory));
        }
        
        foreach (var file in Files.ListFiles(dir)) {
             files.Add(file);
        }

        return files;
    }
}