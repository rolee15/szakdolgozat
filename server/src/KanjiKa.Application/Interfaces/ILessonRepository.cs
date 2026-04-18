using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Interfaces;

public interface ILessonRepository
{
    Task<User?> GetUserAsync(int userId);

    Task<User?> GetUserWithProficienciesAsync(int userId);

    Task<int> CountLessonsCompletedTodayAsync(int userId);

    Task<List<Character>> GetNewCharactersAsync(List<KanaProficiency> proficiencies);

    Task<int> CountNewCharactersAsync(List<int> learnedCharacterIds);

    Task<Character?> GetCharacterByIdAsync(int characterId);

    Task<Character?> GetCharacterBySymbolAsync(string symbol);

    Task<KanaProficiency?> GetProficiencyAsync(int userId, int characterId);

    Task AddProficiencyAsync(KanaProficiency proficiency);

    Task AddLessonCompletionAsync(LessonCompletion completion);

    Task<List<LessonCompletion>> GetLessonCompletionsByUserAsync(int userId);

    Task<List<KanaProficiency>> GetDueReviewsAsync(int userId);

    Task SaveChangesAsync();
}
