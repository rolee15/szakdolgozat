namespace KanjiKa.Application.DTOs.Kanji;

public class KanjiDetailDto
{
    public string Character { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;
    public string OnyomiReading { get; set; } = string.Empty;
    public string KunyomiReading { get; set; } = string.Empty;
    public int StrokeCount { get; set; }
    public int JlptLevel { get; set; }
    public int Grade { get; set; }
    public int Proficiency { get; set; }
    public string SrsStage { get; set; } = string.Empty;
    public List<KanjiExampleDto> Examples { get; set; } = [];
}
