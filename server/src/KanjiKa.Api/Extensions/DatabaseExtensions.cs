using KanjiKa.Application.Interfaces;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;

namespace KanjiKa.Api.Extensions;

public static class DatabaseExtensions
{
    public async static Task<WebApplication> InitialiseDatabaseAsync(this WebApplication app)
    {
        using IServiceScope scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<KanjiKaDbContext>();

        if (app.Environment.IsDevelopment())
        {
            // Terminate other connections to avoid EnsureDeletedAsync timeout
            try
            {
                await db.Database.ExecuteSqlRawAsync(
                    "SELECT pg_terminate_backend(pg_stat_activity.pid) " +
                    "FROM pg_stat_activity " +
                    "WHERE pg_stat_activity.datname = current_database() " +
                    "AND pid <> pg_backend_pid();");
            }
            catch
            {
                // Database may not exist yet on first run
            }

            await db.Database.EnsureDeletedAsync();
        }

        await db.Database.MigrateAsync();

        var seeder = scope.ServiceProvider.GetRequiredService<IDataSeeder>();
        await seeder.SeedAsync();

        return app;
    }
}
