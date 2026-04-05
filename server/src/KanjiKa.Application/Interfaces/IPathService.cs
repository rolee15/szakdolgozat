using KanjiKa.Application.DTOs.Path;

namespace KanjiKa.Application.Interfaces;

public interface IPathService
{
    Task<List<LearningUnitDto>> GetPathAsync(int userId);
    Task<LearningUnitDetailDto?> GetUnitDetailAsync(int unitId, int userId);
    Task<UnitTestDto?> GetUnitTestAsync(int unitId, int userId);
    Task<UnitTestResultDto> SubmitTestAsync(int userId, int unitId, UnitSubmitDto submitDto);
}
