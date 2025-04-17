using System.Data.Common;
using KanjiKa.Api;
using KanjiKa.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace KanjiKa.IntegrationTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private DbConnection _dbConnection = null!;
    private PostgreSqlContainer _dbContainer = null!;
    private Respawner _respawner = null!;

    public HttpClient HttpClient { get; private set; } = null!;

    // Do not change the order of initialization!
    public async Task InitializeAsync()
    {
        _dbContainer = new PostgreSqlBuilder()
            .WithImage("postgres:17.2")
            .WithDatabase("test")
            .WithUsername("admin")
            .WithPassword("admin")
            .Build();
        await _dbContainer.StartAsync();

        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        HttpClient = CreateClient();

        await SeedDatabaseAsync();

        await _dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
        {
            SchemasToInclude = ["public"],
            DbAdapter = DbAdapter.Postgres
        });
    }

    public new async Task DisposeAsync()
    {
        await _dbContainer.DisposeAsync();
        await _dbConnection.DisposeAsync();
    }

    public async Task SeedDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<KanjiKaDataSeeder>();
        await seeder.SeedAsync();
    }

    public async Task TearDownDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services => {
            services.Remove(services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<KanjiKaDbContext>))!);
            services.Remove(services.SingleOrDefault(service => service.ServiceType == typeof(KanjiKaDbContext))!);
            services.AddDbContext<KanjiKaDbContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }
}
