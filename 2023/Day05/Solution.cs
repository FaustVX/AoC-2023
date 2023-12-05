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
            var number = seed;
            foreach (var map in maps)
                number = map.NextNumber(number);
            number.SetMin(ref min);
        }
        return min;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
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
