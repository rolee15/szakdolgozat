using KanjiKa.Domain.Entities.Grammar;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Kanji;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Users;
using KanjiKa.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data.Seeders;

public class DevelopmentDataSeeder : ProductionDataSeeder
{
    private readonly IHashService _hashService;

    public DevelopmentDataSeeder(KanjiKaDbContext context, IHashService hashService)
        : base(context, hashService)
    {
        _hashService = hashService;
    }

    public override async Task SeedAsync()
    {
        await base.SeedAsync();
        await SeedTestUsers();
        await SeedGrammarProficiencies();
    }

    private async Task SeedTestUsers()
    {
        if (await Context.Users.AnyAsync(u => u.Username == "beginner@test.com"))
            return;

        List<Character> characters = await Context.Characters.ToListAsync();
        List<Kanji> kanjis = await Context.Kanjis
            .OrderBy(k => k.JlptLevel)
            .ThenBy(k => k.Id)
            .Take(50)
            .ToListAsync();

        // beginner — no proficiencies
        User beginner = CreateUser("beginner@test.com", "almafa123");
        await Context.Users.AddAsync(beginner);
        await Context.SaveChangesAsync();

        // midlearner — first 30 characters, mixed SRS stages Apprentice1–Guru1
        User midLearner = CreateUser("midlearner@test.com", "almafa123");
        await Context.Users.AddAsync(midLearner);
        await Context.SaveChangesAsync();

        var midStages = new[]
        {
            SrsStage.Apprentice1, SrsStage.Apprentice2, SrsStage.Apprentice3,
            SrsStage.Apprentice4, SrsStage.Guru1
        };
        List<Character> midCharacters = characters.Take(30).ToList();
        List<Proficiency> midProficiencies = midCharacters.Select((c, i) => {
            SrsStage stage = midStages[i % midStages.Length];
            return new Proficiency
            {
                UserId = midLearner.Id,
                CharacterId = c.Id,
                SrsStage = stage,
                NextReviewDate = SrsIntervals.GetNextReviewDate(stage)
            };
        }).ToList();
        List<LessonCompletion> midLessons = midCharacters.Select(c => new LessonCompletion
        {
            UserId = midLearner.Id,
            CharacterId = c.Id,
            CompletionDate = DateTimeOffset.UtcNow
        }).ToList();
        await Context.Proficiencies.AddRangeAsync(midProficiencies);
        await Context.LessonCompletions.AddRangeAsync(midLessons);
        await Context.SaveChangesAsync();

        // advanced — all characters at Guru1
        User advanced = CreateUser("advanced@test.com", "almafa123");
        await Context.Users.AddAsync(advanced);
        await Context.SaveChangesAsync();

        List<Proficiency> advancedProficiencies = characters.Select(c => new Proficiency
        {
            UserId = advanced.Id,
            CharacterId = c.Id,
            SrsStage = SrsStage.Guru1,
            NextReviewDate = SrsIntervals.GetNextReviewDate(SrsStage.Guru1)
        }).ToList();
        List<LessonCompletion> advancedLessons = characters.Select(c => new LessonCompletion
        {
            UserId = advanced.Id,
            CharacterId = c.Id,
            CompletionDate = DateTimeOffset.UtcNow
        }).ToList();
        await Context.Proficiencies.AddRangeAsync(advancedProficiencies);
        await Context.LessonCompletions.AddRangeAsync(advancedLessons);
        await Context.SaveChangesAsync();

        // reviewer — 40 kana + 10 kanji, all due for review
        User reviewer = CreateUser("reviewer@test.com", "almafa123");
        await Context.Users.AddAsync(reviewer);
        await Context.SaveChangesAsync();

        DateTimeOffset reviewerDueDate = DateTimeOffset.UtcNow.AddHours(-1);
        List<Character> reviewerCharacters = characters.Take(40).ToList();
        List<Kanji> reviewerKanjis = kanjis.Take(10).ToList();

        List<Proficiency> reviewerProficiencies = reviewerCharacters.Select(c => new Proficiency
        {
            UserId = reviewer.Id,
            CharacterId = c.Id,
            SrsStage = SrsStage.Apprentice1,
            NextReviewDate = reviewerDueDate
        }).ToList();
        List<LessonCompletion> reviewerLessons = reviewerCharacters.Select(c => new LessonCompletion
        {
            UserId = reviewer.Id,
            CharacterId = c.Id,
            CompletionDate = DateTimeOffset.UtcNow
        }).ToList();
        List<KanjiProficiency> reviewerKanjiProficiencies = reviewerKanjis.Select(k => new KanjiProficiency
        {
            UserId = reviewer.Id,
            KanjiId = k.Id,
            SrsStage = SrsStage.Apprentice1,
            NextReviewDate = reviewerDueDate
        }).ToList();

        await Context.Proficiencies.AddRangeAsync(reviewerProficiencies);
        await Context.LessonCompletions.AddRangeAsync(reviewerLessons);
        await Context.KanjiProficiencies.AddRangeAsync(reviewerKanjiProficiencies);
        await Context.SaveChangesAsync();
    }

    private async Task SeedGrammarProficiencies()
    {
        if (await Context.GrammarProficiencies.AnyAsync())
            return;

        List<int> grammarPointIds = await Context.GrammarPoints.Select(g => g.Id).ToListAsync();
        if (grammarPointIds.Count == 0)
            return;

        User? advanced = await Context.Users.FirstOrDefaultAsync(u => u.Username == "advanced@test.com");
        User? midLearner = await Context.Users.FirstOrDefaultAsync(u => u.Username == "midlearner@test.com");

        if (advanced != null)
        {
            List<GrammarProficiency> advancedProficiencies = grammarPointIds.Select(id => new GrammarProficiency
            {
                UserId = advanced.Id,
                GrammarPointId = id,
                CorrectCount = 3,
                AttemptCount = 3,
                LastPracticedAt = DateTimeOffset.UtcNow
            }).ToList();
            await Context.GrammarProficiencies.AddRangeAsync(advancedProficiencies);
        }

        if (midLearner != null)
        {
            List<int> first5Ids = grammarPointIds.Take(5).ToList();
            List<GrammarProficiency> midProficiencies = first5Ids.Select((id, i) => new GrammarProficiency
            {
                UserId = midLearner.Id,
                GrammarPointId = id,
                CorrectCount = i % 2 == 0 ? 1 : 2,
                AttemptCount = i % 2 == 0 ? 2 : 3,
                LastPracticedAt = DateTimeOffset.UtcNow
            }).ToList();
            await Context.GrammarProficiencies.AddRangeAsync(midProficiencies);
        }

        await Context.SaveChangesAsync();
    }

    private User CreateUser(string username, string password)
    {
        (byte[] hash, byte[] salt) = _hashService.Hash(password);
        return new User
        {
            Username = username,
            PasswordHash = hash,
            PasswordSalt = salt,
            Role = UserRole.User,
            IsActive = true
        };
    }
}
