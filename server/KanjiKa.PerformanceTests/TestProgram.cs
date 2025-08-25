using BenchmarkDotNet.Running;

namespace KanjiKa.PerformanceTest;

public static class TestProgram
{
    public static void Main(string[] args)
    {
        // Run the benchmark tests
        BenchmarkRunner.Run<IntegratedPerformanceBenchmark>();
    }
}
