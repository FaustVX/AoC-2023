```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2715/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method  | Mean     | Error     | StdDev    | Gen0     | Allocated |
|-------- |---------:|----------:|----------:|---------:|----------:|
| PartOne | 3.925 ms | 10.186 ms | 0.5583 ms | 347.6563 |    4.2 MB |
| PartTwo | 8.326 ms |  3.705 ms | 0.2031 ms | 328.1250 |   3.98 MB |
