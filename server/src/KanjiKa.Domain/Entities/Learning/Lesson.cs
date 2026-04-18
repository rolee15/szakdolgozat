using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Domain.Entities.Learning;

public class Lesson
{
    public int Id { get; set; }
    public required Character Character { get; set; }
    public required string Answer { get; set; }
}
