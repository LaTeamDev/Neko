using ImGuiNET;
using NativeFileDialogSharp;
using NekoLib.Core;
using NekoRay;
using ZeroElectric.Vinculum;
using Image = NekoRay.Image;

namespace HotlineSPonyami.Tools;

public class EditorMenu : EditorWindow
{
    
    void DrawGui()
    {
        ImGui.BeginMainMenuBar();

        if (ImGui.BeginMenu("File"))
        {
            if (ImGui.MenuItem("Save"))
            {
                DialogResult result = Dialog.FileSave();
                if (result.IsOk)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(result.Path, FileMode.OpenOrCreate)))
                    {
                        Scene.Save(writer);
                    }
                }
            }
            if (ImGui.MenuItem("Open"))
            {
                DialogResult result = Dialog.FileOpen();
                if (result.IsOk)
                {
                    using (BinaryReader reader = new BinaryReader(File.Open(result.Path, FileMode.Open)))
                    {
                        Scene.Load(reader);
                    }
                }
            }
            ImGui.EndMenu();
        }

        if (ImGui.BeginMenu("Image"))
        {
            if (ImGui.MenuItem("Generate Tile Map Texture"))
            {
                Image finalImage = ImageGen.Color(UnpackedTextures.GetAllTextures().Count * 32, 32, Raylib.WHITE);
                for (int i = 0; i < UnpackedTextures.GetAllTextures().Count; i++)
                {
                    Image img = Image.FromTexture(UnpackedTextures.GetAllTextures()[i]);
                    Rectangle src = new Rectangle(0, 0, 32, 32);
                    Rectangle dest = new Rectangle(i * 32, 0, 32, 32);
                    finalImage.Draw(img, src, dest, Raylib.WHITE);
                }
                finalImage.Export("data/textures/texture_atlas.png");
            }

            if (ImGui.MenuItem("Export Map"))
            {
                DialogResult result = Dialog.FileSave();
                if (result.IsOk)
                {
                    Image image = Scene.Field.Export();
                    image.Export(result.Path);
                }
            }
            ImGui.EndMenu();
        }
        
        ImGui.EndMainMenuBar();
    }
}