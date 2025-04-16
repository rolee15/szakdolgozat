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
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:17.2")
        .WithDatabase("test")
        .WithUsername("admin")
        .WithPassword("admin")
        .Build();

    private DbConnection _dbConnection = null!;
    private Respawner _respawner = null!;

    public HttpClient HttpClient { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());

        HttpClient = CreateClient();

        using var scope = Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<KanjiKaDataSeeder>();
        await seeder.SeedAsync();

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

    public async Task ResetDatabaseAsync()
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
