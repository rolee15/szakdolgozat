using KanjiKa.Domain.Entities.Grammar;
using KanjiKa.Domain.Entities.Kana;
using KanjiKa.Domain.Entities.Kanji;
using KanjiKa.Domain.Entities.Learning;
using KanjiKa.Domain.Entities.Path;
using KanjiKa.Domain.Entities.Reading;
using KanjiKa.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data;

public class KanjiKaDbContext : DbContext
{

    public KanjiKaDbContext(DbContextOptions<KanjiKaDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }
    public DbSet<KanaProficiency> KanaProficiencies { get; set; }
    public DbSet<LessonCompletion> LessonCompletions { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Example> Examples { get; set; }
    public DbSet<Kanji> Kanjis { get; set; }
    public DbSet<KanjiExample> KanjiExamples { get; set; }
    public DbSet<KanjiProficiency> KanjiProficiencies { get; set; }
    public DbSet<GrammarPoint> GrammarPoints { get; set; }
    public DbSet<GrammarExample> GrammarExamples { get; set; }
    public DbSet<GrammarExercise> GrammarExercises { get; set; }
    public DbSet<GrammarProficiency> GrammarProficiencies { get; set; }
    public DbSet<ReadingPassage> ReadingPassages { get; set; }
    public DbSet<ComprehensionQuestion> ComprehensionQuestions { get; set; }
    public DbSet<ReadingProficiency> ReadingProficiencies { get; set; }
    public DbSet<LearningUnit> LearningUnits { get; set; }
    public DbSet<UnitContent> UnitContents { get; set; }
    public DbSet<UnitTest> UnitTests { get; set; }
    public DbSet<UnitProgress> UnitProgresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity => {
            entity.HasKey(user => user.Id);
            entity.Property(u => u.Role)
                .HasConversion<int>()
                .HasDefaultValue(UserRole.User);
            entity.Property(u => u.MustChangePassword)
                .HasDefaultValue(false);
            entity.Property(u => u.IsActive)
                .HasDefaultValue(false);
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.ActivationToken)
                .HasFilter("activation_token IS NOT NULL");
            entity.HasMany(x => x.KanaProficiencies)
                .WithOne(proficiency => proficiency.User)
                .HasForeignKey(proficiency => proficiency.UserId);
            entity.HasMany(x => x.LessonCompletions)
                .WithOne(lessonCompletion => lessonCompletion.User)
                .HasForeignKey(lessonCompletion => lessonCompletion.UserId);
            entity.HasOne(x => x.Settings)
                .WithOne(s => s.User)
                .HasForeignKey<UserSettings>(s => s.UserId);
        });

        modelBuilder.Entity<UserSettings>(entity => {
            entity.HasKey(s => s.Id);
        });

        modelBuilder.Entity<KanaProficiency>(entity => {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => new { p.UserId, p.CharacterId }).IsUnique();
            entity.Property(p => p.SrsStage).HasConversion<int>();
            entity.Ignore(p => p.Level);
            entity.HasOne(p => p.Character)
                .WithMany()
                .HasForeignKey(p => p.CharacterId);
            entity.Ignore(p => p.Content);
        });

        modelBuilder.Entity<LessonCompletion>(entity => {
            entity.HasKey(lessonCompletion => new { lessonCompletion.UserId, lessonCompletion.CharacterId });
        });

        modelBuilder.Entity<Character>(entity => {
            entity.HasKey(character => character.Id);
            entity.HasMany(character => character.Examples)
                .WithOne(example => example.Character)
                .HasForeignKey(example => example.CharacterId);
        });

        modelBuilder.Entity<Example>(entity => {
            entity.HasKey(example => example.Id);
        });

        modelBuilder.Entity<Kanji>(entity => {
            entity.HasKey(k => k.Id);
            entity.HasMany(k => k.Examples)
                .WithOne(e => e.Kanji)
                .HasForeignKey(e => e.KanjiId);
        });

        modelBuilder.Entity<KanjiExample>(entity => {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<KanjiProficiency>(entity => {
            entity.HasKey(kp => kp.Id);
            entity.HasIndex(kp => new { kp.UserId, kp.KanjiId }).IsUnique();
            entity.Property(kp => kp.SrsStage).HasConversion<int>();
            entity.HasOne(kp => kp.Kanji)
                .WithMany()
                .HasForeignKey(kp => kp.KanjiId);
            entity.Ignore(kp => kp.Content);
        });

        modelBuilder.Entity<GrammarPoint>(entity => {
            entity.HasKey(g => g.Id);
            entity.HasMany(g => g.Examples)
                .WithOne(e => e.GrammarPoint)
                .HasForeignKey(e => e.GrammarPointId);
            entity.HasMany(g => g.Exercises)
                .WithOne(ex => ex.GrammarPoint)
                .HasForeignKey(ex => ex.GrammarPointId);
        });

        modelBuilder.Entity<GrammarExample>(entity => {
            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<GrammarExercise>(entity => {
            entity.HasKey(ex => ex.Id);
        });

        modelBuilder.Entity<GrammarProficiency>(entity => {
            entity.HasKey(gp => gp.Id);
            entity.HasIndex(gp => new { gp.UserId, gp.GrammarPointId }).IsUnique();
            entity.Property(gp => gp.SrsStage).HasConversion<int>();
            entity.HasOne(gp => gp.GrammarPoint)
                .WithMany()
                .HasForeignKey(gp => gp.GrammarPointId);
            entity.Ignore(gp => gp.Content);
        });

        modelBuilder.Entity<ReadingPassage>(entity => {
            entity.HasKey(p => p.Id);
            entity.HasMany(p => p.Questions)
                .WithOne(q => q.ReadingPassage)
                .HasForeignKey(q => q.ReadingPassageId);
        });

        modelBuilder.Entity<ComprehensionQuestion>(entity => {
            entity.HasKey(q => q.Id);
        });

        modelBuilder.Entity<ReadingProficiency>(entity => {
            entity.HasKey(rp => rp.Id);
            entity.HasIndex(rp => new { rp.UserId, rp.ReadingPassageId }).IsUnique();
            entity.Property(rp => rp.SrsStage).HasConversion<int>();
            entity.HasOne(rp => rp.ReadingPassage)
                .WithMany()
                .HasForeignKey(rp => rp.ReadingPassageId);
            entity.Ignore(rp => rp.Content);
        });

        modelBuilder.Entity<LearningUnit>(entity => {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Contents)
                .WithOne(c => c.LearningUnit)
                .HasForeignKey(c => c.LearningUnitId);
            entity.HasMany(u => u.Tests)
                .WithOne(t => t.LearningUnit)
                .HasForeignKey(t => t.LearningUnitId);
        });

        modelBuilder.Entity<UnitContent>(entity => {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.ContentType).HasConversion<int>();
        });

        modelBuilder.Entity<UnitTest>(entity => {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.ContentType).HasConversion<int>();
        });

        modelBuilder.Entity<UnitProgress>(entity => {
            entity.HasKey(up => up.Id);
            entity.HasIndex(up => new { up.UserId, up.LearningUnitId }).IsUnique();
        });
    }
}
