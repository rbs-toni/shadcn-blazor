using BenchmarkDotNet.Attributes;
using System;

namespace ShadcnBlazor.Benchmarks.Builders;

[MemoryDiagnoser]
[RankColumn]
[ReturnValueValidator(failOnError: true)]
public class StyleBuilderBenchmark
{
    // ========== Light Load ==========

    [Benchmark(Baseline = true)]
    public string? V1Builder()
    {
        var builder = new StyleBuilderV1()
            .AddStyle("width", "100px")
            .AddStyle("height", "200px")
            .AddStyle("background-color", "red")
            .AddStyle("display", "block", when: true)
            .AddStyle("opacity", "0.5", "1", when: false);

        return builder.Build();
    }

    [Benchmark]
    public string? V2Builder()
    {
        var builder = new StyleBuilderV2()
            .AddStyle("width", "100px")
            .AddStyle("height", "200px")
            .AddStyle("background-color", "red")
            .AddStyle("display", "block", when: true)
            .AddStyle("opacity", "0.5", "1", when: false);

        return builder.Build();
    }

    [Benchmark]
    public string? V3Builder()
    {
        var builder = new StyleBuilderV3()
            .AddStyle("width", "100px")
            .AddStyle("height", "200px")
            .AddStyle("background-color", "red")
            .AddStyle("display", "block", when: true)
            .AddStyle("opacity", "0.5", "1", when: false);

        return builder.Build();
    }
}
