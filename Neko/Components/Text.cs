using System.Drawing;
using System.Numerics;

namespace Neko; 

public class Text : Behaviour {
    public Font Font = Font.Default;
    public string TextString = "";
    public Vector2 Origin;
    public float Spacing = 1f;
    public Color Color = Color.White;
    public virtual void Render() {
        /*var o = Raylib.MeasureTextEx(Font._font, TextString, Font.BaseSize * Transform.LocalScale.X, Spacing) * Origin;
        Font.Draw(
            TextString, 
            new Vector2(Transform.Position.X, Transform.Position.Y), 
            new Vector2(MathF.Round(o.X), MathF.Round(o.Y)), 
            Transform.Rotation.YawPitchRollAsVector3().Z,
            Font.BaseSize*Transform.LocalScale.X,
            Spacing,
            Color);*/
    }
}