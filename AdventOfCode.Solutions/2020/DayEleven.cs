using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayEleven : BaseProblem
    {
        public DayEleven() : base(2020, 11)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"L.LL.LL.LL
LLLLLLL.LL
L.L.L..L..
LLLL.LL.LL
L.LL.LL.LL
L.LLLLL.LL
..L.L.....
LLLLLLLLLL
L.LLLLLL.L
L.LLLLL.LL"
            };

        protected override void DoSolve(string input)
        {
            var grid = new List<List<GridState>>();

            foreach (var line in input.SplitByLine())
            {
                var lineList = new List<GridState>();
                foreach (var item in line)
                {
                    switch (item)
                    {
                        case 'L':
                            lineList.Add(GridState.Empty);
                            break;
                        case '.':
                            lineList.Add(GridState.Floor);
                            break;
                        default:
                            throw new InvalidOperationException("Something wrong with the input");
                    }
                }

                if (lineList.Any())
                {
                    grid.Add(lineList);
                }
            }

            var changedState = true;
            var timesRun = 0;
            while (changedState)
            {
                timesRun++;
                if (timesRun > 10000)
                {
                    break;
                }
                changedState = ApplyRulesPart1(grid);
            }

            var occupiedSeatsPart1 = grid.Sum(x => x.Count(c => c == GridState.Occupied));

            PartOneAnswer = occupiedSeatsPart1.ToString();
            PartTwoAnswer = "N.A";
        }

        public override bool RunExamples { get; set; } = true;

        private List<List<GridState>> DeepCopyList(List<List<GridState>> grid)
        {
            var newGrid = new List<List<GridState>>();

            foreach (var line in grid)
            {
                newGrid.Add(line.ToList());
            }

            return newGrid;
        }

        private bool ApplyRulesPart1(List<List<GridState>> grid)
        {
            var changedState = false;

            var snapshotGrid = DeepCopyList(grid);

            var noOccupiedSeatsDirectional = new List<ApplyRulesDirection>
            {
                new ApplyRulesDirection(0, -1),
                new ApplyRulesDirection(1, -1),
                new ApplyRulesDirection(1, 0),
                new ApplyRulesDirection(1, 1),
                new ApplyRulesDirection(0, 1),
                new ApplyRulesDirection(-1, 1),
                new ApplyRulesDirection(-1, 0),
                new ApplyRulesDirection(-1, -1)
            };

            for (var y = 0; y < grid.Count; y++)
            {
                for (var x = 0; x < grid[y].Count; x++)
                {
                    if (x == 92)
                    {
                      //  Console.WriteLine();
                    }
                    //If a seat is empty (L) and there are no occupied seats adjacent to it, the seat becomes occupied.
                    var adjacentSeats = 0;
                    foreach (var rule in noOccupiedSeatsDirectional)
                    {
                        var xDiff = x + rule.X;
                        var yDiff = y + rule.Y;

                        if (yDiff < 0 || yDiff >= snapshotGrid.Count || xDiff < 0 || xDiff >= snapshotGrid[y].Count)
                            continue;

                        var gridToCheck = snapshotGrid[yDiff][xDiff];

                        if (gridToCheck == GridState.Occupied)
                        {
                            adjacentSeats++;
                        }
                    }

                    if (snapshotGrid[y][x] == GridState.Empty && adjacentSeats == 0)
                    {
                        changedState = true;
                        grid[y][x] = GridState.Occupied;
                    }
                    
                    if (snapshotGrid[y][x] == GridState.Occupied && adjacentSeats >= 4)
                    {
                        changedState = true;
                        grid[y][x] = GridState.Empty;
                    }
                }
            }

            return changedState;
        }
    }

    public struct ApplyRulesDirection
    {
        public ApplyRulesDirection(int y, int x)
        {
            X = x;
            Y = y;
        }
        public int X { get; }

        public int Y { get; }
    }


    public enum GridState
    {
        Floor, Empty, Occupied
    }
}
