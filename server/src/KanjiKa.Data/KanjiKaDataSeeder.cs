using KanjiKa.Application;
using KanjiKa.Application.Interfaces;
using KanjiKa.Domain.Interfaces;

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
    }

    private async Task Initialize()
    {
        if (!_context.Characters.Any())
        {
            var characters = TestData.GetKanaCharacters();
            await _context.Characters.AddRangeAsync(characters);
        }

        if (!_context.Kanjis.Any())
        {
            var kanjis = Kanjidic2Parser.Parse();
            await _context.Kanjis.AddRangeAsync(kanjis);
        }

        await _context.SaveChangesAsync();

        if (!_context.Users.Any())
        {
            var users = TestData.GetUsers();
            await _context.Users.AddRangeAsync(users);
        }

        await _context.SaveChangesAsync();
    }
}
