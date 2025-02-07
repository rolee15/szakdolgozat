using KanjiKa.Core.Dtos;
using KanjiKa.Core.Entities.Kana;

namespace KanjiKa.Core.Interfaces;

public interface IKanaService
{
    Task<IEnumerable<KanaCharacterDto>> GetKanaCharacters(KanaType type, int userId);

    Task<KanaCharacterDetailDto> GetCharacterDetail(string character, KanaType type, int userId);

    Task<IEnumerable<ExampleDto>> GetExamples(string character, KanaType type);
}
