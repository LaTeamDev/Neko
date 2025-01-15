namespace Neko.Tools;

public class GroupAttribute(string text) : Attribute {
    public readonly string Text = text;
}