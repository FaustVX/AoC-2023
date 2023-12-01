#nullable enable
using System.Buffers;

namespace AdventOfCode.Y2023.Day01;

[ProblemName("Trebuchet?!")]
public class Solution : ISolver //, IDisplay
{
    private static readonly SearchValues<char> _numbers = SearchValues.Create("0123456789");
    public object PartOne(ReadOnlyMemory<char> input)
    {
        try
        {
            var sum = 0;
            foreach (var line in input.EnumerateLines())
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

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        var sum = 0;
        foreach (var line in input.EnumerateLines())
            sum += GetCode(line.Span);
        return sum;

        static int GetCode(ReadOnlySpan<char> line)
        {
            return GetFirst(line) * 10 + GetLast(line);

            static int GetFirst(ReadOnlySpan<char> line)
            {
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] is >= '1' and <= '9')
                        return line[i] - '0';
                    switch (line[i])
                    {
                        case 'o' when line[i..].StartsWith("one"):
                            return 1;
                        case 't' when line[i..].StartsWith("two"):
                            return 2;
                        case 't' when line[i..].StartsWith("three"):
                            return 3;
                        case 'f' when line[i..].StartsWith("four"):
                            return 4;
                        case 'f' when line[i..].StartsWith("five"):
                            return 5;
                        case 's' when line[i..].StartsWith("six"):
                            return 6;
                        case 's' when line[i..].StartsWith("seven"):
                            return 7;
                        case 'e' when line[i..].StartsWith("eight"):
                            return 8;
                        case 'n' when line[i..].StartsWith("nine"):
                            return 9;
                    }
                }
                throw new UnreachableException();
            }

            static int GetLast(ReadOnlySpan<char> line)
            {
                for (var i = line.Length - 1; i >= 0; i--)
                {
                    if (line[i] is >= '1' and <= '9')
                        return line[i] - '0';
                    switch (line[i])
                    {
                        case 'o' when line[i..].StartsWith("one"):
                            return 1;
                        case 't' when line[i..].StartsWith("two"):
                            return 2;
                        case 't' when line[i..].StartsWith("three"):
                            return 3;
                        case 'f' when line[i..].StartsWith("four"):
                            return 4;
                        case 'f' when line[i..].StartsWith("five"):
                            return 5;
                        case 's' when line[i..].StartsWith("six"):
                            return 6;
                        case 's' when line[i..].StartsWith("seven"):
                            return 7;
                        case 'e' when line[i..].StartsWith("eight"):
                            return 8;
                        case 'n' when line[i..].StartsWith("nine"):
                            return 9;
                    }
                }
                throw new UnreachableException();
            }
        }
    }
}
