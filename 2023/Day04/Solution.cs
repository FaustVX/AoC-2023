#nullable enable

namespace AdventOfCode.Y2023.Day04;

[ProblemInfo("Scratchcards", normalizeInput: false)]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var lines = GetLines(input, out var pipe);
        var winningSpan = (stackalloc int[(pipe - 1 - 9) / 3]);
        var mineSpan = (stackalloc int[(lines.Width - (pipe - 1)) / 3]);
        var sum = 0;
        for (int i = 0; i < lines.Height; i++)
        {
            ParseInts(lines.Span.GetRowSpan(i)[9..(pipe - 1)], winningSpan);
            ParseInts(lines.Span.GetRowSpan(i)[(pipe + 1)..], mineSpan);
            var count = 0;
            foreach (var item in winningSpan)
                if (mineSpan.Contains(item))
                    if (count is 0)
                        count = 1;
                    else
                        count *= 2;
            sum += count;
        }
        return sum;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }

    public static ReadOnlyMemory2D<char> GetLines(ReadOnlyMemory<char> input, out int pipeIndex)
    {
        var lineLength = input.Span.IndexOf('\n') + 1;
        pipeIndex = input.Span.IndexOf('|');
        return input.AsMemory2D(input.Length / lineLength, lineLength)[.., ..^1];
    }

    static void ParseInts(ReadOnlySpan<char> input, Span<int> output)
    {
        for (int i = 0; i < output.Length; i++)
            output[i] = int.Parse(input[(i * 3)..(i * 3 + 3)]);
    }
}
