namespace KanjiKa.Application.DTOs.User;

public record UserSettingsDto(int DailyLessonLimit, int ReviewBatchSize, string JlptLevel);
