﻿using System.Drawing;
using System.Numerics;
using Box2D;
using NekoLib.Core;
using NekoRay;
using NekoRay.Physics2D;

namespace NeuroSama.Gameplay.MiniGame;

public class MiniGameScene : BaseScene {
    public World World;
    public override void Initialize() {
        World = this.CreateWorld(Vector2.Zero);
        var gameObject = new GameObject("Camera");
        var camera = gameObject.AddComponent<Camera2D>();
        camera.IsMain = true;
        camera.Zoom = 2f;

        var background = new GameObject("background").AddComponent<SpriteRenderer2D>();
        background.Sprite = Data.GetSprite("textures/minigame/ballpit.png");
        background.Transform.Position = new Vector3(-1f, -24f, 0f);

        var tutel = new GameObject("Tutel").AddComponent<SpriteRenderer2D>();
        tutel.Sprite = Data.GetSprite("textures/minigame/tutel.png");
        var player = tutel.GameObject.AddComponent<PlayerMiniGameController>();
        var playerCollider = player.GameObject.AddComponent<CircleCollider>();
        playerCollider.Radius = 9f;
        player.Collider = playerCollider;
        player.Rigidbody2D = player.GameObject.AddComponent<Rigidbody2D>();
        player.Rigidbody2D.Type = BodyType.Dynamic;

        var overlay = gameObject.AddChild("Overlay").AddComponent<SpriteRenderer2D>();
        overlay.Sprite = Data.GetSprite("textures/minigame/overlay_dev.png");
        overlay.Width = 1280/2;
        overlay.Height = 720/2;
        CreateCollider(new RectangleF(-90f, 50f, 64f, 64f));
        CreateCollider(new RectangleF(-93f, -65f, 137f/2, 32f/2f));
        CreateCollider(new RectangleF(90f, 16f, 137f/2, 32f/2f));
        CreateCollider(new RectangleF(0f, -145f, 320f, 64f));
        CreateCollider(new RectangleF(90f, 80f, 137f/2f, 32f/2f));
        CreateCollider(new RectangleF(90f, -57f, 137f/2f, 48f/2f));
        CreateCollider(new RectangleF(0f, -145f, 320f, 64f));
        
            
        base.Initialize();
    }

    private void CreateCollider(RectangleF rect) {
        var collider = new GameObject("bound").AddComponent<BoxCollider>();
        collider.Width = rect.Width;
        collider.Height = rect.Height;
        collider.Transform.Position = new Vector3(rect.X, rect.Y, 0f);
        var rb = collider.GameObject.AddComponent<Rigidbody2D>();
    }

    public override void FixedUpdate() {
        base.FixedUpdate();
        this.GetWorld().Step(Time.FixedDeltaF, 4);
    }
}