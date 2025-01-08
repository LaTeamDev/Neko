namespace NekoRay;

public interface IAsset {
    public string Path { get; }
    public void Reload();
}