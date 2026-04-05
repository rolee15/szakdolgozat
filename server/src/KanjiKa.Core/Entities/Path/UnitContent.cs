namespace KanjiKa.Core.Entities.Path;

public class UnitContent
{
    public int Id { get; set; }
    public int LearningUnitId { get; set; }
    public LearningUnit? LearningUnit { get; set; }
    public ContentType ContentType { get; set; }
    public int ContentId { get; set; }
    public int SortOrder { get; set; }
}
