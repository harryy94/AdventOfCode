using System;
using System.Collections.Generic;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DaySix() : BaseProblem(2024, 6)
{
    public override bool RunActual { get; set; } = true;
    
    public override List<string> ExampleInput { get; } =
    [
        @"....#.....
.........#
..........
..#.......
.......#..
..........
.#..^.....
........#.
#.........
......#...
"
    ];
    
    protected override void DoSolve(string input)
    {
        var inputLines = input.SplitByLine();

        var grid = AoC2DGrid.CreateWithLineInput(inputLines);
        
        Run(grid);
        
        PartOneAnswer = grid.FindChars('X').Count.ToString();

        var infiniteLoops = 0;
        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                var newGrid = AoC2DGrid.CreateWithLineInput(inputLines);
                if (newGrid[x, y] == '.')
                {
                    newGrid[x, y] = '#';
                }

                if (Run(newGrid))
                {
                    infiniteLoops++;
                }
            }
        }
        
        PartTwoAnswer = infiniteLoops.ToString();
    }

    private bool Run(AoC2DGrid grid)
    {
        var mainCharCoords = grid.FindCharFirst('^');

        var direction = Direction.Up;
        
        var alreadyBeenSet = new HashSet<GridTrackEntry>();
        
        while (true)
        {
            var neighbour = direction switch
            {
                Direction.Up => grid.FindNeighbourAbove(mainCharCoords.X, mainCharCoords.Y),
                Direction.Down => grid.FindNeighbourBelow(mainCharCoords.X, mainCharCoords.Y),
                Direction.Left => grid.FindNeighbourLeft(mainCharCoords.X, mainCharCoords.Y),
                Direction.Right => grid.FindNeighbourRight(mainCharCoords.X, mainCharCoords.Y),
                _ => throw new ArgumentOutOfRangeException()
            };

            var set = new GridTrackEntry(mainCharCoords.X, mainCharCoords.Y, direction);
            
            if (!alreadyBeenSet.Add(set))
            {
                return true;
            }

            if (neighbour == null)
            {
                // Run off the map
                grid[mainCharCoords.X, mainCharCoords.Y] = 'X';
                return false;
            }

            if (neighbour.Value == '#')
            {
                direction++;
                if (direction > Direction.Left)
                {
                    direction = Direction.Up;
                }
            }
            else
            {
                grid[mainCharCoords.X, mainCharCoords.Y] = 'X';
                mainCharCoords = neighbour;
            }
        }

        return true;
    }

    private record struct GridTrackEntry(int x, int y, Direction direction);
    
    private enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}