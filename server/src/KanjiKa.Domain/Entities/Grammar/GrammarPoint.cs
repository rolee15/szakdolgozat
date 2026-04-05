namespace KanjiKa.Domain.Entities.Grammar;

public class GrammarPoint
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Pattern { get; set; }
    public required string Explanation { get; set; }
    public int JlptLevel { get; set; }
    public int SortOrder { get; set; }
    public List<GrammarExample> Examples { get; set; } = [];
    public List<GrammarExercise> Exercises { get; set; } = [];
}
