```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.22631.5189/23H2/2023Update/SunValley3)
AMD Ryzen 7 5800U with Radeon Graphics, 1 CPU, 16 logical and 8 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2
  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT AVX2


```
| Method                | Mean     | Error     | StdDev    | Median   | Ratio | RatioSD | Rank | Gen0   | Allocated | Alloc Ratio |
|---------------------- |---------:|----------:|----------:|---------:|------:|--------:|-----:|-------:|----------:|------------:|
| CachedStringBuilder   | 1.806 μs | 0.0361 μs | 0.0886 μs | 1.785 μs |  0.34 |    0.02 |    1 | 0.7648 |   6.25 KB |        0.24 |
| StandardStringBuilder | 5.391 μs | 0.1070 μs | 0.2543 μs | 5.302 μs |  1.00 |    0.07 |    2 | 3.2501 |  26.56 KB |        1.00 |
