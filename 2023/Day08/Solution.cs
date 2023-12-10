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
        var current = GetNode("AAA", nodes);
        for (int i = 0; i < int.MaxValue; i++)
        {
            if (current.Name is "ZZZ")
                return i;
            current = tape[i % tape.Length] switch
            {
                'L' => GetNode(current.Left, nodes),
                'R' => GetNode(current.Right, nodes),
                _ => throw new UnreachableException(),
            };
        }
        throw new UnreachableException();
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
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

    static Node GetNode(ReadOnlySpan<char> name, ReadOnlySpan2D<char> nodes)
    {
        for (var i = 0; i < nodes.Height; i++)
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
