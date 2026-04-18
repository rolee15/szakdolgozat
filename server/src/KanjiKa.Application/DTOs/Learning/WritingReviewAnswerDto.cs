using System.ComponentModel.DataAnnotations;

namespace KanjiKa.Application.DTOs.Learning;

public class WritingReviewAnswerDto
{
    public int CharacterId { get; set; }

    [MinLength(1)]
    public required string TypedCharacter { get; set; }
}
