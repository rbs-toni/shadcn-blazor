using BenchmarkDotNet.Attributes;
using System;

namespace ShadcnBlazor.Benchmarks.Builders;

[MemoryDiagnoser]
[RankColumn]
[ReturnValueValidator(failOnError: true)]
public class CssBuilderBenchmark
{
    const string TestClasses = "btn primary large hover:scale-110 focus:ring-2";
    const string UserClasses = "disabled loading";

    [Benchmark(Baseline = true)]
    public string? V1Builder()
    {
        var builder = new CssBuilderV1(UserClasses)
            .AddClass(TestClasses)
            .AddClass("hidden", false)
            .AddClass("block", true);
        return builder.Build();
    }

    [Benchmark]
    public string? V2Builder()
    {
        var builder = new CssBuilderV2(UserClasses)
            .AddClass(TestClasses)
            .AddClass("hidden", false)
            .AddClass("block", true);
        return builder.Build();
    }
    [Benchmark]
    public string? V3Builder()
    {
        var builder = new CssBuilderV3(UserClasses)
            .AddClass(TestClasses)
            .AddClass("hidden", false)
            .AddClass("block", true);
        return builder.Build();
    }
}

