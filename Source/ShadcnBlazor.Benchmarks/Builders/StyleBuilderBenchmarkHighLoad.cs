using BenchmarkDotNet.Attributes;
using System;

namespace ShadcnBlazor.Benchmarks.Builders;

[MemoryDiagnoser]
[RankColumn]
[ReturnValueValidator(failOnError: true)]
public class StyleBuilderBenchmarkHighLoad
{
    private const int HighLoad = 512;
    // ========== High Load ==========

    [Benchmark]
    public string? V1Builder_HighLoad()
    {
        var builder = new StyleBuilderV1();

        for (int i = 0; i < HighLoad; i++)
        {
            builder.AddStyle($"prop-{i}", $"val-{i}", when: i % 2 == 0);
        }

        return builder.Build();
    }

    [Benchmark]
    public string? V2Builder_HighLoad()
    {
        var builder = new StyleBuilderV2();

        for (int i = 0; i < HighLoad; i++)
        {
            builder.AddStyle($"prop-{i}", $"val-{i}", when: i % 2 == 0);
        }

        return builder.Build();
    }

    [Benchmark]
    public string? V3Builder_HighLoad()
    {
        var builder = new StyleBuilderV3();

        for (int i = 0; i < HighLoad; i++)
        {
            builder.AddStyle($"prop-{i}", $"val-{i}", when: i % 2 == 0);
        }

        return builder.Build();
    }
}
