using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Entities.Common;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Users;

namespace KanjiKa.Application.Services;

public class KanaService : IKanaService
{
    private readonly IKanaRepository _repo;

    public KanaService(IKanaRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<KanaDto>> GetKanaCharacters(KanaType type, int userId)
    {
        User? user = await _repo.GetUserWithProficienciesAsync(userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found");
        }

        return _repo.GetCharactersByType(type)
            .Select(c => MapToKanaCharacterDto(c, user));
    }

    public async Task<KanaDetailDto> GetCharacterDetail(string character, KanaType type, int userId)
    {
        Character? kanaCharacter = await _repo.GetCharacterBySymbolAndTypeAsync(character, type);

        if (kanaCharacter == null)
        {
            throw new ArgumentException($"Character {character} not found");
        }

        User? user = await _repo.GetUserWithProficienciesAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found");
        }

        KanaProficiency? userProficiency = user.KanaProficiencies
            .FirstOrDefault(p => p.CharacterId == kanaCharacter.Id);
        int level = userProficiency?.Level ?? 0;
        SrsStage srsStage = userProficiency?.SrsStage ?? SrsStage.Locked;

        return new KanaDetailDto
        {
            Character = kanaCharacter.Symbol,
            Romanization = kanaCharacter.Romanization,
            Type = kanaCharacter.Type,
            Proficiency = level,
            SrsStage = (int)srsStage,
            SrsStageName = SrsIntervals.GetStageName(srsStage)
        };
    }

    public async Task<IEnumerable<KanaExampleDto>> GetExamples(string character, KanaType type)
    {
        Character? ch = await _repo.GetCharacterWithExamplesBySymbolAndTypeAsync(character, type);

        if (ch == null)
            throw new ArgumentException($"Character {character} not found");

        return ch.Examples.Select(e => new KanaExampleDto
        {
            Word = e.Word,
            Romanization = e.Romanization,
            Meaning = e.Meaning
        });
    }

    private static KanaDto MapToKanaCharacterDto(Character c, User user)
    {
        return new KanaDto
        {
            Character = c.Symbol,
            Romanization = c.Romanization,
            Type = c.Type,
            Proficiency = user.KanaProficiencies
                .FirstOrDefault(p => p.CharacterId == c.Id)?
                .Level ?? 0
        };
    }
}
