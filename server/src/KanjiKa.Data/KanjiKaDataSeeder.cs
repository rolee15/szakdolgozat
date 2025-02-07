using KanjiKa.Core;

namespace KanjiKa.Data;

public class KanjiKaDataSeeder
{
    private readonly KanjiKaDbContext _context;

    public KanjiKaDataSeeder(KanjiKaDbContext context)
    {
        _context = context;
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
    }
}
