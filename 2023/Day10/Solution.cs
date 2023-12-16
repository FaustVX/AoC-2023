#nullable enable
namespace AdventOfCode.Y2023.Day10;

using Point = (int row, int column);
using ROGrid = ReadOnlySpan2D<char>;
using Grid = Span2D<char>;
using System.Runtime.InteropServices;

[ProblemInfo("Pipe Maze", normalizeInput: false)]
public class Solution : ISolver //, IDisplay
{
    public object PartOne(ReadOnlyMemory<char> input)
    {
#if DEBUG
        return Globals.ExpectedOutput ?? "0";
#endif
        var grid = ParseInput(input.Span);
        var (start, next1, next2) = GetOrigin(grid);
        return WalkBothDirection(start, next1, start, next2, grid) + 1;

        static int WalkBothDirection(Point previous1, Point current1, Point previous2, Point current2, ROGrid grid)
        {
            if (current1 == current2)
                return 0;
            return WalkBothDirection(current1, NextPoint(previous1, current1, grid), current2, NextPoint(previous2, current2, grid), grid) + 1;
        }

        static Point NextPoint(Point previous, Point current, ROGrid grid)
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

        static (Point start, Point next1, Point next2) GetOrigin(ROGrid grid)
        {
            var start = grid.IndexOf('S');
            return (start, Next1(start, grid), Next2(start, grid));

            static Point Next1(Point start, ROGrid grid)
            {
                if (grid[start.row - 1, start.column] is '7' or 'F' or '|')
                    return (start.row - 1, start.column);
                if (grid[start.row, start.column + 1] is 'J' or '7' or '-')
                    return (start.row, start.column + 1);
                if (grid[start.row + 1, start.column] is 'J' or 'L' or '|')
                    return (start.row + 1, start.column);
                throw new UnreachableException();
            }

            static Point Next2(Point start, ROGrid grid)
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
        var grid = ParseInput(input.Span);
        ReplaceLoop(grid);
        File.WriteAllText(Path.Combine("2023", "Day10", Path.ChangeExtension(Globals.InputFileName, "txt")), input.Span.ToString());
        return ScanLine(grid);

        static void ReplaceLoop(Grid grid)
        {
            var (start, next, pipe) = GetOrigin(grid);
            var previous = start;
            At(grid, start) = pipe;
            while (next != start)
            {
                Replace(ref At(grid, previous));
                (previous, next) = (next, NextPoint(previous, next, grid));
            }
            Replace(ref At(grid, previous));

            static void Replace(ref char c)
            => c = c switch
            {
                '|' => '┃',
                '-' => '━',
                'L' => '┗',
                'J' => '┛',
                '7' => '┓',
                'F' => '┏',
                _ => throw new UnreachableException(),
            };
        }

        static int ScanLine(ROGrid grid)
        {
            var sum = 0;
            for (var y = 0; y < grid.Height; y++)
            {
                var inside = false;
                var lastCorner = '\0';
                for (var x = 0; x < grid.Width; x++)
                    switch (At(grid, (y, x)))
                    {
                        case '┃':
                            inside ^= true; // a xor b to invert a only if b is true
                            break;
                        case ('┗' or '┏') and var start:
                            lastCorner = start;
                            break;
                        case '┛':
                            if (lastCorner == '┏')
                                inside ^= true;
                            lastCorner = '\0';
                            break;
                        case '┓':
                            if (lastCorner == '┗')
                                inside ^= true;
                            lastCorner = '\0';
                            break;
                        case '━':
                            break;
                        case '.' or 'L' or 'J' or 'F' or '7' or '-' or '|' when inside:
                            sum++;
                            break;
                    }
            }
            return sum;
        }

        static Point NextPoint(Point previous, Point current, ROGrid grid)
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

            static Point Add(Point point, Point offset)
            => (point.row + offset.row, point.column + offset.column);
        }

        static Point Offset(Point from, Point to)
        => (to.row - from.row, to.column - from.column);

        static (Point start, Point next, char pipe) GetOrigin(ROGrid grid)
        {
            var start = grid.IndexOf('S');
            var next1 = Next1(start, grid);
            var next2 = Next2(start, grid);
            var pipe = (Offset(start, next1), Offset(start, next2)) switch
            {
                ((-1, 0), (0, 1)) => 'L',
                ((1, 0),(0, -1)) => '7',
                ((-1, 0), (0, -1)) => 'J',
                ((0, 1), (1, 0)) => 'F',
                ((-1, 0), (1, 0)) => '|',
                ((0, 1), (0, -1)) => '-',
                _ => throw new UnreachableException(),
            };
            return (start, next1, pipe);

            static Point Next1(Point start, ROGrid grid)
            {
                if (grid[start.row - 1, start.column] is '7' or 'F' or '|')
                    return (start.row - 1, start.column);
                if (grid[start.row, start.column + 1] is 'J' or '7' or '-')
                    return (start.row, start.column + 1);
                if (grid[start.row + 1, start.column] is 'J' or 'L' or '|')
                    return (start.row + 1, start.column);
                throw new UnreachableException();
            }

            static Point Next2(Point start, ROGrid grid)
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

    static ref char At(Grid grid, Point point)
    => ref grid[point.row, point.column];

    static char At(ROGrid grid, Point point)
    => grid[point.row, point.column];

    static Grid ParseInput(ReadOnlySpan<char> input)
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

        var span = MemoryMarshal.CreateSpan(ref input.DangerousGetReference(), input.Length); // Convert ROSpan to Span

        return span.AsSpan2D(size, size + 1)[.., ..^1];
    }
}
