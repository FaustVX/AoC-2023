#nullable enable
namespace AdventOfCode.Y2023.Day03;

[ProblemName("Gear Ratios")]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var lines = GetLines(input);
        var partSum = 0;
        foreach (var span in GetSpans(lines, char.IsDigit))
            if (IsValidPartNumber(span, lines.Span))
                partSum += int.Parse(span.GetSpan(lines.Span));
        return partSum;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        var lines = GetLines(input);
        var parts = GetSpans(lines, char.IsDigit).ToArray();
        var gearRatioSum = 0;
        foreach (var span in GetSpans(lines, static c => c is '*'))
            if (IsValidGear(span, lines.Span, parts, out var ratio))
                gearRatioSum += ratio;
        return gearRatioSum;
    }

    public static ReadOnlyMemory2D<char> GetLines(ReadOnlyMemory<char> input)
    {
        var lineLength = input.Span.IndexOf('\n') + 1;
        return input.AsMemory2D(input.Length / lineLength, lineLength)[.., ..^1];
    }

    private static IEnumerable<TextSpan> GetSpans(ReadOnlyMemory2D<char> input, Func<char, bool> isIncluded)
    {
        var currentSpan = default(TextSpan);
        for (var y = 0; y < input.Height; y++)
        for (var x = 0; x < input.Width; x++)
            if (isIncluded(input.Span[y, x]))
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

    private static bool IsValidPartNumber(TextSpan span, ReadOnlySpan2D<char> input)
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

    private static bool IsValidGear(TextSpan gear, ReadOnlySpan2D<char> input, IEnumerable<TextSpan> parts, out int gearRatio)
    {
        var (part1, part2) = (-1, -1);
        foreach (var part in parts)
            if (gear.Line >= part.Line - 1 && gear.Line <= part.Line + 1 && gear.Column >= part.Column - 1 && gear.Column <= part.Column + part.Length)
                if (!TrySetValue(ref part1, part, input) && !TrySetValue(ref part2, part, input))
                    break;
        if ((part1, part2) is not (> -1, > -1))
        {
            gearRatio = 0;
            return false;
        }
        gearRatio = part1 * part2;
        return true;

        static bool TrySetValue(ref int partX, TextSpan part, ReadOnlySpan2D<char> input)
        {
            if (partX is not -1)
                return false;
            partX = int.Parse(part.GetSpan(input));
            return true;
        }
    }
}

readonly record struct TextSpan(int Column, int Line, int Length)
{
    public ReadOnlySpan<char> GetSpan(ReadOnlySpan2D<char> input)
    => input.GetRowSpan(Line).Slice(Column, Length);
}
