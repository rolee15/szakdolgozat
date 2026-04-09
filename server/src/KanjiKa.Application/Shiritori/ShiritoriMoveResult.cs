namespace KanjiKa.Application.Shiritori;

public class ShiritoriMoveResult
{
    public bool IsValid { get; set; }
    public string? Reason { get; set; }
    public char? NextChar { get; set; }
    public string? ComputerWord { get; set; }
    public bool GameOver { get; set; }
    public string? Winner { get; set; }
}
