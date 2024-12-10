using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayTen() : BaseProblem(2024, 10)
{
    public override List<string> ExampleInput { get; } =
    [
        @"89010123
78121874
87430965
96549874
45678903
32019012
01329801
10456732
"
    ];

    protected override void DoSolve(string input)
    {
        var inputLines = input.SplitByLine();
        
        var grid = AoC2DGrid.CreateWithLineInput(inputLines);
        
        var partOneAnswer = new List<(int x, int y)>();
        var partTwoAnswer = new List<(int x, int y)>();
        
        for (var y = 0; y < grid.Height; y++)
        {
            for (var x = 0; x < grid.Width; x++)
            {
                if (grid[x, y] == '0')
                {
                    var entry = new GridEntry
                    {
                        X = x,
                        Y = y,
                        Value = '0'
                    };
                    var entriesToStore = FindNextInTrail(grid, entry);
                    
                    var hashSet = new HashSet<(int x, int y)>();
                    foreach (var entryToStore in entriesToStore)
                    {
                        hashSet.Add((entryToStore.X, entryToStore.Y));
                    }
                    
                    partOneAnswer.AddRange(hashSet.ToList());
                    partTwoAnswer.AddRange(entriesToStore.Select(p => (p.X, p.Y)));
                }
            }
        }

        PartOneAnswer = partOneAnswer.Count.ToString();
        PartTwoAnswer = partTwoAnswer.Count.ToString();
    }

    private List<GridEntry> FindNextInTrail(AoC2DGrid grid, GridEntry entry)
    {
        var left = grid.FindNeighbourLeft(entry.X, entry.Y);
        var right = grid.FindNeighbourRight(entry.X, entry.Y);
        var up = grid.FindNeighbourAbove(entry.X, entry.Y);
        var down = grid.FindNeighbourBelow(entry.X, entry.Y);
        
        var nextValue = int.Parse(entry.Value.ToString()) + 1;

        var results = new List<GridEntry>();
        
        if (left != null && int.Parse(left.Value.ToString()) == nextValue)
        {
            if (nextValue == 9)
            {
                results.Add(left with { Value = '9' });
            }
            else
            {
                results.AddRange(FindNextInTrail(grid, left));
            }
        }
        if (right != null && int.Parse(right.Value.ToString()) == nextValue)
        {
            if (nextValue == 9)
            {
                results.Add(right with { Value = '9' });
            }
            else
            {
                results.AddRange(FindNextInTrail(grid, right));
            }
        }
        if (up != null && int.Parse(up.Value.ToString()) == nextValue)
        {
            if (nextValue == 9)
            {
                results.Add(up with { Value = '9' });
            }
            else
            {
                results.AddRange(FindNextInTrail(grid, up));
            }
        }
        
        if (down != null && int.Parse(down.Value.ToString()) == nextValue)
        {
            if (nextValue == 9)
            {
                results.Add(down with { Value = '9' });
            }
            else
            {
                results.AddRange(FindNextInTrail(grid, down));
            }
        }
        
        return results;
    }
}