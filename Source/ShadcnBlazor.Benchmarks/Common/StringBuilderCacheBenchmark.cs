using BenchmarkDotNet.Attributes;
using System;
using System.Linq;
using System.Text;

namespace ShadcnBlazor.Benchmarks.Common;
[Config(typeof(GlobalConfig))]
public class StringBuilderCacheBenchmark
{
    const int Iterations = 100;
    const string SampleText = "ShadcnBlazor is fast!";

    [Benchmark]
    public string CachedStringBuilder()
    {
        string result = string.Empty;
        for (int i = 0; i < Iterations; i++)
        {
            var sb = StringBuilderCache.Acquire();
            sb.Append(SampleText);
            result = StringBuilderCache.GetStringAndRelease(sb);
        }
        return result;
    }
    [Benchmark(Baseline = true)]
    public string StandardStringBuilder()
    {
        string result = string.Empty;
        for (int i = 0; i < Iterations; i++)
        {
            var sb = new StringBuilder();
            sb.Append(SampleText);
            result = sb.ToString();
        }
        return result;
    }
}
