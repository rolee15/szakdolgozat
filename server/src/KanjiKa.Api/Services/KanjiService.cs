using KanjiKa.Core.DTOs.Kanji;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Api.Services;

public class KanjiService : IKanjiService
{
    private readonly IKanjiRepository _kanjiRepository;

    public KanjiService(IKanjiRepository kanjiRepository)
    {
        _kanjiRepository = kanjiRepository;
    }

    public async Task<List<KanjiDto>> GetKanjiByLevelAsync(int jlptLevel, int userId)
    {
        var kanjis = await _kanjiRepository.GetByJlptLevelAsync(jlptLevel);
        return kanjis.Select(k => new KanjiDto
        {
            Character = k.Character,
            Meaning = k.Meaning,
            OnyomiReading = k.OnyomiReading,
            KunyomiReading = k.KunyomiReading,
            JlptLevel = k.JlptLevel,
            StrokeCount = k.StrokeCount,
            Proficiency = 0,
            SrsStage = "Locked"
        }).ToList();
    }

    public async Task<KanjiDetailDto?> GetKanjiDetailAsync(string character, int userId)
    {
        var kanji = await _kanjiRepository.GetByCharacterAsync(character);
        if (kanji == null) return null;

        return new KanjiDetailDto
        {
            Character = kanji.Character,
            Meaning = kanji.Meaning,
            OnyomiReading = kanji.OnyomiReading,
            KunyomiReading = kanji.KunyomiReading,
            StrokeCount = kanji.StrokeCount,
            JlptLevel = kanji.JlptLevel,
            Grade = kanji.Grade,
            Proficiency = 0,
            SrsStage = "Locked",
            Examples = kanji.Examples.Select(e => new KanjiExampleDto
            {
                Word = e.Word,
                Reading = e.Reading,
                Meaning = e.Meaning
            }).ToList()
        };
    }
}
