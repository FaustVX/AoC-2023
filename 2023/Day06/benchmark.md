```

BenchmarkDotNet v0.13.10, Windows 11 (10.0.22621.2715/22H2/2022Update/SunValley2)
12th Gen Intel Core i7-12700H, 1 CPU, 20 logical and 14 physical cores
.NET SDK 8.0.100
  [Host]   : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2
  ShortRun : .NET 8.0.0 (8.0.23.53103), X64 RyuJIT AVX2

Job=ShortRun  IterationCount=3  LaunchCount=1  
WarmupCount=3  

```
| Method  | Mean            | Error            | StdDev          | Gen0   | Allocated |
|-------- |----------------:|-----------------:|----------------:|-------:|----------:|
| PartOne |        257.7 ns |         35.88 ns |         1.97 ns | 0.0019 |      24 B |
| PartTwo | 31,414,464.6 ns | 78,204,801.26 ns | 4,286,669.05 ns |      - |      36 B |
