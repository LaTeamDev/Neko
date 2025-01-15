namespace Neko.Tools; 

public interface IPoppableConsoleToken : IConsoleToken {
    public bool Pop { get; set; }
}