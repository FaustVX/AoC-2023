```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2715/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method  | Mean            | Error            | StdDev         | Gen0   | Allocated |
|-------- |----------------:|-----------------:|---------------:|-------:|----------:|
| PartOne |        15.57 μs |         6.276 μs |       0.344 μs | 0.2747 |   3.52 KB |
| PartTwo | 6,872,053.03 μs | 4,442,976.423 μs | 243,534.530 μs |      - |   3.91 KB |
