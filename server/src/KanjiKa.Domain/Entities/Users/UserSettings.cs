namespace KanjiKa.Domain.Entities.Users;

public class UserSettings
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int DailyLessonLimit { get; set; } = 10;
    public int ReviewBatchSize { get; set; } = 50;
    public string JlptLevel { get; set; } = "N5";
}
