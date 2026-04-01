using KanjiKa.Core;
using KanjiKa.Core.Entities.Users;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Seeders;

public class ProductionDataSeeder : IDataSeeder
{
    protected readonly KanjiKaDbContext Context;
    private readonly IHashService _hashService;

    public ProductionDataSeeder(KanjiKaDbContext context, IHashService hashService)
    {
        Context = context;
        _hashService = hashService;
    }

    public virtual async Task SeedAsync()
    {
        await SeedCharacters();
        await SeedKanjis();
        await SeedAdminUser();
    }

    private async Task SeedCharacters()
    {
        if (await Context.Characters.AnyAsync())
            return;

        var characters = TestData.GetKanaCharacters();
        await Context.Characters.AddRangeAsync(characters);
        await Context.SaveChangesAsync();
    }

    private async Task SeedKanjis()
    {
        if (await Context.Kanjis.AnyAsync())
            return;

        var kanjis = Kanjidic2Parser.Parse();
        await Context.Kanjis.AddRangeAsync(kanjis);
        await Context.SaveChangesAsync();
    }

    private async Task SeedAdminUser()
    {
        if (await Context.Users.AnyAsync(u => u.Role == UserRole.Admin))
            return;

        var (hash, salt) = _hashService.Hash("Admin123!");
        var admin = new User
        {
            Username = "admin@kanjika.com",
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = UserRole.Admin,
            MustChangePassword = true
        };
        await Context.Users.AddAsync(admin);
        await Context.SaveChangesAsync();
    }
}
