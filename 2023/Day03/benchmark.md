```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2715/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method  | Mean     | Error    | StdDev   | Gen0   | Allocated |
|-------- |---------:|---------:|---------:|-------:|----------:|
| PartOne | 182.0 μs | 711.7 μs | 39.01 μs |      - |     152 B |
| PartTwo | 865.9 μs | 589.4 μs | 32.31 μs | 3.9063 |   52320 B |
