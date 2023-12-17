#nullable enable
namespace AdventOfCode.Y2023.Day11;

using CommunityToolkit.HighPerformance.Enumerables;
using Grid = ReadOnlySpan2D<char>;

[ProblemInfo("Cosmic Expansion", normalizeInput: false)]
public class Solution : ISolver //, IDisplay
{
    delegate ReadOnlyRefEnumerable<char> GetLineHandler(Grid grid, int n);

    public object PartOne(ReadOnlyMemory<char> input)
    {
        var grid = ParseInput(input.Span);
        var emptyRows = (stackalloc int[10]);
        var emptyCols = (stackalloc int[10]);
        var galaxies = (stackalloc (int x, int y)[500]);
        ParseEmptyLine(grid, ref emptyRows, static (grid, i) => grid.GetRow(i), grid.Height);
        ParseEmptyLine(grid, ref emptyCols, static (grid, i) => grid.GetColumn(i), grid.Height);
        FindGalaxies(grid, ref galaxies);
        return 0;

        static void ParseEmptyLine(Grid grid, ref Span<int> span, GetLineHandler handler, int length)
        {
            var doubledIdx = 0;
            for (var i = 0; i < length; i++)
                if (IsEmptyLine(handler(grid, i)))
                    span[doubledIdx++] = i;
            span = span[..doubledIdx];

            static bool IsEmptyLine(ReadOnlyRefEnumerable<char> line)
            {
                foreach (var c in line)
                    if (c is not '.')
                        return false;
                return true;
            }
        }

        static void FindGalaxies(Grid grid, ref Span<(int, int)> galaxies)
        {
            var idx = 0;
            for (var y = 0; y < grid.Height; y++)
                for (var x = 0; x < grid.Width; x++)
                    if (grid[y, x] is '#')
                        galaxies[idx++] = (x, y);
            galaxies = galaxies[..idx];
        }
    }

    public object PartTwo(ReadOnlyMemory<char> input)
    {
        return 0;
    }

    private static Grid ParseInput(ReadOnlySpan<char> input)
    {
        var size = Globals.IsTestInput ? 10 : 140;
        return input.AsSpan2D(size, size + 1)[.., ..^1];
    }
}
