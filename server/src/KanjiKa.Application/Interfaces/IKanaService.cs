using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Domain.Entities.Kana;

namespace KanjiKa.Application.Interfaces;

public interface IKanaService
{
    Task<IEnumerable<KanaDto>> GetKanaCharacters(KanaType type, int userId);

    Task<KanaDetailDto> GetCharacterDetail(string character, KanaType type, int userId);

    Task<IEnumerable<KanaExampleDto>> GetExamples(string character, KanaType type);
}
