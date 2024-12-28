using NekoRay;
using NekoRay.Physics2D;

namespace NeuroSama.CodeData;

public class Player : Entity
{
    public IController? Controller;
    public Rigidbody2D Rigidbody;
    public CircleCollider Collider;
    public SpriteRenderer2D Sprite;
    
    // Controller = new WalkController(this);
}