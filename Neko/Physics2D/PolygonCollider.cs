using Box2D;

namespace Neko.Physics2D; 

public class PolygonCollider : Collider {
    public override ShapeType Type => ShapeType.Polygon;
    public override Shape CreateShape(Body body) {
        throw new NotImplementedException();
    }
}