using KanjiKa.Core.Entities;
using KanjiKa.Core.Entities.Kana;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Data;

public class KanjiKaDbContext : DbContext
{

    public KanjiKaDbContext(DbContextOptions<KanjiKaDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<Proficiency> Proficiencies { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<Example> Examples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity => {
            entity.HasKey(user => user.Id);
            entity.HasMany(x => x.Proficiencies)
                .WithOne(proficiency => proficiency.User)
                .HasForeignKey(proficiency => proficiency.UserId);
        });

        modelBuilder.Entity<Proficiency>(entity => {
            entity.HasKey(proficiency => proficiency.Id);
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
