using System.Numerics;
using Box2D.NetStandard.Dynamics.Bodies;
using FlappyPegasus.GameStuff;
using FlappyPegasus.Gui;
using NekoLib.Core;
using NekoLib.Scenes;
using NekoRay;
using NekoRay.Physics2D;
using ZeroElectric.Vinculum;
using Camera2D = NekoRay.Camera2D;
using Timer = NekoRay.Timer;

namespace FlappyPegasus; 

public class GameScene : BaseScene {

    public OverlayScene OverlayScene;
    
    public override void Initialize() {
        this.CreateWorld();

        #region Camera
        var camera = new GameObject("Camera").AddComponent<Camera2D>();
        camera.BackgroundColor = Raylib.BLUE;
        camera.IsMain = true;
        camera.Zoom = 2f;
        #endregion

        #region Canvas
        var canvas = new GameObject("Canvas").AddComponent<Canvas>();
        var scoreText = canvas.GameObject.AddChild("Score Text").AddComponent<ShadowedText>();
        scoreText.Font = Data.GetFont("Data/texture/scorefont.png", "0123456789xm");
        scoreText.ShadowFont = Data.GetFont("Data/texture/scorefont_s.png", "0123456789xm");
        scoreText.Transform.LocalPosition = new Vector3(120f, 120f, 0f);
        scoreText.Transform.LocalScale = Vector3.One * 2;
        #endregion

        #region Player
        var playerAnimation = new GameObject("Player")
            .AddComponent<PlayerAnimation>();
        playerAnimation._spriteRenderer = playerAnimation.GameObject.AddChild("Sprite").AddComponent<SpriteRenderer2D>();
        var spriteTransform = playerAnimation._spriteRenderer.GameObject.Transform;
        //playerAnimation._spriteRenderer.GameObject.Transform.Parent = null;
        playerAnimation._spriteRenderer.FlipX = true;
        var collider = playerAnimation.GameObject.AddComponent<CircleCollider>();
        collider.Radius = 12f;
        
        var rigidbody = playerAnimation.GameObject.AddComponent<Rigidbody2D>();
        rigidbody.BodyType = BodyType.Dynamic;
        var player = playerAnimation.GameObject.AddComponent<PlayerMove>();
        player.Sprite = playerAnimation._spriteRenderer;
        player.rb2D = rigidbody;
        player.Animation = playerAnimation;
        player.Animation.FreezeFrame = 0;
        
        playerAnimation.AnimationFrames = AsepriteLoader.Load("Data/texture/bladhead.json");
        #endregion

        base.Initialize();

        OverlayScene = new PauseScene();
        SceneManager.LoadScene(OverlayScene, SceneLoadMode.Inclusive);
        SceneManager.SetSceneActive(this);
    }

    void OnKeyPressed(KeyboardKey key) {
        if (key != KeyboardKey.KEY_ENTER) return;
        OverlayScene.Toggle();
    }

    public override void Draw() {
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) base.Draw();
    }

    public override void Update() {
        if (!SceneManager.ActiveScene.GetType().IsAssignableTo(typeof(OverlayScene))) {
            this.GetWorld().Step(Timer.DeltaF, 8, 3);
            base.Update();
        }
    }
}