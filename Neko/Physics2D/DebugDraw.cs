using System.Numerics;
using NekoLib.Extra;
using Box2D;
using Box2D.Interop;
using Transform = Box2D.Transform;

namespace Neko.Physics2D; 

public class DebugDraw : IDebugDraw {
    private static DebugDraw? _instance;
    public static DebugDraw Instance {
        get {
            if (_instance is null) _instance = new DebugDraw();
            return _instance;
        }
    }
    [ConVariable("phys_draw")]
    [ConTags("cheat")]
    public static bool ConvarDraw { get; set; }

    [ConVariable("phys_drawbounds")]
    [ConTags("cheat")]
    public static bool ConvarUseDrawingBounds {
        get => Instance.UseDrawingBounds;
        set => Instance.UseDrawingBounds = value;
    }

    [ConVariable("phys_drawshapes")]
    [ConTags("cheat")]
    public static bool ConvarDrawShapes {
        get => Instance.DrawShapes;
        set => Instance.DrawShapes = value;
    }

    [ConVariable("phys_drawcontacts")]
    [ConTags("cheat")]
    public static bool ConvarDrawContacts {
        get => Instance.DrawContacts;
        set => Instance.DrawContacts = value;
    }

    [ConVariable("phys_drawjoints")]
    [ConTags("cheat")]
    public static bool ConvarDrawJoints {
        get => Instance.DrawJoints;
        set => Instance.DrawJoints = value;
    }

    [ConVariable("phys_drawmass")]
    [ConTags("cheat")]
    public static bool ConvarDrawMass {
        get => Instance.DrawMass;
        set => Instance.DrawMass = value;
    }

    [ConVariable("phys_drawcontactimpulses")]
    [ConTags("cheat")]
    public static bool ConvarDrawContactImpulses {
        get => Instance.DrawContactImpulses;
        set => Instance.DrawContactImpulses = value;
    }

    [ConVariable("phys_drawcontactnormals")]
    [ConTags("cheat")]
    public static bool ConvarDrawContactNormals {
        get => Instance.DrawContactNormals;
        set => Instance.DrawContactNormals = value;
    }

    [ConVariable("phys_drawfrictionimpulses")]
    [ConTags("cheat")]
    public static bool ConvarDrawFrictionImpulses {
        get => Instance.DrawFrictionImpulses;
        set => Instance.DrawFrictionImpulses = value;
    }

    [ConVariable("phys_drawgraphcolors")]
    [ConTags("cheat")]
    public static bool ConvarDrawGraphColors {
        get => Instance.DrawGraphColors;
        set => Instance.DrawGraphColors = value;
    }

    [ConVariable("phys_drawjointextras")]
    [ConTags("cheat")]
    public static bool ConvarDrawJointExtras {
        get => Instance.DrawJointExtras;
        set => Instance.DrawJointExtras = value;
    }

    [ConVariable("phys_drawaabbs")]
    [ConTags("cheat")]
    public static bool ConvarDrawAABBs {
        get => Instance.DrawAABBs;
        set => Instance.DrawAABBs = value;
    }

    private DebugDraw() {
        /* TODO:
        DrawString = DebugDrawDrawString;
        DrawCircle = DebugDrawDrawCircle;
        DrawCapsule = DebugDrawDrawCapsule;
        DrawPoint = DebugDrawDrawPoint;
        DrawPolygon = DebugDrawDrawPolygon;
        DrawSegment = DebugDrawDrawSegment;
        DrawTransform = DebugDrawDrawTransform;
        DrawSolidCapsule = DebugDrawDrawSolidCapsule;
        DrawSolidCircle = DebugDrawDrawSolidCircle;
        DrawSolidPolygon = DebugDrawDrawSolidPolygon;*/
        UseDrawingBounds = false;
        DrawShapes = true;
        DrawContacts = true;
        DrawJoints = true;
        DrawMass = true;
        DrawContactImpulses = true;
        DrawContactNormals = true;
        DrawFrictionImpulses = true;
        DrawGraphColors = true;
        DrawJointExtras = true;
        DrawAABBs = true;
    }
    
    public unsafe void Polygon(Span<Vector2> pos, b2HexColor color) {
        //idk how to draw polygons in raylib honestly
        //fixed(Vector2* ptr = pos)
        //    Raylib.DrawLineStrip(ptr, pos.Length, color.ToRaylib());
    }

    public unsafe void SolidPolygon(Transform transform, Span<Vector2> pos, b2HexColor color) {
        //fixed(Vector2* ptr = pos)
        //    Raylib.DrawLineStrip(ptr, pos.Length, color.ToRaylib());
    }
    public void Circle(Vector2 pos, float radius, b2HexColor color) {
        //Raylib.DrawCircleLinesV(pos, radius, color.ToRaylib());
    }
    public void SolidCircle(Transform transform, float radius, b2HexColor color) {
        //Raylib.DrawCircleV(transform.Position, radius, color.ToRaylib());
    }
    public void Capsule(Vector2 start, Vector2 end, float radius, b2HexColor color) {
        //Raylib.DrawCircleLinesV(start, radius, color.ToRaylib());
        //Raylib.DrawCircleLinesV(start, radius, color.ToRaylib());
        //var normal = Vector2.Normalize(end - start);
        //var perp = new Vector2(normal.Y, -normal.X);
        //Raylib.DrawLineV(start + perp * radius, end + perp * radius, color.ToRaylib());
        //Raylib.DrawLineV(start - perp * radius, end - perp * radius, color.ToRaylib());
    }
    public void SolidCapsule(Vector2 start, Vector2 end, float radius, b2HexColor color) {
        //Raylib.DrawCircleV(start, radius, color.ToRaylib());
        //Raylib.DrawCircleV(start, radius, color.ToRaylib());
        //Raylib.DrawLineEx(start, end, radius, color.ToRaylib());
    }
    public void Segment(Vector2 start, Vector2 end, b2HexColor color) {
        //Raylib.DrawLineV(start, end, color.ToRaylib());
    }
    public void Transform(Transform transform) { }
    public void Point(Vector2 pos, float radius, b2HexColor color) {
        //Raylib.DrawRectangleV(pos - Vector2.One * radius / 2, Vector2.One * radius, color.ToRaylib());
    }
    public void String(Vector2 pos, string str) {
        //Raylib.DrawText(str, pos.X, pos.Y, 10f, Raylib.GOLD);
    }
    public AABB DrawingBounds { get; set; }
    public bool UseDrawingBounds { get; set; }
    public bool DrawShapes { get; set; }
    public bool DrawJoints { get; set; }
    public bool DrawJointExtras { get; set; }
    public bool DrawAABBs { get; set; }
    public bool DrawMass { get; set; }
    public bool DrawContacts { get; set; }
    public bool DrawGraphColors { get; set; }
    public bool DrawContactNormals { get; set; }
    public bool DrawContactImpulses { get; set; }
    public bool DrawFrictionImpulses { get; set; }
}