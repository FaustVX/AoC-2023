#nullable enable
namespace AdventOfCode.Y2023.Day05;

using System;

[ProblemInfo("If You Give A Seed A Fertilizer")]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var (maps, seeds) = ParseInput(input.Span);
        var min = uint.MaxValue;
        foreach (var seed in seeds)
        {
            var number = GetLocationFromSeed(seed, maps);
            number.SetMin(ref min);
        }
        return min;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        var (maps, seeds) = ParseInput(input.Span);
        var ranges = seeds.AsSpan().Cast<uint, Range>();

        if (Globals.IsTestInput)
            return GetMinTest(maps, ranges);
        return GetMin(maps, ranges, 100_000);

        static uint GetMin(ReadOnlySpan<Map> maps, ReadOnlySpan<Range> ranges, uint jump)
        {
            var (min, minRange) = (uint.MaxValue, new Range(0U, 0U));
            foreach (var (start, length) in ranges)
                for (var s = 0u; s < length; s += jump)
                {
                    var number = GetLocationFromSeed(start + s, maps);
                    if (number < min)
                        (min, minRange) = (number, new(start, length));
                }

            if (jump <= 100)
                return GetMinTest(maps, ranges);

            var newRanges = (stackalloc Range[11]);
            GetSmallerRanges(minRange, ref newRanges);
            return GetMin(maps, newRanges, jump / 100);

            static void GetSmallerRanges(Range range, ref Span<Range> ranges)
            {
                var start = range.Start;
                var length = range.Length / 10;

                foreach (ref var r in ranges[..10])
                {
                    r = new(start, length);
                    start += length;
                }

                var last = range.Length % 10;
                if (last is 0)
                    ranges = ranges[..10];
                else
                    ranges[^1] = new (start, last);
            }
        }

        static uint GetMinTest(ReadOnlySpan<Map> maps, ReadOnlySpan<Range> ranges)
        {
            var min = uint.MaxValue;
            foreach (var (start, length) in ranges)
                for (var s = 0u; s < length; s++)
                {
                    var number = GetLocationFromSeed(start + s, maps);
                    number.SetMin(ref min);
                }
            return min;
        }
    }

    static uint GetLocationFromSeed(uint number, ReadOnlySpan<Map> maps)
    {
        foreach (var map in maps)
            number = map.NextNumber(number);
        return number;
    }

    private static (Map[] maps, uint[] seeds) ParseInput(ReadOnlySpan<char> input)
    {
        var lines = input.EnumerateLines();
        lines.MoveNext();
        var seeds = new uint[lines.Current[6..].Count(' ')];
        ParseSeeds(lines.Current[7..], seeds);
        var maps = new Map[7];
        ParseMaps(input[input.IndexOf("\n\n")..], maps);
        return (maps, seeds);

        static void ParseSeeds(ReadOnlySpan<char> input, Span<uint> seeds)
        {
            foreach (ref var seed in seeds[..^1])
            {
                var space = input.IndexOf(' ') + 1;
                seed = uint.Parse(input[..space]);
                input = input[space..];
            }
            seeds[^1] = uint.Parse(input);
        }

        static void ParseMaps(ReadOnlySpan<char> input, Span<Map> maps)
        {
            foreach (ref var map in maps)
            {
                var colon = input.IndexOf(':') + 2; // 2 => ":\n"
                input = input[colon..];
                var end = input.IndexOf("\n\n") is var i and not -1 ? i : ^0;
                map = Map.Parse(input[..end]);
            }
        }
    }
}

readonly record struct Map(Span[] Spans)
{
    public readonly uint NextNumber(uint input)
    {
        foreach (var span in Spans)
            if (span.IsIn(input, out var destination))
                return destination;
        return input;
    }

    public static Map Parse(ReadOnlySpan<char> input)
    {
        var spans = new Span[input.Count('\n') + 1];
        var lines = input.EnumerateLines();
        foreach (ref var span in spans.AsSpan())
        {
            lines.MoveNext();
            span = Span.Parse(lines.Current);
        }
        return new(spans);
    }
}

readonly record struct Range(uint Start, uint Length);

readonly record struct Span(uint Destination, uint Source, int Length)
{
    public readonly bool IsIn(uint source, out uint destination)
    {
        if (source >= Source && source <= (Source + Length))
        {
            destination = Destination + (source - Source);
            return true;
        }
        destination = default;
        return false;
    }

    public static Span Parse(ReadOnlySpan<char> input)
    {
        var space = input.IndexOf(' ') + 1;
        var destination = uint.Parse(input[..space]);
        input = input[space..];
        space = input.IndexOf(' ') + 1;
        var source = uint.Parse(input[..space]);
        input = input[space..];
        var length = int.Parse(input);

        return new(destination, source, length);
    }
}
