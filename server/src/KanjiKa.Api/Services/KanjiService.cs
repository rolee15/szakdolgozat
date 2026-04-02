using KanjiKa.Core.DTOs;
using KanjiKa.Core.DTOs.Kanji;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Kanji;
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
        var ids = kanjis.Select(k => k.Id).ToList();
        var proficiencies = await _kanjiRepository.GetProficienciesForUserAsync(userId, ids);

        return kanjis.Select(k => MapToDto(k, proficiencies)).ToList();
    }

    public async Task<KanjiDetailDto?> GetKanjiDetailAsync(string character, int userId)
    {
        var kanji = await _kanjiRepository.GetByCharacterAsync(character);
        if (kanji == null) return null;

        var proficiencies = await _kanjiRepository.GetProficienciesForUserAsync(userId, [kanji.Id]);
        proficiencies.TryGetValue(kanji.Id, out var prof);

        var srsStage = prof?.SrsStage ?? SrsStage.Locked;

        return new KanjiDetailDto
        {
            Character = kanji.Character,
            Meaning = kanji.Meaning,
            OnyomiReading = kanji.OnyomiReading,
            KunyomiReading = kanji.KunyomiReading,
            StrokeCount = kanji.StrokeCount,
            JlptLevel = kanji.JlptLevel,
            Grade = kanji.Grade,
            Proficiency = (int)srsStage,
            SrsStage = SrsIntervals.GetStageName(srsStage),
            Examples = kanji.Examples.Select(e => new KanjiExampleDto
            {
                Word = e.Word,
                Reading = e.Reading,
                Meaning = e.Meaning
            }).ToList()
        };
    }

    public async Task<PagedResult<KanjiDto>> GetKanjiPagedAsync(int? jlptLevel, int page, int pageSize, int userId)
    {
        var (items, totalCount) = await _kanjiRepository.GetPagedAsync(jlptLevel, page, pageSize);
        var ids = items.Select(k => k.Id).ToList();
        var proficiencies = await _kanjiRepository.GetProficienciesForUserAsync(userId, ids);

        return new PagedResult<KanjiDto>
        {
            Items = items.Select(k => MapToDto(k, proficiencies)).ToList(),
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    private static KanjiDto MapToDto(Kanji kanji, Dictionary<int, KanjiProficiency> proficiencies)
    {
        proficiencies.TryGetValue(kanji.Id, out var prof);
        var srsStage = prof?.SrsStage ?? SrsStage.Locked;

        return new KanjiDto
        {
            Character = kanji.Character,
            Meaning = kanji.Meaning,
            OnyomiReading = kanji.OnyomiReading,
            KunyomiReading = kanji.KunyomiReading,
            JlptLevel = kanji.JlptLevel,
            StrokeCount = kanji.StrokeCount,
            Proficiency = (int)srsStage,
            SrsStage = SrsIntervals.GetStageName(srsStage)
        };
    }
}
