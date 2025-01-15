using System.Numerics;
using Box2D;

namespace Neko.Physics2D; 

public class BoxCollider : PolygonCollider {

    public float Width;
    public float Height;
    public override Shape CreateShape(Body body) {
        return body.CreatePolygonShape(ShapeDef,
            Polygon.MakeBox(Width, Height, Vector2.Zero, 
                Transform.Rotation.GetEulerAngles().Z));
    }
}