#nullable enable
using System.Buffers;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
public class Solution : ISolver //, IDisplay
{
    private static readonly SearchValues<char> _numbers = SearchValues.Create("0123456789");
    public object PartOne(string input)
    {
        var sum = 0;
        foreach (var line in input.AsMemory().EnumerateLines())
            sum += GetCode(line.Span);
        return sum;

        static int GetCode(ReadOnlySpan<char> line)
        {
            var first = line.IndexOfAny(_numbers);
            var last = line.LastIndexOfAny(_numbers);
            return (line[first] - '0') * 10 + (line[last] - '0');
        }
    }

    public object PartTwo(string input)
    {
        return 0;
    }
}
