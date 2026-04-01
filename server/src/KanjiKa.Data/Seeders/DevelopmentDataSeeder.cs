using KanjiKa.Core;
using KanjiKa.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Seeders;

public class DevelopmentDataSeeder : ProductionDataSeeder
{
    public DevelopmentDataSeeder(KanjiKaDbContext context, IHashService hashService)
        : base(context, hashService) { }

    public override async Task SeedAsync()
    {
        await base.SeedAsync();
        await SeedTestUsers();
    }

    private async Task SeedTestUsers()
    {
        if (await Context.Users.AnyAsync(u => u.Username == "testuser1@kanjika.com"))
            return;

        var savedCharacters = await Context.Characters.ToListAsync();
        var users = TestData.GetUsers(savedCharacters);
        await Context.Users.AddRangeAsync(users);
        await Context.SaveChangesAsync();
    }
}
