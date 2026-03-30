using KanjiKa.Core;
using KanjiKa.Core.Interfaces;

namespace KanjiKa.Data;

public class KanjiKaDataSeeder
{
    private readonly KanjiKaDbContext _context;
    private readonly IHashService _hashService;

    public KanjiKaDataSeeder(KanjiKaDbContext context, IHashService hashService)
    {
        _context = context;
        _hashService = hashService;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureDeletedAsync();
        await _context.Database.EnsureCreatedAsync();

        await Initialize();
        await _context.SaveChangesAsync();
    }

    private async Task Initialize()
    {
        if (!_context.Users.Any())
        {
            var users = TestData.GetUsers();
            await _context.Users.AddRangeAsync(users);
        }

        if (!_context.Characters.Any())
        {
            var characters = TestData.GetKanaCharacters();
            await _context.Characters.AddRangeAsync(characters);
        }

        if (!_context.Kanjis.Any())
        {
            var kanjis = TestData.GetKanjiData();
            await _context.Kanjis.AddRangeAsync(kanjis);
        }
    }
}
