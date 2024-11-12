namespace RocketPlaner.Core.Tools;

public record Error(string Text)
{
    public static Error None = new Error(string.Empty);
}
