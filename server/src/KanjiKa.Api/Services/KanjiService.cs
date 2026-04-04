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

    public async Task<int> GetDueReviewsCountAsync(int userId)
    {
        var reviews = await _kanjiRepository.GetDueReviewsAsync(userId);
        return reviews.Count;
    }

    public async Task<List<KanjiReviewDto>> GetDueReviewsAsync(int userId)
    {
        var reviews = await _kanjiRepository.GetDueReviewsAsync(userId);
        return reviews.Select(kp => new KanjiReviewDto
        {
            KanjiId = kp.KanjiId,
            Character = kp.Kanji!.Character,
            Meaning = kp.Kanji.Meaning,
            OnyomiReading = kp.Kanji.OnyomiReading,
            KunyomiReading = kp.Kanji.KunyomiReading
        }).ToList();
    }

    public async Task<KanjiReviewResultDto> CheckReviewAsync(int userId, KanjiReviewAnswerDto answer)
    {
        var proficiency = await _kanjiRepository.GetProficiencyAsync(userId, answer.KanjiId);
        if (proficiency == null)
            throw new KeyNotFoundException($"Proficiency for kanji {answer.KanjiId} not found for user {userId}.");

        if (answer.IsCorrect)
            proficiency.AnswerCorrectly();
        else
            proficiency.AnswerIncorrectly();

        await _kanjiRepository.SaveChangesAsync();

        return new KanjiReviewResultDto
        {
            IsCorrect = answer.IsCorrect,
            SrsStage = (int)proficiency.SrsStage,
            SrsStageName = SrsIntervals.GetStageName(proficiency.SrsStage),
            NextReviewDate = proficiency.NextReviewDate
        };
    }

    public async Task<KanjiProficiency> LearnKanjiAsync(int userId, int kanjiId)
    {
        var existing = await _kanjiRepository.GetProficiencyAsync(userId, kanjiId);
        if (existing != null)
            throw new InvalidOperationException($"User {userId} has already learned kanji {kanjiId}.");

        var proficiency = new KanjiProficiency
        {
            UserId = userId,
            KanjiId = kanjiId,
            SrsStage = SrsStage.Apprentice1,
            NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage.Apprentice1)
        };

        await _kanjiRepository.AddProficiencyAsync(proficiency);
        await _kanjiRepository.SaveChangesAsync();

        return proficiency;
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
