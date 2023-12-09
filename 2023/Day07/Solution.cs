#nullable enable
namespace AdventOfCode.Y2023.Day07;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static System.MemoryExtensions;

[ProblemInfo("Camel Cards", normalizeInput: false)]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
        var hands = (stackalloc Hand[input.Span.Count('\n')]);
        var lines = input.Span.EnumerateLines();
        foreach (ref var hand in hands)
        {
            lines.MoveNext();
            hand = Hand.Parse(lines.Current);
        }
        hands.Sort(Hand.CompareTo);

        var score = 0;
        for (var i = 0; i < hands.Length; i++)
            score += (i + 1) * hands[i].Bid;
        return score;
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }
}

readonly record struct Hand(Rank Card1, Rank Card2, Rank Card3, Rank Card4, Rank Card5, ushort Bid)
{
    // see: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/proposals/csharp-12.0/inline-arrays#obtaining-instances-of-span-types-for-an-inline-array-type
    public readonly Span<Rank> Span => MemoryMarshal.CreateSpan(ref Unsafe.As<Hand, Rank>(ref Unsafe.AsRef(in this)), 5);

    public readonly Type Type
    {
        get
        {
            var cards = (stackalloc Rank[5]);
            Span.CopyTo(cards);
            cards.Sort();
            if ((IsFive(cards)
            ?? IsFour(cards)
            ?? IsFull(cards)
            ?? IsThree(cards)
            ?? IsTwo(cards)
            ?? IsOne(cards)) is Type type)
                return type;
            return Type.High;

            static Type? IsFive(ReadOnlySpan<Rank> cards) // XXXXX
            => IsAllTheSame(cards) ? Type.Five : null;

            static Type? IsFour(ReadOnlySpan<Rank> cards) // XXXX_ _XXXX
            => IsAllTheSame(cards.Slice(0, 4)) || IsAllTheSame(cards.Slice(1, 4)) ? Type.Four : null;

            static Type? IsFull(ReadOnlySpan<Rank> cards) // XXYYY XXXYY
            => (IsAllTheSame(cards.Slice(0, 2)) && IsAllTheSame(cards.Slice(2, 3))) || (IsAllTheSame(cards.Slice(0, 3)) && IsAllTheSame(cards.Slice(3, 2))) ? Type.Full : null;

            static Type? IsThree(ReadOnlySpan<Rank> cards) // XXX__ _XXX_ __XXX
            => IsAllTheSame(cards.Slice(0, 3)) || IsAllTheSame(cards.Slice(1, 3)) || IsAllTheSame(cards.Slice(2, 3)) ? Type.Three : null;

            static Type? IsTwo(ReadOnlySpan<Rank> cards) // _XXYY XX_YY XXYY_
            => (IsAllTheSame(cards.Slice(1, 2)) && IsAllTheSame(cards.Slice(3, 2))) || (IsAllTheSame(cards.Slice(0, 2)) && IsAllTheSame(cards.Slice(3, 2))) || (IsAllTheSame(cards.Slice(0, 2)) && IsAllTheSame(cards.Slice(2, 2))) ? Type.Two : null;

            static Type? IsOne(ReadOnlySpan<Rank> cards)
            => IsAllTheSame(cards.Slice(0, 2)) || IsAllTheSame(cards.Slice(1, 2)) || IsAllTheSame(cards.Slice(2, 2)) || IsAllTheSame(cards.Slice(3, 2)) ? Type.One : null;

            static bool IsAllTheSame(ReadOnlySpan<Rank> cards)
            => cards[0] == cards[^1];
        }
    }

    public static bool operator >(Hand left, Hand right)
    {
        if (left.Type > right.Type)
            return true;
        if (left.Type < right.Type)
            return false;
        return CompareHands(left.Span, right.Span);

        static bool CompareHands(ReadOnlySpan<Rank> left, ReadOnlySpan<Rank> right)
        {
            if (left[0] > right[0])
                return true;
            if (left[0] < right[0])
                return false;
            if (left.Length <= 1)
                return false;
            return CompareHands(left[1..], right[1..]);
        }
    }

    [Obsolete("See '>' operator", error: true)]
    public static bool operator <(Hand left, Hand right)
    => throw new NotImplementedException("See '>' operator");

    public static int CompareTo(Hand left, Hand right)
    {
        if (left > right)
            return 1;
        if (right > left)
            return -1;
        return 0;
    }

    public static Hand Parse(ReadOnlySpan<char> line)
    {
        return new(GetRank(line[0]), GetRank(line[1]), GetRank(line[2]), GetRank(line[3]), GetRank(line[4]), ushort.Parse(line[6..]));
        static Rank GetRank(char c)
        => c switch
        {
            '2' => Rank._2,
            '3' => Rank._3,
            '4' => Rank._4,
            '5' => Rank._5,
            '6' => Rank._6,
            '7' => Rank._7,
            '8' => Rank._8,
            '9' => Rank._9,
            'T' => Rank.T,
            'J' => Rank.J,
            'Q' => Rank.Q,
            'K' => Rank.K,
            'A' => Rank.A,
            _ => throw new UnreachableException(),
        };
    }
}

enum Type : ushort
{
    High,
    One,
    Two,
    Three,
    Full,
    Four,
    Five,
}

enum Rank : ushort
{
    _2 = 2,
    _3,
    _4,
    _5,
    _6,
    _7,
    _8,
    _9,
    T,
    J,
    Q,
    K,
    A,
}
