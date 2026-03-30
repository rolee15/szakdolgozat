namespace KanjiKa.Core.Entities.Kanji;

public class Kanji
{
    public int Id { get; set; }
    public required string Character { get; set; }
    public required string Meaning { get; set; }
    public required string OnyomiReading { get; set; }
    public required string KunyomiReading { get; set; }
    public int StrokeCount { get; set; }
    public int JlptLevel { get; set; }   // 5 = N5 (beginner), 1 = N1 (advanced)
    public int Grade { get; set; }       // school grade, 0 = not in school curriculum
    public List<KanjiExample> Examples { get; set; } = [];
}
