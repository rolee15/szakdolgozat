using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Application.Interfaces;

public interface IKanaService
{
    Task<IEnumerable<KanaCharacterDto>> GetKanaCharacters(KanaType type, int userId);

    Task<KanaCharacterDetailDto> GetCharacterDetail(string character, KanaType type, int userId);

    Task<IEnumerable<ExampleDto>> GetExamples(string character, KanaType type);
}
