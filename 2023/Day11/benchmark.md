```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2861/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method  | Mean     | Error     | StdDev    | Allocated |
|-------- |---------:|----------:|----------:|----------:|
| PartOne | 2.375 ms | 0.8582 ms | 0.0470 ms |      26 B |
| PartTwo | 2.257 ms | 1.3769 ms | 0.0755 ms |      24 B |
