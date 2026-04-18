using KanjiKa.Application.DTOs.Grammar;

namespace KanjiKa.Application.Interfaces;

public interface IGrammarService
{
    Task<List<GrammarPointDto>> GetGrammarPointsAsync(int userId);

    Task<GrammarPointDetailDto?> GetGrammarPointDetailAsync(int grammarPointId, int userId);

    Task<GrammarExerciseResultDto> CheckExerciseAsync(int userId, int grammarPointId, GrammarExerciseAnswerDto answer);
}
