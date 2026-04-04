using KanjiKa.Core.DTOs.Grammar;

namespace KanjiKa.Core.Interfaces;

public interface IGrammarService
{
    Task<List<GrammarPointDto>> GetGrammarPointsAsync(int userId);
    Task<GrammarPointDetailDto?> GetGrammarPointDetailAsync(int grammarPointId, int userId);
    Task<GrammarExerciseResultDto> CheckExerciseAsync(int userId, int grammarPointId, GrammarExerciseAnswerDto answer);
}
