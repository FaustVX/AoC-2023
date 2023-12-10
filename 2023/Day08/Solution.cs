#nullable enable

namespace AdventOfCode.Y2023.Day08;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.MemoryExtensions;

[ProblemInfo("Haunted Wasteland", normalizeInput: false)]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        ParseInput(input.Span, out var tape, out var nodes);
        var current = GetNode("AAA", nodes, out _);
        for (var i = 0; i < int.MaxValue; i++)
        {
            if (current.Name is "ZZZ")
                return i;
            current = tape[i % tape.Length] switch
            {
                'L' => GetNode(current.Left, nodes, out _),
                'R' => GetNode(current.Right, nodes, out _),
                _ => throw new UnreachableException(),
            };
        }
        throw new UnreachableException();
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        ParseInput(input.Span, out var tape, out var nodes);
        var currents = CountLinesEndingInA(nodes, stackalloc (int line, int length)[10]);
        foreach (ref var current in currents)
        {
            for (var i = 0; i < int.MaxValue; i++)
            {
                if (Node.Parse(nodes.GetRowSpan(current.line)).Name[^1] is 'Z')
                {
                    current.length = i;
                    break;
                }
                _ = tape[i % tape.Length] switch
                {
                    'L' => GetNode(Node.Parse(nodes.GetRowSpan(current.line)).Left, nodes, out current.line),
                    'R' => GetNode(Node.Parse(nodes.GetRowSpan(current.line)).Right, nodes, out current.line),
                    _ => throw new UnreachableException(),
                };
            }
            if (current.length is 0)
                throw new UnreachableException();
        }
        return Lcm(currents);

        static long Lcm(ReadOnlySpan<(int line, int length)> arr)
        {
            var lcm = (long)arr[0].length;
            for (var i = 1; i < arr.Length; i++)
            {
                var num1 = lcm;
                var num2 = (long)arr[i].length;
                var gcdVal = Gcd(num1, num2);
                lcm = (lcm * (long)arr[i].length) / gcdVal;
            }
            return lcm;

            static long Gcd(long num1, long num2)
            {
                if (num2 == 0)
                    return num1;
                return Gcd(num2, num1 % num2);
            }
        }

        static Span<(int line, int length)> CountLinesEndingInA(ReadOnlySpan2D<char> nodes, Span<(int line, int length)> currents)
        {
            var count = 0;
            for (var n = 0; n < nodes.Height; n++)
                if (nodes.GetRowSpan(n)[2] is 'A')
                    currents[count++] = (n, 0);
            return currents[..count];
        }

        static bool IsAllInZ(Span<(int line, int length)> lines, ReadOnlySpan2D<char> nodes, int length)
        {
            foreach (ref var line in lines)
                if (Node.Parse(nodes.GetRowSpan(line.line)).Name[^1] is not 'Z')
                    return false;
                else
                    line.length = length;
            return true;
        }
    }

    static void ParseInput(ReadOnlySpan<char> input, out ReadOnlySpan<char> tape, out ReadOnlySpan2D<char> nodes)
    {
        var lines = input.EnumerateLines();
        lines.MoveNext(); // Tape
        tape = lines.Current;
        lines.MoveNext(); // /n
        lines.MoveNext(); // nodes
        var offset = Unsafe.ByteOffset(ref input.DangerousGetReference(), ref lines.Current.DangerousGetReference()) / Unsafe.SizeOf<char>();
        input = input[offset.ToInt32()..];
        nodes = input.AsSpan2D(input.Count('\n'), 17)[.., ..^1];
    }

    static Node GetNode(ReadOnlySpan<char> name, ReadOnlySpan2D<char> nodes, out int i)
    {
        for (i = 0; i < nodes.Height; i++)
        {
            var node = Node.Parse(nodes.GetRowSpan(i));
            if (node.Name.SequenceEqual(name))
                return node;
        }
        throw new UnreachableException();
    }
}

[StructLayout(LayoutKind.Auto)]
readonly ref partial struct Node([RefField] ref char name, [RefField] ref char left, [RefField] ref char right)
{
    public readonly ReadOnlySpan<char> Name => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _name), 3);
    public readonly ReadOnlySpan<char> Left => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _left), 3);
    public readonly ReadOnlySpan<char> Right => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _right), 3);

    public static Node Parse(ReadOnlySpan<char> line)
    => new(ref line.DangerousGetReference(), ref line.DangerousGetReferenceAt(7), ref line.DangerousGetReferenceAt(12));
}
