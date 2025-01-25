using NekoLib.Extra;
using NekoLib.Tools;

namespace Neko.Tools;

public static class ExtraCommands {
    [ConCommand("quit")]
    public static void Quit() {
        Program.Quit();
    }
}