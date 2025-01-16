using NekoLib.Filesystem;
using Neko.Sdl.Video;
using Serilog;
using Tomlyn;

namespace Neko; 

public class WindowSettings {
    private static WindowSettings? _instance;
    public static WindowSettings Instance {
        get {
            if (_instance is not null) return _instance;
            if (Files.FileExists("cfg/video.toml"))
                _instance = Toml.ToModel<WindowSettings>(Files.GetFile("cfg/video.toml").Read());
            else {
                if (Files.FileExists("cfg/video.default.toml"))
                    _instance = Toml.ToModel<WindowSettings>(Files.GetFile("cfg/video.default.toml").Read());
                else
                    _instance = new();
                Save();
            }
            return _instance;
        }
    }

    public static void Save() {
        try {
            Files.GetWritableFilesystem().CreateFile("cfg/video.toml").Write(Toml.FromModel(Instance));
            Log.Verbose("Successfully saved video settings");
        }
        catch (Exception e) {
            Log.Error("Failed to save video settings");
            Log.Verbose(e, "Failed to save video settings");
        }
    }

    public int Width { get; set; }
    public int Height { get; set; }

    internal WindowFlags _flags;

    public bool Fullscreen {
        get => _flags.HasFlag(WindowFlags.Fullscreen);
        set {
            if (value) _flags |= WindowFlags.Fullscreen;
            else _flags &= ~WindowFlags.Fullscreen;
        }
    }

    public bool OpenGL {
        get => _flags.HasFlag(WindowFlags.Opengl);
        set {
            if (value) _flags |= WindowFlags.Opengl;
            else _flags &= ~WindowFlags.Opengl;
        }
    }

    public bool Occluded {
        get => _flags.HasFlag(WindowFlags.Occluded);
        set {
            if (value) _flags |= WindowFlags.Occluded;
            else _flags &= ~WindowFlags.Occluded;
        }
    }

    public bool Hidden {
        get => _flags.HasFlag(WindowFlags.Hidden);
        set {
            if (value) _flags |= WindowFlags.Hidden;
            else _flags &= ~WindowFlags.Hidden;
        }
    }

    public bool Borderless {
        get => _flags.HasFlag(WindowFlags.Borderless);
        set {
            if (value) _flags |= WindowFlags.Borderless;
            else _flags &= ~WindowFlags.Borderless;
        }
    }

    public bool Resizable {
        get => _flags.HasFlag(WindowFlags.Resizable);
        set {
            if (value) _flags |= WindowFlags.Resizable;
            else _flags &= ~WindowFlags.Resizable;
        }
    }

    public bool Minimized {
        get => _flags.HasFlag(WindowFlags.Minimized);
        set {
            if (value) _flags |= WindowFlags.Minimized;
            else _flags &= ~WindowFlags.Minimized;
        }
    }

    public bool Maximized {
        get => _flags.HasFlag(WindowFlags.Maximized);
        set {
            if (value) _flags |= WindowFlags.Maximized;
            else _flags &= ~WindowFlags.Maximized;
        }
    }

    public bool MouseGrabbed {
        get => _flags.HasFlag(WindowFlags.MouseGrabbed);
        set {
            if (value) _flags |= WindowFlags.MouseGrabbed;
            else _flags &= ~WindowFlags.MouseGrabbed;
        }
    }

    public bool InputFocus {
        get => _flags.HasFlag(WindowFlags.InputFocus);
        set {
            if (value) _flags |= WindowFlags.InputFocus;
            else _flags &= ~WindowFlags.InputFocus;
        }
    }

    public bool MouseFocus {
        get => _flags.HasFlag(WindowFlags.MouseFocus);
        set {
            if (value) _flags |= WindowFlags.MouseFocus;
            else _flags &= ~WindowFlags.MouseFocus;
        }
    }

    public bool HighPixelDensity {
        get => _flags.HasFlag(WindowFlags.HighPixelDensity);
        set {
            if (value) _flags |= WindowFlags.HighPixelDensity;
            else _flags &= ~WindowFlags.HighPixelDensity;
        }
    }

    public bool MouseCapture {
        get => _flags.HasFlag(WindowFlags.MouseCapture);
        set {
            if (value) _flags |= WindowFlags.MouseCapture;
            else _flags &= ~WindowFlags.MouseCapture;
        }
    }

    public bool AlwaysOnTop {
        get => _flags.HasFlag(WindowFlags.AlwaysOnTop);
        set {
            if (value) _flags |= WindowFlags.AlwaysOnTop;
            else _flags &= ~WindowFlags.AlwaysOnTop;
        }
    }

    public bool Utility {
        get => _flags.HasFlag(WindowFlags.Utility);
        set {
            if (value) _flags |= WindowFlags.Utility;
            else _flags &= ~WindowFlags.Utility;
        }
    }

    public bool Tooltip {
        get => _flags.HasFlag(WindowFlags.Tooltip);
        set {
            if (value) _flags |= WindowFlags.Tooltip;
            else _flags &= ~WindowFlags.Tooltip;
        }
    }

    public bool PopupMenu {
        get => _flags.HasFlag(WindowFlags.PopupMenu);
        set {
            if (value) _flags |= WindowFlags.PopupMenu;
            else _flags &= ~WindowFlags.PopupMenu;
        }
    }

    public bool KeyboardGrabbed {
        get => _flags.HasFlag(WindowFlags.KeyboardGrabbed);
        set {
            if (value) _flags |= WindowFlags.KeyboardGrabbed;
            else _flags &= ~WindowFlags.KeyboardGrabbed;
        }
    }

    public bool Vulkan {
        get => _flags.HasFlag(WindowFlags.Vulkan);
        set {
            if (value) _flags |= WindowFlags.Vulkan;
            else _flags &= ~WindowFlags.Vulkan;
        }
    }

    public bool Metal {
        get => _flags.HasFlag(WindowFlags.Metal);
        set {
            if (value) _flags |= WindowFlags.Metal;
            else _flags &= ~WindowFlags.Metal;
        }
    }

    public bool Transparent {
        get => _flags.HasFlag(WindowFlags.Transparent);
        set {
            if (value) _flags |= WindowFlags.Transparent;
            else _flags &= ~WindowFlags.Transparent;
        }
    }

    public bool NotFocusable {
        get => _flags.HasFlag(WindowFlags.NotFocusable);
        set {
            if (value) _flags |= WindowFlags.NotFocusable;
            else _flags &= ~WindowFlags.NotFocusable;
        }
    }
}