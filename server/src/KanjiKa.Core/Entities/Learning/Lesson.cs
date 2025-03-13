using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Entities.Learning;

public class LessonItem
{
    public int Id { get; set; }
    public Character Character { get; set; }
    public string Answer { get; set; }
}
