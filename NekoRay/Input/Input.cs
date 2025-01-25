using System.Numerics;
using ImGuiNET;
using NekoLib.Extra;
using NekoRay.Tools;
using rlImGui_cs;
using Serilog;
using Console = NekoLib.Extra.Console;

namespace NekoRay; 

public sealed class Input {
    private Input() {}
    
    [ConCommand("bind")]
    [ConDescription("Bind action on a key")]
    public static void Bind(string key, string action) {
        if (Enum.TryParse<KeyboardKey>(key, out var kbKey)) {
            Bind(kbKey, action);
            return;
        }
        if (Enum.TryParse<MouseButton>(key, out var mKey)) {
            Bind(mKey, action);
            return;
        }
        if (Enum.TryParse<GamepadButton>(key, out var gpKey)) {
            Bind(gpKey, action);
            return;
        }
        Log.Error("Failed to bind {Action} to {Key}", action, key);
    }

    [ConCommand("bindcommand")]
    [ConDescription("Bind command on a key")]
    public static void BindCommand(string key, string command) {
        if (Enum.TryParse<KeyboardKey>(key, out var kbKey)) {
            BindCommand(kbKey, command);
            return;
        }
        Log.Error("Failed to bind {Command} to {Key}", command, key);
    }
    
    private static HashSet<KeyboardBind> _kbBinds = new();
    private static HashSet<MouseBind> _mBinds = new();
    private static HashSet<GamepadBind> _gpBinds = new();
    
    private static Dictionary<string, bool> _down = new();
    private static Dictionary<string, bool> _pressed = new();
    private static Dictionary<string, bool> _released = new();
    private static Dictionary<string, bool> _up = new();
    private static Dictionary<string, bool> _pressedRepeat = new();
    private static HashSet<string> _bindList = new();
    private static List<Tuple<KeyboardKey, string>> _commandList = new();

    public static void Bind(KeyboardKey key, string action) {
        _bindList.Add(action);
        _kbBinds.Add(new KeyboardBind(action, key));
    }
    
    public static void Bind(MouseButton key, string action) {
        _bindList.Add(action);
        _mBinds.Add(new MouseBind(action, key));
    }
    
    public static void Bind(GamepadButton key, string action) {
        _bindList.Add(action);
        _gpBinds.Add(new GamepadBind(action, key));
    }
    
    public static void BindCommand(KeyboardKey key, string command) {
        _commandList.Add(new Tuple<KeyboardKey, string>(key, command));
    }

    [ConCommand("unbind_action")]
    [ConDescription("Unbind Action")]
    public static void Unbind(string action) {
        _kbBinds.RemoveWhere(bind => bind.Name == action);
        _mBinds.RemoveWhere(bind => bind.Name == action);
        _gpBinds.RemoveWhere(bind => bind.Name == action);
    }
    
    [ConCommand("unbind_key")]
    [ConDescription("Unbind key")]
    public static void Unbind(string key, string action) {
        if (Enum.TryParse<KeyboardKey>(key, out var kbKey)) {
            Unbind(kbKey);
            return;
        }
        if (Enum.TryParse<MouseButton>(key, out var mKey)) {
            Unbind(mKey);
            return;
        }
        if (Enum.TryParse<GamepadButton>(key, out var gpKey)) {
            Unbind(gpKey);
            return;
        }
        Log.Error("Failed to bind {Action} to {Key}", action, key);
    }

    [ConCommand("unbindall")]
    [ConDescription("Unbind all actions")]
    public static void UnbindAll() {
        _bindList.Clear();
    }

    public static void Unbind(KeyboardKey key) {
        _kbBinds.RemoveWhere(bind => bind.Button == key);
    }
    public static void Unbind(MouseButton button) {
        _mBinds.RemoveWhere(bind => bind.Button == button);
    }
    public static void Unbind(GamepadButton key) {
        _gpBinds.RemoveWhere(bind => bind.Button == key);
    }
    
    [ConCommand("unbindcommand")]
    [ConDescription("Unbind command")]
    public static void UnbindCommand(string command) {
        foreach (var a in _commandList.Where(tuple => tuple.Item2 == command)) {
            _commandList.Remove(a);
        }
    }

    public static bool IsDown(string action) {
        return _down.TryGetValue(action, out var value) && value;
    }

    public static bool IsPressed(string action) {
        return _pressed.TryGetValue(action, out var value) && value;
    }

    public static bool IsReleased(string action) {
        return _released.TryGetValue(action, out var value) && value;
    }

    public static bool IsUp(string action) {
        return _up.TryGetValue(action, out var value) && value;
    }

    public static bool IsRepeat(string action) {
        return _pressedRepeat.TryGetValue(action, out var value) && value;
    }

    internal static Vector2 _lastMousePos;
    public static Vector2 MousePosition {
        get => _lastMousePos;
        internal set => _lastMousePos = value;
    }
    
    internal static void UpdateMouseBinds() {
        foreach (var bind in _mBinds) {
            _down[bind.Name] |= bind.Down;
            _pressed[bind.Name] |= bind.Pressed;
            _released[bind.Name] |= bind.Released;
            _up[bind.Name] |= bind.Up;
        }
    }

    internal static void UpdateKeyboardBinds() {
        foreach (var bind in _kbBinds) {
            _down[bind.Name] |= bind.Down;
            _pressed[bind.Name] |= bind.Pressed;
            _released[bind.Name] |= bind.Released;
            _up[bind.Name] |= bind.Up;
            _pressedRepeat[bind.Name] |= bind.Repeat;
        }
    }

    internal static void UpdateGamepadBinds() {
        foreach (var bind in _gpBinds) {
            _down[bind.Name] |= bind.Down;
            _pressed[bind.Name] |= bind.Pressed;
            _released[bind.Name] |= bind.Released;
            _up[bind.Name] |= bind.Up;
        }
    }

    public static bool ForceUpdate = false;
    
    public static bool KeyboardLocked { get; private set; }
    public static bool MouseLocked { get; private set; }
    public static bool MousePositionLocked { get; private set; }

    public static void Update() {
        var io = ImGui.GetIO();
        
        KeyboardLocked = io.WantCaptureKeyboard && !ForceUpdate;
        MouseLocked = io.WantCaptureMouse && !ForceUpdate;
        MousePositionLocked = io.WantCaptureMouse;
        
        foreach (var bind in _bindList) {
            _down[bind] = _pressed[bind] = _released[bind] = _up[bind] = _pressedRepeat[bind] = false;
        }

        if (!KeyboardLocked) {
            foreach (var commandBind in _commandList) {
                if (commandBind.Item1.IsPressed()) Console.Submit(commandBind.Item2);
            }
            UpdateKeyboardBinds();
        }

        if (!MouseLocked) {
            UpdateMouseBinds();
        }
        if (!MousePositionLocked)
            MousePosition = Raylib.GetMousePosition();
        
        if (Raylib.IsGamepadAvailable(0))
            UpdateGamepadBinds();
    }
}