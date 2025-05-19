using BenchmarkDotNet.Attributes;

namespace ShadcnBlazor.Benchmarks.Builders;

[MemoryDiagnoser]
[RankColumn]
[ReturnValueValidator(failOnError: true)]
public class CssBuilderBenchmarkHighLoad
{
    const int HighLoad = 512;
    const string UserClasses = "disabled loading";

    [Benchmark(Baseline = true)]
    public string? V1Builder_HighLoad()
    {
        var builder = new CssBuilderV1(UserClasses);

        for (int i = 0; i < HighLoad; i++)
        {
            builder.AddClass($"item-{i}", i % 2 == 0);
        }

        return builder.Build();
    }

    [Benchmark]
    public string? V2Builder_HighLoad()
    {
        var builder = new CssBuilderV2(UserClasses);

        for (int i = 0; i < HighLoad; i++)
        {
            builder.AddClass($"item-{i}", i % 2 == 0);
        }

        return builder.Build();
    }

    [Benchmark]
    public string? V3Builder_HighLoad()
    {
        var builder = new CssBuilderV3(UserClasses);

        for (int i = 0; i < HighLoad; i++)
        {
            builder.AddClass($"item-{i}", i % 2 == 0);
        }

        return builder.Build();
    }
}

