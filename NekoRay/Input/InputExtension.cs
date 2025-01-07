namespace NekoRay;

public static class InputExtension {
    public static bool IsDown(this KeyboardKey key) => Raylib.IsKeyDown(key);
    public static bool IsUp(this KeyboardKey key) => Raylib.IsKeyUp(key);
    public static bool IsPressed(this KeyboardKey key) => Raylib.IsKeyPressed(key);
    public static bool IsReleased(this KeyboardKey key) => Raylib.IsKeyReleased(key);
    public static bool IsPressedRepeat(this KeyboardKey key) => Raylib.IsKeyPressedRepeat(key);
    
    public static bool IsDown(this MouseButton key) => Raylib.IsMouseButtonDown(key);
    public static bool IsUp(this MouseButton key) => Raylib.IsMouseButtonUp(key);
    public static bool IsPressed(this MouseButton key) => Raylib.IsMouseButtonPressed(key);
    public static bool IsReleased(this MouseButton key) => Raylib.IsMouseButtonReleased(key);
    
    public static bool IsDown(this GamepadButton key, int gamepad) => Raylib.IsGamepadButtonDown(gamepad, key);
    public static bool IsUp(this GamepadButton key, int gamepad) => Raylib.IsGamepadButtonUp(gamepad, key);
    public static bool IsPressed(this GamepadButton key, int gamepad) => Raylib.IsGamepadButtonPressed(gamepad, key);
    public static bool IsReleased(this GamepadButton key, int gamepad) => Raylib.IsGamepadButtonReleased(gamepad, key);
    
    public static float GetMovement(this GamepadAxis key, int gamepad) => Raylib.GetGamepadAxisMovement(gamepad, key);
}