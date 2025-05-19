using BenchmarkDotNet.Running;

namespace ShadcnBlazor.Benchmarks;
static class Program
{
    static void Main(string[] args)
    {
        BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
    }
}
