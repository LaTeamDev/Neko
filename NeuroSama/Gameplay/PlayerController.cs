using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using NeuroSama.Gameplay;
namespace NeuroSama.Gameplay;

public class PlayerController: Behaviour
{
    public PlayerController Player { get; }
    public Rigidbody2D rb;
    private Sprite sprite;

    public PlayerController()
    {
        sprite = Data.GetSprite("textures/characters/neuro/idle-Sheet.png");
    }
    
    void Awake() {}

    void Render()
    {
        // sprite.Draw();
    }
    
    public void Update(){}

}