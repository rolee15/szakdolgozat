namespace KanjiKa.Application.DTOs.Kanji;

public class KanjiDto
{
    public string Character { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string OnyomiReading { get; set; } = string.Empty;
    public string KunyomiReading { get; set; } = string.Empty;
    public int JlptLevel { get; set; }
    public int StrokeCount { get; set; }
    public int Proficiency { get; set; } // 0-100
    public string SrsStage { get; set; } = string.Empty;
}
