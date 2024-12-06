using System.Collections.Generic;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayFour() : BaseProblem(2024, 4)
{
    public override List<string> ExampleInput { get; }
        = new()
        {
            @"MMMSXXMASM
MSAMXMSMSA
AMXSXMAAMM
MSAMASMSMX
XMASAMXAMM
XXAMMXXAMA
SMSMSASXSS
SAXAMASAAA
MAMMMXMMMM
MXMXAXMASX
"
        };
    protected override void DoSolve(string input)
    {
        var lines = input.SplitByLine();

        var horizontalLength = lines[0].Length;
        var verticalLength = lines.Count;

        var grid = AoC2DGrid.CreateWithLineInput(lines);
        
        var resultsFoundPartOne = 0;
        var resultsFoundPartTwo = 0;

        for (var y = 0; y < verticalLength; y++)
        {
            for (var x = 0; x < horizontalLength; x++)
            {
                resultsFoundPartOne += ScanPartOne(grid, x, y);

                if (ScanPartTwo(grid, x, y))
                {
                    resultsFoundPartTwo++;
                }
            }
        }
        
        PartOneAnswer = resultsFoundPartOne.ToString();
        PartTwoAnswer = resultsFoundPartTwo.ToString();
    }

    private bool ScanPartTwo(AoC2DGrid grid, int x, int y)
    {
        if (grid[x, y] != 'A')
        {
            return false;
        }
        
        if (x - 1 < 0 || x + 1 >= grid.Width || y - 1 < 0 || y + 1 >= grid.Height)
        {
            // Out of bounds
            return false;
        }
        
        if (!(grid[x - 1, y - 1] is 'M' or 'S' && grid[x + 1, y + 1] is 'M' or 'S' &&
            grid[x - 1, y - 1] != grid[x + 1, y + 1]))
        {
            return false;
        }
        
        if (!(grid[x - 1, y + 1] is 'M' or 'S' && grid[x + 1, y - 1] is 'M' or 'S' &&
              grid[x - 1, y + 1] != grid[x + 1, y - 1]))
        {
            return false;
        }

        return true;
    }
    
    private int ScanPartOne(AoC2DGrid grid, int x, int y)
    {
        var resultsFound = 0;
        if (grid[x, y] != 'X')
        {
            return 0;
        }

        // Above
        if (y - 3 >= 0 && 
            grid[x, y - 1] == 'M' && 
            grid[x, y - 2] == 'A' && 
            grid[x, y - 3] == 'S')
        {
           // Console.WriteLine("Above found");
            resultsFound++;
        }
        
        // Top right
        if (y - 3 >= 0 && x + 3 < grid.Width &&
            grid[x + 1, y - 1] == 'M' && 
            grid[x + 2, y - 2] == 'A' && 
            grid[x + 3, y - 3] == 'S')
        {
            resultsFound++;
        }
        
        // Right
        if (x + 3 < grid.Width &&
            grid[x + 1, y] == 'M' && 
            grid[x + 2, y] == 'A' && 
            grid[x + 3, y] == 'S')
        {
            resultsFound++;
        }
        
        // Bottom right
        if (y + 3 < grid.Height && x + 3 < grid.Width &&
            grid[x + 1, y + 1] == 'M' && 
            grid[x + 2, y + 2] == 'A' && 
            grid[x + 3, y + 3] == 'S')
        {
            resultsFound++;
        }
        
        // Bottom
        if (y + 3 < grid.Height &&
            grid[x, y + 1] == 'M' && 
            grid[x, y + 2] == 'A' && 
            grid[x, y + 3] == 'S')
        {
            resultsFound++;
        }
        
        // Bottom left
        if (y + 3 < grid.Height && x - 3 >= 0 &&
            grid[x - 1, y + 1] == 'M' && 
            grid[x - 2, y + 2] == 'A' && 
            grid[x - 3, y + 3] == 'S')
        {
            resultsFound++;
        }
        
        // Left
        if (x - 3 >= 0 &&
            grid[x - 1, y] == 'M' && 
            grid[x - 2, y] == 'A' && 
            grid[x - 3, y] == 'S')
        {
            resultsFound++;
        }
        
        // Top left
        if (y - 3 >= 0 && x - 3 >= 0 &&
            grid[x - 1, y - 1] == 'M' && 
            grid[x - 2, y - 2] == 'A' && 
            grid[x - 3, y - 3] == 'S')
        {
            resultsFound++;
        }

        return resultsFound;

    }
}