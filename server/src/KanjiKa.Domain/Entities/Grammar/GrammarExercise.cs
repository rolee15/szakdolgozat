namespace KanjiKa.Domain.Entities.Grammar;

public class GrammarExercise
{
    public int Id { get; set; }
    public required string Sentence { get; set; } // e.g. "これ___ペンです。"
    public required string CorrectAnswer { get; set; }
    public required string Distractor1 { get; set; }
    public required string Distractor2 { get; set; }
    public required string Distractor3 { get; set; }
    public int GrammarPointId { get; set; }
    public GrammarPoint? GrammarPoint { get; set; }
}
