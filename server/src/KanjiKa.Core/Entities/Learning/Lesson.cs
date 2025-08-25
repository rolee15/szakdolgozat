using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Entities.Learning;

public class Lesson
{
    public int Id { get; set; }
    public required Character Character { get; set; }
    public required string Answer { get; set; }
}
