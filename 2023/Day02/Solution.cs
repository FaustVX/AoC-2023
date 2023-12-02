#nullable enable
namespace AdventOfCode.Y2023.Day02;

using System.Buffers;
using static System.MemoryExtensions;

file readonly record struct Hand(int Red, int Green, int Blue)
{
    public static readonly Hand Max = new(12, 13, 14);

    public static bool operator <(Hand left, Hand right)
    => left.Red < right.Red
    && left.Green < right.Green
    && left.Blue < right.Blue;

    public static bool operator >(Hand left, Hand right)
    => left.Red > right.Red
    || left.Green > right.Green
    || left.Blue > right.Blue;

    public static Hand operator +(Hand left, Hand right)
    => new (Math.Max(left.Red, right.Red),
            Math.Max(left.Green, right.Green),
            Math.Max(left.Blue, right.Blue));

    public readonly int Power = Red * Green * Blue;

    public static Hand Parse(ReadOnlySpan<char> line) // line format: " 3 blue, 4 red" (note the heading space)
    {
        var (r, g, b) = (0, 0, 0);
        do
        {
            var n = int.Parse(line[..3]);
            line = line[3..].TrimStart();
            if (line[0] == 'r')
                r += n;
            else if (line[0] == 'g')
                g += n;
            else if (line[0] == 'b')
                b += n;
            var comma = line.IndexOf(',');
            if (comma < 0)
                break;
            line = line[(comma + 1)..];
        } while (true);
        return new(r, g, b);
    }
}

file readonly ref struct Game(int id, Game.ArrayRental hands)
{
    private static readonly ArrayPool<Hand> _pool = ArrayPool<Hand>.Create(10, 1);

    public int Id { get; } = id;
    public ArrayRental Hands { get; } = hands;

    public readonly struct ArrayRental(Hand[] array, int length)
    {
        public readonly Span<Hand> Span => array.AsSpan(0, length);
        public readonly void Return()
        => _pool.Return(array);
    }

    private static ArrayRental RentArray(int length)
    => new(_pool.Rent(length), length);

    public static Game Parse(ReadOnlySpan<char> line)
    {
        var colon = line.IndexOf(':');
        var id = int.Parse(line[5..colon]);
        line = line[(colon + 1)..];
        var rental = RentArray(line.Count(';') + 1);
        foreach (ref var hand in rental.Span)
        {
            var semi_colon = line.IndexOf(';');
            if (semi_colon < 0)
                hand = Hand.Parse(line);
            else
            {
                hand = Hand.Parse(line[..semi_colon]);
                line = line[(semi_colon + 1)..];
            }
        }
        return new(id, rental);
    }

    public readonly bool IsValidGame()
    {
        foreach (var hand in Hands.Span)
            if (hand > Hand.Max)
                return false;
        return true;
    }

    public readonly Hand GetMinimumValidGame()
    {
        var min = Hands.Span[0];
        foreach (var hand in Hands.Span)
            min += hand; // + is overloaded
        return min;
    }

    public void Dispose()
    => Hands.Return();
}

[ProblemName("Cube Conundrum")]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var idSum = 0;
        foreach (var line in input.EnumerateLines())
        {
            using var game = Game.Parse(line.Span);
            if (game.IsValidGame())
                idSum += game.Id;
        }
        return idSum;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        var powerSum = 0;
        foreach (var line in input.EnumerateLines())
        {
            using var game = Game.Parse(line.Span);
            powerSum += game.GetMinimumValidGame().Power;
        }
        return powerSum;
    }
}
