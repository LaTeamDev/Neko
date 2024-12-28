using NekoRay.Physics2D;

namespace NeuroSama.Gameplay;

public class WalkController: IController
{
    public Player Player { get; }
    public Rigidbody2D rb;

    public WalkController(Player player) {
        Player = player;
    }
    
    public void Update(){}

}