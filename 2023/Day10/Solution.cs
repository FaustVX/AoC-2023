#nullable enable
namespace AdventOfCode.Y2023.Day10;

using Point = (int row, int column);
using Grid = ReadOnlySpan2D<char>;
using System.Runtime.InteropServices;

[ProblemInfo("Pipe Maze", normalizeInput: false)]
public class Solution : ISolver //, IDisplay
{
#if DEBUG
    static char At(Grid grid, Point point)
    => grid[point.row, point.column];
#endif

    public object PartOne(ReadOnlyMemory<char> input)
    {
        var grid = ParseInput(input.Span, out _);
        var (start, next1, next2) = GetOrigin(grid);
        return WalkBothDirection(start, next1, start, next2, grid) + 1;

        static int WalkBothDirection(Point previous1, Point current1, Point previous2, Point current2, Grid grid)
        {
            if (current1 == current2)
                return 0;
            return WalkBothDirection(current1, NextPoint(previous1, current1, grid), current2, NextPoint(previous2, current2, grid), grid) + 1;
        }

        static Point NextPoint(Point previous, Point current, Grid grid)
        {
            return grid[current.row, current.column] switch
            {
                '|' when Offset(previous, current) is (1, 0) => Add(current, (1, 0)),
                '|' => Add(current, (-1, 0)),
                '-' when Offset(previous, current) is (0, 1) => Add(current, (0, 1)),
                '-' => Add(current, (0, -1)),
                'L' when Offset(previous, current) is (1, 0) => Add(current, (0, 1)),
                'L' => Add(current, (-1, 0)),
                'J' when Offset(previous, current) is (1, 0) => Add(current, (0, -1)),
                'J' => Add(current, (-1, 0)),
                '7' when Offset(previous, current) is (0, 1) => Add(current, (1, 0)),
                '7' => Add(current, (0, -1)),
                'F' when Offset(previous, current) is (0, -1) => Add(current, (1, 0)),
                'F' => Add(current, (0, 1)),
                _ => throw new UnreachableException(),
            };

            static Point Offset(Point from, Point to)
            => (to.row - from.row, to.column - from.column);

            static Point Add(Point point, Point offset)
            => (point.row + offset.row, point.column + offset.column);
        }

        static (Point start, Point next1, Point next2) GetOrigin(Grid grid)
        {
            var start = grid.IndexOf('S');
            return (start, Next1(start, grid), Next2(start, grid));

            static Point Next1(Point start, Grid grid)
            {
                if (grid[start.row - 1, start.column] is '7' or 'F' or '|')
                    return (start.row - 1, start.column);
                if (grid[start.row, start.column + 1] is 'J' or '7' or '-')
                    return (start.row, start.column + 1);
                if (grid[start.row + 1, start.column] is 'J' or 'L' or '|')
                    return (start.row + 1, start.column);
                throw new UnreachableException();
            }

            static Point Next2(Point start, Grid grid)
            {
                if (grid[start.row, start.column - 1] is 'L' or 'F' or '-')
                    return (start.row, start.column - 1);
                if (grid[start.row + 1, start.column] is 'J' or 'L' or '|')
                    return (start.row + 1, start.column);
                if (grid[start.row, start.column + 1] is 'J' or '7' or '-')
                    return (start.row, start.column + 1);
                throw new UnreachableException();
            }
        }
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }

    static Span2D<char> ParseInput(ReadOnlySpan<char> input, out Span<char> span)
    {
        var size = (Globals.IsTestInput, Globals.InputFileName[^4]) switch
        {
            (false, _) => 140,
            (_, '1' or '2') => 5,
            (_, '3') => 9,
            (_, '4') => 8,
            (_, '5' or '6') => 20,
            _ => throw new UnreachableException(),
        } + 2; // +2 for the dots around

        span = MemoryMarshal.CreateSpan(ref input.DangerousGetReference(), input.Length);

        return span.AsSpan2D(size, size + 1)[.., ..^1];
    }
}
