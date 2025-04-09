using BenchmarkDotNet.Attributes;
using KanjiKa.Api.Services;
using KanjiKa.Core.Interfaces;
using KanjiKa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

[MemoryDiagnoser]
public class LessonServiceBenchmark
{
    private ILessonService _lessonService;
    private IServiceProvider _serviceProvider;

    [GlobalSetup]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<KanjiKaDbContext>(options =>
            options.UseNpgsql("Host=localhost;Database=KanjiKaDb;Port=5432;Username=kanjika_admin;Password=12345"));
        services.AddScoped<ILessonService, LessonService>();

        _serviceProvider = services.BuildServiceProvider();
        _lessonService = _serviceProvider.GetRequiredService<ILessonService>();
    }
}
