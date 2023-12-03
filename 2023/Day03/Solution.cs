#nullable enable
namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var lineLength = input.Span.IndexOf('\n') + 1;
        var lines = input.AsMemory2D(input.Length / lineLength, lineLength)[.., ..^1];
        var partSum = 0;
        foreach (var span in GetSpans(lines))
            if (IsValidSpan(span, lines.Span))
                partSum += int.Parse(span.GetSpan(lines.Span));
        return partSum;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }

    private static IEnumerable<TextSpan> GetSpans(ReadOnlyMemory2D<char> input)
    {
        var currentSpan = default(TextSpan);
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
            if (input.Span[y, x] is >= '0' and <= '9')
                if (currentSpan == default)
                    currentSpan = new(x, y, 1);
                else
                    currentSpan = currentSpan with { Length = currentSpan.Length + 1 };
            else if (currentSpan != default)
            {
                yield return currentSpan;
                currentSpan = default;
            }
    }

    private static bool IsValidSpan(TextSpan span, ReadOnlySpan2D<char> input)
    {
        for (var i = 0; i < span.Length; i++)
            foreach (var c in GetArround(span.Column + i, span.Line, input))
                if (c is not ((>= '0' and  <= '9') or '.'))
                    return true;
        return false;

        static ReadOnlySpan2D<char> GetArround(int x, int y, ReadOnlySpan2D<char> input)
        {
            var width = Math.Max(0, x - 1)..Math.Min(input.Width - 1, x + 2);
            var height = Math.Max(0, y - 1)..Math.Min(input.Height - 1, y + 2);
            return input[height, width];
        }
    }
}

readonly record struct TextSpan(int Column, int Line, int Length)
{
    public ReadOnlySpan<char> GetSpan(ReadOnlySpan2D<char> input)
    => input.GetRowSpan(Line).Slice(Column, Length);
}
