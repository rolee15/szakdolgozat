using KanjiKa.Core.Dtos;
using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Interfaces;

public interface IKanaService
{
    Task<IEnumerable<KanaCharacterDto>> GetKanaCharacters(KanaType type, string userId);
    Task<KanaCharacterDetailDto> GetCharacterDetail(string character, KanaType type, string userId);
    Task<IEnumerable<string>> GetExamples(string character, KanaType type);
}