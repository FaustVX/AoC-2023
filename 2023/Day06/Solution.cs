#nullable enable
namespace AdventOfCode.Y2023.Day06;

[ProblemInfo("Wait For It")]
public class Solution : ISolver //, IDisplay
{
    // https://www.desmos.com/calculator/pd7bdyskxx
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var infos = (stackalloc Info[Globals.IsTestInput ? 3 : 4]);
        ParseInput(input.Span, infos);
        var product = 1UL;
        foreach (var info in infos)
            product *= info.Count();
        return product;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        var infos = (stackalloc Info[Globals.IsTestInput ? 3 : 4]);
        ParseInput(input.Span, infos);
        ref var last = ref infos[^1];
        for (var i = infos.Length - 2; i >= 0 ; i--)
            last = last.Combine(infos[i]);
        return last.Count();
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
            info = info with { Duration = int.Parse(line) };
        }
        start = 10;
        lines.MoveNext();
        foreach (ref var info in infos)
        {
            var line = lines.Current.Slice(start, Math.Min(offset, lines.Current.Length - start));
            start += offset;
            info = info with { Record = ulong.Parse(line) };
        }
    }
}

readonly record struct Info(int Duration, ulong Record)
{
    public ulong Count()
    {
        var count = 0UL;
        for (var t = 1U; t < Duration; t++)
            if (((ulong)(Duration - t) * t) > Record)
                count++;
        return count;
    }

    public Info Combine(Info other)
    {
        var result = this;
        var power1 = (int)Math.Pow(10, (int)Math.Log10(Duration) + 1);
        var power2 = (ulong)Math.Pow(10, (ulong)Math.Log10(Record) + 1);
        for (var i = 0; i < 4; i++)
        {
            result = new(result.Duration + (other.Duration % 10) * power1, result.Record + (other.Record % 10) * power2);
            (power1, power2) = (power1 * 10, power2 * 10);
            other = new(other.Duration / 10, other.Record / 10);
        }
        return result;
    }
}
