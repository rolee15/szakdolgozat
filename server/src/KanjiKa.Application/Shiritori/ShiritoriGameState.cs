namespace KanjiKa.Application.Shiritori;

public class ShiritoriGameState
{
    public HashSet<string> UsedWords { get; } = new();
    public char LastChar { get; set; }
    public bool IsOver { get; set; }
    public string? Winner { get; set; }
}
