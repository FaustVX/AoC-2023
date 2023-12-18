#nullable enable
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace AdventOfCode.Y2023.Day12;

[ProblemInfo("Hot Springs")]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var ranges = (stackalloc Range[10]);
        var rulesInt = (stackalloc int[10]);
        var count = 0UL;
        foreach (var line in input.Span.EnumerateLines())
        {
            var space = line.IndexOf(' ');
            var pattern = Convert(line[..space]);
            var rules = ParseInts(line.Slice(space + 1), ranges[..line.Slice(space + 1).Split(ranges, ',')], rulesInt);

            foreach (var a in new PermutationEnumerable(pattern))
            {
                if (IsValid(a, rules))
                    count++;
            }
        }
        return count;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }

    static ReadOnlySpan<int> ParseInts(ReadOnlySpan<char> line, ReadOnlySpan<Range> ranges, Span<int> ints)
    {
        for (int i = 0; i < ranges.Length; i++)
            ints[i] = int.Parse(line[ranges[i]]);
        return ints[..ranges.Length];
    }

    static Span<bool?> Convert(ReadOnlySpan<char> line)
    {
        var span = MemoryMarshal.CreateSpan(ref Unsafe.As<char, bool?>(ref line.DangerousGetReference()), line.Length); // char and bool? are 2 bytes long each
        foreach (ref var item in span)
            item = Unsafe.As<bool?, char>(ref item) switch
            {
                '.' => true,
                '#' => false,
                '?' => null,
                _ => throw new UnreachableException(),
            };
        return span;
    }

    static bool IsValid(ReadOnlySpan<bool?> pattern, ReadOnlySpan<int> rules)
    {
        var idx = 0;
        do
        {
            var start = IndexOf(pattern, false);
            if (start < 0)
                break;
            pattern = pattern[start..];
            var end = IndexOf(pattern, true);
            if (end < 0)
                end = pattern.Length;
            if (idx >= rules.Length || rules[idx++] != end)
                return false;
            if (end >= pattern.Length)
                break;
            pattern = pattern[end..];
        } while (!pattern.IsEmpty);
        return idx == rules.Length;

        static int IndexOf(ReadOnlySpan<bool?> span, bool? value)
        {
            for (var i = 0; i < span.Length; i++)
                if (span[i] == value)
                    return i;
            return -1;
        }
    }
}

[StructLayout(LayoutKind.Auto)]
ref partial struct PermutationEnumerable([Field]ReadOnlySpan<bool?> pattern)
{
    private int _permutation = (int)Math.Pow(2, pattern.Count(null)) - 1;
    private readonly int _unknownLength = pattern.Count(null);
    private readonly Buffer<int> _lookup = CreateBuffer(pattern);

    private static Buffer<int> CreateBuffer(ReadOnlySpan<bool?> pattern)
    {
        var buffer = Buffer<int>.Values;
        var j = 0;
        for (var i = 0; i < pattern.Length; i++)
            if (pattern[i] == null)
                buffer[j++] = i;
        return buffer;
    }

    [InlineArray(20)]
    private struct Buffer<T>
    {
        public static readonly Buffer<T> Values = new();
        private T _value;
        public readonly Span<T> GetSpan(int length)
        => MemoryMarshal.CreateSpan(ref Unsafe.AsRef(in _value), length);
    }

    public readonly PermutationEnumerable GetEnumerator()
    => this;

    public bool MoveNext()
    {
        var destination = Current.AsWriteable();
        _pattern.CopyTo(destination);
        // for (var i = 0; i < _pattern.Length; i++)
        // {
        //     if (_pattern[i] != null)
        //         continue;
        //     destination[i] = ((_permutation >> i) & 0b1) == 1;
        // }
        for (var i = 0; i < _unknownLength; i++)
            destination[_lookup[i]] = ((_permutation >> i) & 0b1) == 1;
        return _permutation-- >= 0;
    }

    public readonly ReadOnlySpan<bool?> Current
    => Buffer<bool?>.Values.GetSpan(_pattern.Length);
}
