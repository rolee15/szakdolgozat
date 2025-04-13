using BenchmarkDotNet.Attributes;
using KanjiKa.Api.Services;
using KanjiKa.Core.Interfaces;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace KanjiKa.PerformanceTest;

public class LessonServiceBenchmark
{
    private ILessonService? _lessonService;
    private ServiceProvider? _serviceProvider;

    [GlobalSetup]
    public async Task Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<KanjiKaDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=kanjika_db;Port=5432;Username=admin;Password=12345"));
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<KanjiKaDataSeeder>();

        _serviceProvider = services.BuildServiceProvider();
        _lessonService = _serviceProvider.GetRequiredService<ILessonService>();
        var seeder = _serviceProvider.GetRequiredService<KanjiKaDataSeeder>();
        await seeder.SeedAsync();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _serviceProvider?.Dispose();
    }

    [Benchmark]
    public async Task<int> GetLessonsCount()
    {
        var result = await _lessonService!.GetLessonsCountAsync(1);
        return result.Count;
    }

    [Benchmark]
    public async Task<int> GetLessonReviewsCount()
    {
        var result = await _lessonService!.GetLessonReviewsCountAsync(1);
        return result.Count;
    }
}
