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
        var currents = CountLinesEndingInA(nodes, stackalloc int[10]);
        for (var i = 0UL; i < ulong.MaxValue; i++)
        {
            if (IsAllInZ(currents, nodes))
                return i;
            foreach (ref var current in currents)
            {
                _ = tape[(int)(i % (ulong)tape.Length)] switch
                {
                    'L' => GetNode(Node.Parse(nodes.GetRowSpan(current)).Left, nodes, out current),
                    'R' => GetNode(Node.Parse(nodes.GetRowSpan(current)).Right, nodes, out current),
                    _ => throw new UnreachableException(),
                };
            }
        }
        throw new UnreachableException();

        static Span<int> CountLinesEndingInA(ReadOnlySpan2D<char> nodes, Span<int> currents)
        {
            var count = 0;
            for (var n = 0; n < nodes.Height; n++)
                if (nodes.GetRowSpan(n)[2] is 'A')
                    currents[count++] = n;
            return currents[..count];
        }

        static bool IsAllInZ(ReadOnlySpan<int> lines, ReadOnlySpan2D<char> nodes)
        {
            foreach (var line in lines)
                if (Node.Parse(nodes.GetRowSpan(line)).Name[^1] is not 'Z')
                    return false;
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
