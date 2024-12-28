using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;

namespace NeuroSama.Gameplay;

public class Player : Entity
{
    public IController? Controller;
    public Rigidbody2D Rigidbody;
    public CircleCollider Collider;
    public SpriteRenderer2D Sprite;
    
    public Player() : base("Player") { }
    
    public override void Initialize()
    {
        Controller = new WalkController(this);
        Collider = AddComponent<CircleCollider>();
        // Collider.Filter = new Filter<PhysicsCategory>()
        // {
        //     Category = PhysicsCategory.Player,
        //     Mask = PhysicsCategory.Buildings | PhysicsCategory.Enemy
        // };
        Collider.Radius = 16f;
        Rigidbody = AddComponent<Rigidbody2D>();
        Rigidbody.Type = BodyType.Dynamic;
        // Rigidbody.FixedRotation = true;
        Sprite = this.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        // Sprite.Sprite = Data.GetSprite("");

        base.Initialize();
    }
}