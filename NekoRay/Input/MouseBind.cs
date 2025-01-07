namespace NekoRay;

public class MouseBind(string name, MouseButton button) : Bind<MouseButton>(name, button) {
    public override bool Down => Button.IsDown();
    public override bool Pressed => Button.IsPressed();
    public override bool Released => Button.IsReleased();
    public override bool Up => Button.IsUp();
}