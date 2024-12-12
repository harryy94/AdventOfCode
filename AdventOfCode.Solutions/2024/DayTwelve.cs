using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayTwelve() : BaseProblem(2024, 12)
{
    public override List<string> ExampleInput { get; } =
    [
        @"AAAA
BBCD
BBCC
EEEC
",
        @"OOOOO
OXOXO
OOOOO
OXOXO
OOOOO
",
        @"RRRRIICCFF
RRRRIICCCF
VVRRRCCFFF
VVRCCCJFFF
VVVVCJJCFE
VVIVCCJJEE
VVIIICJJEE
MIIIIIJJEE
MIIISIJEEE
MMMISSJEEE
",
    ];

    public override bool RunActual { get; set; } = true;

    protected override void DoSolve(string input)
    {
        var grid = AoC2DGrid.CreateWithLineInput(input.SplitByLine());

        var plots = new List<KeyValuePair<char, List<Coords>>>();
        
        // Find original plot groups
        for (var x = 0; x < grid.Width; x++)
        {
            for (var y = 0; y < grid.Height; y++)
            {
                var possiblePlots = plots.Where(p => p.Key == grid[x, y]).ToList();
                
                if (possiblePlots.Count == 0)
                {
                    plots.Add(new KeyValuePair<char, List<Coords>>(grid[x, y], [new Coords(x, y)]));
                }
                else
                {
                    var found = false;
                    foreach (var possiblePlot in possiblePlots)
                    {
                        foreach (var coordToCheck in possiblePlot.Value)
                        {
                            if (Math.Abs(coordToCheck.X - x) == 1 && Math.Abs(coordToCheck.Y - y) == 0)
                            {
                                found = true;
                                break;
                            }
                            
                            if (Math.Abs(coordToCheck.X - x) == 0 && Math.Abs(coordToCheck.Y - y) == 1)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found)
                        {
                            possiblePlot.Value.Add(new Coords(x, y));
                            break;
                        }
                    }

                    if (!found)
                    {
                        plots.Add(new KeyValuePair<char, List<Coords>>(grid[x, y], [new Coords(x, y)]));
                    }
                }
            }
        }
        
        // Look for any that can be merged
        foreach (var plotsLeft in plots)
        {
            if (plotsLeft.Value.Count == 0)
            {
                continue;
            }
            
            foreach (var plotsRight in plots.Where(x => 
                         x.Key == plotsLeft.Key))
            {
                if (plotsRight.Value.Count == 0)
                {
                    continue;
                }
                
                if (plotsLeft.Value == plotsRight.Value)
                {
                    continue;
                }

                var found = false;
                foreach (var plotLeft in plotsLeft.Value)
                {
                    foreach (var plotRight in plotsRight.Value)
                    {
                        if (Math.Abs(plotLeft.X - plotRight.X) == 1 && Math.Abs(plotLeft.Y - plotRight.Y) == 0)
                        {
                            found = true;
                            break;
                        }
                        if (Math.Abs(plotLeft.X - plotRight.X) == 0 && Math.Abs(plotLeft.Y - plotRight.Y) == 1)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        plotsLeft.Value.AddRange(plotsRight.Value);
                        plotsRight.Value.Clear();
                        break;
                    }
                }
            }
        }

        var results = new List<(char plots, int area, int partOnePermiter, int partTwoPermiter)>();
        
        foreach (var plot in plots)
        {
            var perimiter = 0;

            foreach (var plant in plot.Value)
            {
                perimiter += 4;
                
                if (plot.Value.Any(p => p.X == plant.X + 1 && p.Y == plant.Y))
                {
                    perimiter--;
                }
                
                if (plot.Value.Any(p => p.X == plant.X - 1 && p.Y == plant.Y))
                {
                    perimiter--;
                }
                
                if (plot.Value.Any(p => p.X == plant.X && p.Y == plant.Y + 1))
                {
                    perimiter--;
                }
                
                if (plot.Value.Any(p => p.X == plant.X && p.Y == plant.Y - 1))
                {
                    perimiter--;
                }
            }
            
            results.Add((plot.Key, plot.Value.Count, perimiter, 0));
        }
        
        PartOneAnswer = results.Sum(p => p.area * p.partOnePermiter).ToString();
    }

    public record Coords(int X, int Y);
}