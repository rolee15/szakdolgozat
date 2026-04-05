using KanjiKa.Application.DTOs.Kana;
using KanjiKa.Application.Interfaces;
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

    public async Task<IEnumerable<KanaCharacterDto>> GetKanaCharacters(KanaType type, int userId)
    {
        var user = await _repo.GetUserWithProficienciesAsync(userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found");
        }

        return _repo.GetCharactersByType(type)
            .Select(c => MapToKanaCharacterDto(c, user));
    }

    public async Task<KanaCharacterDetailDto> GetCharacterDetail(string character, KanaType type, int userId)
    {
        var kanaCharacter = await _repo.GetCharacterBySymbolAndTypeAsync(character, type);

        if (kanaCharacter == null)
        {
            throw new ArgumentException($"Character {character} not found");
        }

        var user = await _repo.GetUserWithProficienciesAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found");
        }

        var userProficiency = user.Proficiencies
            .FirstOrDefault(p => p.CharacterId == kanaCharacter.Id);
        var level = userProficiency?.Level ?? 0;
        var srsStage = userProficiency?.SrsStage ?? SrsStage.Locked;

        return new KanaCharacterDetailDto
        {
            Character = kanaCharacter.Symbol,
            Romanization = kanaCharacter.Romanization,
            Type = kanaCharacter.Type,
            Proficiency = level,
            SrsStage = (int)srsStage,
            SrsStageName = SrsIntervals.GetStageName(srsStage)
        };
    }

    public async Task<IEnumerable<ExampleDto>> GetExamples(string character, KanaType type)
    {
        var ch = await _repo.GetCharacterWithExamplesBySymbolAndTypeAsync(character, type);

        if (ch == null)
            throw new ArgumentException($"Character {character} not found");

        return ch.Examples.Select(e => new ExampleDto
        {
            Word = e.Word,
            Romanization = e.Romanization,
            Meaning = e.Meaning
        });
    }

    private static KanaCharacterDto MapToKanaCharacterDto(Character c, User user)
    {
        return new KanaCharacterDto
        {
            Character = c.Symbol,
            Romanization = c.Romanization,
            Type = c.Type,
            Proficiency = user.Proficiencies
                .FirstOrDefault(p => p.CharacterId == c.Id)?
                .Level ?? 0
        };
    }
}
