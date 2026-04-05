using KanjiKa.Core.Entities.Path;

namespace KanjiKa.Core.Interfaces;

public interface IPathRepository
{
    Task<List<LearningUnit>> GetAllUnitsAsync();
    Task<LearningUnit?> GetUnitByIdAsync(int id);
    Task<LearningUnit?> GetUnitWithTestAsync(int id);
    Task<Dictionary<int, UnitProgress>> GetProgressForUserAsync(int userId, List<int> unitIds);
    Task UpsertProgressAsync(UnitProgress progress);
    Task SaveChangesAsync();
    Task<string?> GetContentTitleAsync(ContentType contentType, int contentId);
}
