using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Application.DTOs.Kana;

public class KanaCharacterDetailDto
{
    public required string Character { get; set; }
    public required string Romanization { get; set; }
    public KanaType Type { get; set; }
    public int Proficiency { get; set; }
    public int SrsStage { get; set; }
    public required string SrsStageName { get; set; }
}
