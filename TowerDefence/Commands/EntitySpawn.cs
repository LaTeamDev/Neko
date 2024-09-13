using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;
using NekoRay.Tools;
using Serilog;
using TowerDefence.Gameplay;
using TowerDefence.Gameplay.Buildings;
using TowerDefence.Gameplay.AI;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;

namespace TowerDefence.Commands;

public partial class Commands {
    public delegate GameObject EntityBuilder();

    private static Dictionary<string, EntityBuilder> _ents = new();
    public static void EntAdd(string paramName, EntityBuilder b) {
        _ents[paramName] = b;
    }

    [ConCommand("spawn")]
    [ConTags("cheat")]
    public static GameObject EntSpawn(string name) {
        if (!_ents.TryGetValue(name, out var builder)) {
            Log.Error("No registered entity builder found");
            return null;
        }

        var gameObject = builder();

        InitializeTree(gameObject);
        gameObject.Transform.Position = BaseCamera.Main.ScreenToWorld(Input.MousePosition);

        return gameObject;
    }
    
    private static void InitializeTree(GameObject gameObject) {
        gameObject.Initialize();
        foreach (var transform in gameObject.Transform) {
            InitializeTree(transform.GameObject);
        }
    }

    private static void DefaultEnt() {
        EntAdd("player", () => new Player());
        EntAdd("camera2d", () => {
            var gameObject = new GameObject("camera2d");
            var camera = gameObject.AddComponent<Camera2D>();
            camera.IsMain = true;
            camera.BackgroundColor = Raylib.RAYWHITE;
            return gameObject;
        });
        EntAdd("box_reactor", () =>
        {
            Box box = new Box(BuildingsList.List[0]);
            return box;
        });
        EntAdd("box_turret", () =>
        {
            Box box = new Box(BuildingsList.List[1]);
            return box;
        EntAdd("test_anim", () => {
            var gameObject = new GameObject("test_anim");
            var sprite = gameObject.AddComponent<SpriteRenderer2D>();
            sprite.Sprite = Data.GetSprite("textures/enemy/medium.png");
            var animator = gameObject.AddComponent<Animator2D>();
            animator.Animation = Data.GetAnimation("animations/enemy/medium.toml");
            animator.AnimationName = "idle";
            return gameObject;
        });
        EntAdd("enemy_big", () => {
            var gameObject = new GameObject("Enemy (Big)");
            var sprite = gameObject.AddComponent<SpriteRenderer2D>();
            sprite.Sprite = Data.GetSprite("textures/enemy/big.png");
            var animator = gameObject.AddComponent<Animator2D>();
            animator.Animation = Data.GetAnimation("animations/enemy/big.toml");
            animator.AnimationName = "idle";
            return gameObject;
        });
        EntAdd("enemy_small", () => {
            var gameObject = new GameObject("Enemy (small)");
            var sprite = gameObject.AddComponent<SpriteRenderer2D>();
            sprite.Sprite = Data.GetSprite("textures/enemy/small.png");
            var animator = gameObject.AddComponent<Animator2D>();
            animator.Animation = Data.GetAnimation("animations/enemy/small.toml");
            animator.AnimationName = "idle";
            var enemy = gameObject.AddComponent<SmallEnemy>();
            var ai = gameObject.AddComponent<SmallEnemyAi>();
            ai.Animator = animator;
            ai.Enemy = enemy;
            var rb = gameObject.AddComponent<Rigidbody2D>();
            var shape = gameObject.AddComponent<CircleCollider>();
            shape.Radius = 5f;
            rb.Type = BodyType.Dynamic;
            return gameObject;
        });
        EntAdd("enemy_medium", () => {
            var gameObject = new GameObject("Enemy (medium)");
            var sprite = gameObject.AddComponent<SpriteRenderer2D>();
            sprite.Sprite = Data.GetSprite("textures/enemy/medium.png");
            var animator = gameObject.AddComponent<Animator2D>();
            animator.Animation = Data.GetAnimation("animations/enemy/medium.toml");
            animator.AnimationName = "idle";
            return gameObject;
        });
    }
}