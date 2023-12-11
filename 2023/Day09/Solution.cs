#nullable enable
namespace AdventOfCode.Y2023.Day09;

using static System.MemoryExtensions;

[ProblemInfo("Mirage Maintenance")]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var sum = 0L;
        var values = (stackalloc int[Globals.IsTestInput ? 6 : 21]);
        foreach (var line in input.Span.EnumerateLines())
        {
            ParseInput(line, values);
            sum += GetNext(values);
        }
        return sum;

        static int GetNext(ReadOnlySpan<int> values)
        {
            var diffs = (stackalloc int[values.Length - 1]);
            var all0 = true;
            for (var i = 0; i < diffs.Length; i++)
            {
                var diff = diffs[i] = values[i + 1] - values[i];
                if (all0 && diff != 0)
                    all0 = false;
            }
            if (!all0)
                return values[^1] + GetNext(diffs);
            return values[^1];
        }
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        var sum = 0L;
        var values = (stackalloc int[Globals.IsTestInput ? 6 : 21]);
        foreach (var line in input.Span.EnumerateLines())
        {
            ParseInput(line, values);
            sum += GetPrevious(values);
        }
        return sum;

        static int GetPrevious(ReadOnlySpan<int> values)
        {
            var diffs = (stackalloc int[values.Length - 1]);
            var all0 = true;
            for (var i = 0; i < diffs.Length; i++)
            {
                var diff = diffs[i] = values[i] - values[i + 1];
                if (all0 && diff != 0)
                    all0 = false;
            }
            if (!all0)
                return values[0] + GetPrevious(diffs);
            return values[0];
        }
    }

    static void ParseInput(ReadOnlySpan<char> line, Span<int> values)
    {
        foreach (ref var value in values[..^1])
        {
            var space = line.IndexOf(' ') + 1;
            value = int.Parse(line[..space]);
            line = line[space..];
        }
        values[^1] = int.Parse(line);
    }
}
