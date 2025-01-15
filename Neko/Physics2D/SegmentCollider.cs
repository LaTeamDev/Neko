using Box2D;

namespace Neko.Physics2D; 

public class SegmentCollider : Collider {
    public override ShapeType Type => ShapeType.Segment;
    public override Shape CreateShape(Body body) {
        throw new NotImplementedException();
    }
}