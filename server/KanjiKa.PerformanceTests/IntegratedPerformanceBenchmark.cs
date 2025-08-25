using System.Net.Http.Json;
using BenchmarkDotNet.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace KanjiKa.PerformanceTest;

[MemoryDiagnoser]
public class IntegratedPerformanceBenchmark
{
    private HttpClient? _client;

    [GlobalSetup]
    public void Setup()
    {
        var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Benchmark");
                builder.ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                });
            });

        _client = factory.CreateClient();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
    }

    [Benchmark]
    public async Task<int> GetLessonsCountThroughApi()
    {
        var response = await _client!.GetFromJsonAsync<LessonsCountResponse>("/api/lessons/count/1");
        return response!.Count;
    }

    [Benchmark]
    public async Task<int> GetLessonReviewsCountThroughApi()
    {
        var response = await _client!.GetFromJsonAsync<LessonReviewsCountResponse>("/api/lessons/reviews/count/1");
        return response!.Count;
    }

    // Simple DTOs to match your API responses
    private class LessonsCountResponse
    {
        public int Count { get; set; }
    }

    private class LessonReviewsCountResponse
    {
        public int Count { get; set; }
    }
}