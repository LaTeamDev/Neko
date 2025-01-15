using System.Drawing;
using System.Numerics;
using Neko.Physics2D;

namespace Neko;

public class Camera2D : BaseCamera {

    //TODO:
    public float Zoom { get; set; } /*{
        get => _camera.zoom;
        set => _camera.zoom = value;
    }*/

    public Color BackgroundColor = Color.Black;

    void LateUpdate() {
        //_camera.target = new Vector2(Transform.Position.X, Transform.Position.Y);
        //_camera.offset = new Vector2(RenderWidth / 2f, RenderHeight / 2f);
        //_camera.rotation = Transform.Rotation.YawPitchRollAsVector3().Z;
        if (Zoom <= 0f) Zoom = 1f;
    }

    void Draw() {
        CurrentCamera = this;
        using (RenderTexture.Attach()) {
            //RlGl.rlEnableDepthTest();
            //Raylib.BeginMode2D(_camera);
            //Raylib.ClearBackground(BackgroundColor);
            var gameObjects = new List<GameObject>();
            gameObjects.AddRange(GameObject.Scene.GameObjects);
            gameObjects.Sort(Comparison);
            foreach (var gameObject in gameObjects) {
                var tags = gameObject.AllTags;
                if (tags.Contains("Skip2D") || tags.Contains("SkipRender")) continue;
                gameObject.SendMessage("Render");
            }
            if (DebugDraw.ConvarDraw && GameObject.Scene.TryGetWorld(out var world)) world?.Draw(DebugDraw.Instance);
            //Raylib.EndMode2D();
            //RlGl.rlDisableDepthTest();
        }
        CurrentCamera = null;
    }

    private int Comparison(GameObject gameObject, GameObject gameObject2) {
        if (gameObject.Transform.Position.Z > gameObject2.Transform.Position.Z) 
            return 1;
        if (gameObject.Transform.Position.Z < gameObject2.Transform.Position.Z) 
            return -1;
        return 0;
    }

    public override void Dispose() {
        base.Dispose();
        
        RenderTexture.Dispose();
    }

    public override Vector2 WorldToScreen(Vector3 position) {
        throw new NotImplementedException();
        //return Raylib.GetWorldToScreen2D(position.ToVector2(), _camera);
    }
    
    public override Vector3 ScreenToWorld(Vector2 position) {
        throw new NotImplementedException();
        //return new Vector3(Raylib.GetScreenToWorld2D(position, _camera), 0f);
    }
}