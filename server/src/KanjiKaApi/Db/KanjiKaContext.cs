using Microsoft.EntityFrameworkCore;

namespace KanjiKaApi.Db;

public class KanjiKaContext : DbContext
{
    public KanjiKaContext(DbContextOptions<KanjiKaContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) { modelBuilder.Entity<User>().ToTable("users"); }
}
