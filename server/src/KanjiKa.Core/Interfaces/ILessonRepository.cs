using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;

namespace KanjiKa.Core.Interfaces;

public interface ILessonRepository
{
    Task<User?> GetUserWithProficienciesAsync(int userId);

    Task<int> CountLessonsCompletedTodayAsync(int userId, DateTimeOffset todayUtcDate);

    Task<List<Character>> GetAllCharactersAsync();

    Task<Character?> GetCharacterByIdAsync(int characterId);

    Task<Character?> GetCharacterBySymbolAsync(string symbol);

    Task<Proficiency?> GetProficiencyAsync(int userId, int characterId);

    Task AddProficiencyAsync(Proficiency proficiency);

    Task AddLessonCompletionAsync(LessonCompletion completion);

    Task<List<LessonCompletion>> GetLessonCompletionsByUserAsync(int userId);

    Task SaveChangesAsync();
}
