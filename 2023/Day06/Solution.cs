#nullable enable
namespace AdventOfCode.Y2023.Day06;

[ProblemInfo("Wait For It")]
public class Solution : ISolver //, IDisplay
{
    // https://www.desmos.com/calculator/lcw2jcaoje
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var infos = (stackalloc Info[Globals.IsTestInput ? 3 : 4]);
        ParseInput(input.Span, infos);
        return 0;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }

    static void ParseInput(ReadOnlySpan<char> input, Span<Info> infos)
    {
        var lines = input.EnumerateLines();
        var offset = Globals.IsTestInput ? 4 : 7;
        var start = 10;
        lines.MoveNext();
        foreach (ref var info in infos)
        {
            var line = lines.Current.Slice(start, Math.Min(offset, lines.Current.Length - start));
            start += offset;
            info = info with { Time = int.Parse(line) };
        }
        start = 10;
        lines.MoveNext();
        foreach (ref var info in infos)
        {
            var line = lines.Current.Slice(start, Math.Min(offset, lines.Current.Length - start));
            start += offset;
            info = info with { Distance = int.Parse(line) };
        }
    }
}

readonly record struct Info(int Time, int Distance);
