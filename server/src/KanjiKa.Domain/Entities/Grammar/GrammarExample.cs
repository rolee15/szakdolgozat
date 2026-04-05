namespace KanjiKa.Domain.Entities.Grammar;

public class GrammarExample
{
    public int Id { get; set; }
    public required string Japanese { get; set; }
    public required string Reading { get; set; }
    public required string English { get; set; }
    public int GrammarPointId { get; set; }
    public GrammarPoint? GrammarPoint { get; set; }
}
