namespace KanjiKa.Core.DTOs.Grammar;

public class GrammarExerciseDto
{
    public int Id { get; set; }
    public string Sentence { get; set; } = string.Empty;
    public List<string> Options { get; set; } = [];  // shuffled: correct + 3 distractors
}
