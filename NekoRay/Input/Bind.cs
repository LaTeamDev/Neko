namespace NekoRay;

public abstract class Bind(string name) {
    public string Name = name;
    
    public abstract bool Down { get; }
    public abstract bool Pressed { get; }
    public abstract bool Released { get; }
    public abstract bool Up { get; }
}

public abstract class Bind<T>(string name, T button) : Bind(name) {
    public readonly T Button = button;
}