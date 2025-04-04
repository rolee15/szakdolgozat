using KanjiKa.Core.DTOs.Kana;
using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Api.Services;

public class KanaService : IKanaService
{
    private readonly KanjiKaDbContext _db;

    public KanaService(KanjiKaDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<KanaCharacterDto>> GetKanaCharacters(KanaType type, int userId)
    {
        var user = await _db.Users
            .Include(x => x.Proficiencies)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found");
        }

        return _db.Characters
            .Where(c => c.Type == type)
            .Select(c => MapToKanaCharacterDto(c, user));
    }

    public async Task<KanaCharacterDetailDto> GetCharacterDetail(string character, KanaType type, int userId)
    {
        var kanaCharacter = await _db.Characters.FirstOrDefaultAsync(c =>
            c.Symbol == character && c.Type == type);

        if (kanaCharacter == null)
        {
            throw new ArgumentException($"Character {character} not found");
        }

        var user = await _db.Users
            .Include(x => x.Proficiencies)
            .FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
        {
            throw new ArgumentException($"User with id {userId} not found");
        }

        var userProficiency = user.Proficiencies
            .FirstOrDefault(p => p.CharacterId == kanaCharacter.Id);
        var level = userProficiency?.Level ?? 0;

        return new KanaCharacterDetailDto
        {
            Character = kanaCharacter.Symbol,
            Romanization = kanaCharacter.Romanization,
            Type = kanaCharacter.Type,
            Proficiency = level
        };
    }

    public async Task<IEnumerable<ExampleDto>> GetExamples(string character, KanaType type)
    {
        var ch = await _db.Characters
            .Include(x => x.Examples)
            .FirstOrDefaultAsync(c => c.Symbol == character && c.Type == type);

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
