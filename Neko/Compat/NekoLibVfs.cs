using NekoLib.Extra;
using NekoLib.Filesystem;

namespace Neko.Compat;

public class NekoLibVfs : IConsoleFilesystem {
    public bool Exists(string path) => Files.FileExists(path);
    public string Read(string path) => Files.GetFile(path).Read();
}