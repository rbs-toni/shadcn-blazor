using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Validators;
using System;
using System.Linq;

namespace ShadcnBlazor.Benchmarks;
public class GlobalConfig : ManualConfig
{
    public GlobalConfig()
    {
        //var projectRoot = FindProjectRoot();
        //ArtifactsPath = Path.Combine(projectRoot, "Results");

        AddDiagnoser(MemoryDiagnoser.Default);
        AddColumn(RankColumn.Arabic);
        WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
        AddValidator(JitOptimizationsValidator.FailOnError);
        AddValidator(BaselineValidator.FailOnError);
        AddExporter(MarkdownExporter.GitHub);
    }

    static string FindProjectRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);

        while (dir is not null)
        {
            if (dir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly).Any())
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new DirectoryNotFoundException("Could not locate project root (no .csproj found)");
    }
}
