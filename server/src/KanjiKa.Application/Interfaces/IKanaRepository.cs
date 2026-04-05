using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Interfaces;

public interface IKanaRepository
{
    Task<User?> GetUserWithProficienciesAsync(int userId);

    Task<Character?> GetCharacterBySymbolAndTypeAsync(string symbol, KanaType type);

    Task<Character?> GetCharacterWithExamplesBySymbolAndTypeAsync(string symbol, KanaType type);

    IEnumerable<Character> GetCharactersByType(KanaType type);
}
