using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayEight() : BaseProblem(2024, 8)
{
    public override bool RunActual { get; set; } = true;

    public override List<string> ExampleInput { get; } =
    [
        @"T.........
...T......
.T........
..........
..........
..........
..........
..........
..........
..........",
        @"..........
..........
..........
....a.....
........a.
.....a....
..........
......A...
..........
..........
",
        @"............
........0...
.....0......
.......0....
....0.......
......A.....
............
............
........A...
.........A..
............
............
",
    ];
    
    protected override void DoSolve(string input)
    {
        var inputLines = input.SplitByLine();

        var grid = AoC2DGrid.CreateWithLineInput(inputLines);
        
        var antennaGroups = new List<AntennaGroup>();
        
        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                if (grid[x, y] is '.' or '#')
                {
                    continue;
                }

                var antenna = antennaGroups.SingleOrDefault(p => p.Char == grid[x, y]);
                
                if (antenna == null)
                {
                    antenna = new AntennaGroup(grid[x, y]);
                    antennaGroups.Add(antenna);
                }
                
                antenna.Positions.Add(new GridEntry
                {
                    X = x,
                    Y = y,
                    Value = grid[x, y]
                });
            }
        }

        var hashSetPartOne = new HashSet<(int, int)>();
        var hashSetPartTwo = new HashSet<(int, int)>();
        
        foreach (var antennaGroup in antennaGroups)
        {
            foreach (var antenna in antennaGroup.Positions)
            {
                foreach (var otherAntenna in antennaGroup.Positions)
                {
                    if (antenna == otherAntenna)
                    {
                        continue;
                    }
                    
                    var xDiff = Math.Abs(antenna.X - otherAntenna.X);
                    var yDiff = Math.Abs(antenna.Y - otherAntenna.Y);
                    
                    if (antenna.X < otherAntenna.X)
                    {
                        xDiff = -xDiff;
                    }
                    if (antenna.Y < otherAntenna.Y)
                    {
                        yDiff = -yDiff;
                    }

                    var rollingDiffX = antenna.X;
                    var rollingDiffY = antenna.Y;

                    if (grid.IsInBounds(antenna.X + xDiff, antenna.Y + yDiff))
                    {
                        hashSetPartOne.Add((antenna.X + xDiff, antenna.Y + yDiff));
                    }

                    while (true)
                    {
                        if (grid.IsInBounds(rollingDiffX, rollingDiffY))
                        {
                            hashSetPartTwo.Add((rollingDiffX, rollingDiffY));
                        }
                        else
                        {
                            break;
                        }
                        
                        rollingDiffX += xDiff;
                        rollingDiffY += yDiff;
                    }
                }
            }
        }
        
        PartOneAnswer = hashSetPartOne.Count.ToString();
        PartTwoAnswer = hashSetPartTwo.Count.ToString();
    }

    private record AntennaGroup(char Char)
    {
        public List<GridEntry> Positions { get; } = new();
    }
}