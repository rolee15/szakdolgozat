using KanjiKa.Core.DTOs.Path;

namespace KanjiKa.Core.Interfaces;

public interface IPathService
{
    Task<List<LearningUnitDto>> GetPathAsync(int userId);
    Task<LearningUnitDetailDto?> GetUnitDetailAsync(int unitId, int userId);
    Task<UnitTestDto?> GetUnitTestAsync(int unitId, int userId);
    Task<UnitTestResultDto> SubmitTestAsync(int userId, int unitId, UnitSubmitDto submitDto);
}
