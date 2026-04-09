using System.ComponentModel.DataAnnotations;

namespace KanjiKa.Application.DTOs.User;

public record UpdateUserSettingsDto
{
    [Range(1, 50)] public int DailyLessonLimit { get; init; }
    [Range(10, 200)] public int ReviewBatchSize { get; init; }
    [RegularExpression("N[1-5]")] public string JlptLevel { get; init; } = "N5";
}
