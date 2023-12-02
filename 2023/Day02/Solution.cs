#nullable enable
namespace AdventOfCode.Y2023.Day02;

using System.Runtime.InteropServices;
using static System.MemoryExtensions;

readonly record struct Hand(int Red, int Green, int Blue)
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

    public static Hand Parse(ReadOnlySpan<char> line) // line format: " 3 blue, 4 red" (note the leading space)
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

[StructLayout(LayoutKind.Auto)]
readonly partial struct Game([Property]int id, [Property]ArrayRental<Hand> hands) : IDisposable
{
    public static Game Parse(ReadOnlySpan<char> line)
    {
        var colon = line.IndexOf(':');
        var id = int.Parse(line[5..colon]);
        line = line[(colon + 1)..];
        var rental = ArrayRental<Hand>.Rent(line.Count(';') + 1);
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

    void IDisposable.Dispose()
    => Hands.Return();
}

[ProblemName("Cube Conundrum")]
public class Solution : ISolver //, IDisplay
{
    private static int Execute(ReadOnlyMemory<char> input, Func<Game, int> getValue)
    {
        var idSum = 0;
        foreach (var line in input.EnumerateLines())
        {
            using var game = Game.Parse(line.Span);
            idSum += getValue(game);
        }
        return idSum;
    }

    public object PartOne(ReadOnlyMemory<char> input)
    => Execute(input, static game => game.IsValidGame() ? game.Id : 0);

    public object PartTwo(ReadOnlyMemory<char> input)
    => Execute(input, static game => game.GetMinimumValidGame().Power);
}
