namespace RocketPlaner.domain.Tools;

public record Error (string Text)
{
    public static Error None = new Error ("");
}
