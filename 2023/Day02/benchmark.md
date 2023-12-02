```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2715/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method  | Mean     | Error     | StdDev   | Gen0   | Allocated |
|-------- |---------:|----------:|---------:|-------:|----------:|
| PartOne | 20.73 μs |  8.227 μs | 0.451 μs | 0.7629 |   9.46 KB |
| PartTwo | 22.86 μs | 22.535 μs | 1.235 μs | 0.7324 |   9.46 KB |
