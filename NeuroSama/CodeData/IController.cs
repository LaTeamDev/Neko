namespace NeuroSama.CodeData;

public interface IController
{
    public Player Player { get; }
    public void Update();
}