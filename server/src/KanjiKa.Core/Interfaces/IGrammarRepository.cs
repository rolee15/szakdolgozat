using KanjiKa.Core.Entities.Grammar;

namespace KanjiKa.Core.Interfaces;

public interface IGrammarRepository
{
    Task<List<GrammarPoint>> GetAllAsync();
    Task<GrammarPoint?> GetByIdAsync(int id);
    Task<GrammarExercise?> GetExerciseByIdAsync(int exerciseId);
    Task<Dictionary<int, GrammarProficiency>> GetProficienciesForUserAsync(int userId, List<int> grammarPointIds);
    Task<GrammarProficiency?> GetProficiencyAsync(int userId, int grammarPointId);
    Task AddProficiencyAsync(GrammarProficiency proficiency);
    Task SaveChangesAsync();
}
