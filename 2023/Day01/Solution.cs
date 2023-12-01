#nullable enable
using System.Buffers;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
public class Solution : ISolver //, IDisplay
{
    private static readonly SearchValues<char> _numbers = SearchValues.Create("0123456789");
    public object PartOne(string input)
    {
        try
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
        catch
        {
            if (!Globals.IsTestInput)
                throw;
            return -1;
        }
    }

    public object PartTwo(string input)
    {
        var sum = 0;
        foreach (var line in input.AsMemory().EnumerateLines())
            sum += GetCode(line.Span);
        return sum;

        static int GetCode(ReadOnlySpan<char> line)
        {
            return GetValue(line, isReversed: false) * 10 + GetValue(line, isReversed: true);

            static int GetValue(ReadOnlySpan<char> line, bool isReversed)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    var j = isReversed ? line.Length - i - 1 : i;
                    if (line[j] is >= '1' and <= '9')
                        return line[j] - '0';
                    switch (line[j])
                    {
                        case 'o' when line[j..].StartsWith("one"):
                            return 1;
                        case 't' when line[j..].StartsWith("two"):
                            return 2;
                        case 't' when line[j..].StartsWith("three"):
                            return 3;
                        case 'f' when line[j..].StartsWith("four"):
                            return 4;
                        case 'f' when line[j..].StartsWith("five"):
                            return 5;
                        case 's' when line[j..].StartsWith("six"):
                            return 6;
                        case 's' when line[j..].StartsWith("seven"):
                            return 7;
                        case 'e' when line[j..].StartsWith("eight"):
                            return 8;
                        case 'n' when line[j..].StartsWith("nine"):
                            return 9;
                    }
                }
                throw new UnreachableException();
            }
        }
    }
}
