using KanjiKa.Core.Entities.Kana;
using KanjiKa.Core.Entities.Learning;
using KanjiKa.Core.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data;

public class KanjiKaDbContext : DbContext
{

    public KanjiKaDbContext(DbContextOptions<KanjiKaDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Proficiency> Proficiencies { get; set; }
    public DbSet<LessonCompletion> LessonCompletions { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Example> Examples { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity => {
            entity.HasKey(user => user.Id);
            entity.HasMany(x => x.Proficiencies)
                .WithOne(proficiency => proficiency.User)
                .HasForeignKey(proficiency => proficiency.UserId);
            entity.HasMany(x => x.LessonCompletions)
                .WithOne(lessonCompletion => lessonCompletion.User)
                .HasForeignKey(lessonCompletion => lessonCompletion.UserId);
        });

        modelBuilder.Entity<Proficiency>(entity => {
            entity.HasKey(proficiency => new { proficiency.UserId, proficiency.CharacterId });
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
    }
}
