using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DaySeventeen : BaseProblem
    {
        public DaySeventeen() : base(2020, 17)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @".#.
..#
###"
            };
        
        protected override void DoSolve(string input)
        {
            var part1 = DoPart1(input);

            PartOneAnswer = part1.ToString();
            PartTwoAnswer = "N/A";
        }

        private int DoPart1(string input)
        {
            var entries = new List<DataEntry>();
            var x = 0;
            foreach (var line in input.SplitByLine())
            {
                var y = 0;
                foreach (var item in line)
                {
                    if (item == '#')
                    {
                        entries.Add(new DataEntry(x, y, 0));
                    }

                    y++;
                }

                x++;
            }

            for (var i = 0; i < 6; i++)
            {
                entries = RunCycle(entries);
            }

            return entries.Count;
        }

        private List<DataEntry> RunCycle(List<DataEntry> activeEntries)
        {
            var newEntries = new List<DataEntry>();

            foreach (var entry in activeEntries)
            {
                var neighbourCount = GetActiveNeighbours(activeEntries, entry);
                if (neighbourCount >= 2 && neighbourCount <= 3)
                {
                    newEntries.Add(entry);
                }

                //Check inactive neighbours to see if they can be made active

                for (var xOffset = -1; xOffset <= 1; xOffset++)
                {
                    for (var yOffset = -1; yOffset <= 1; yOffset++)
                    {
                        for (var zOffset = -1; zOffset <= 1; zOffset++)
                        {
                            var possibleEntry = new DataEntry(
                                entry.X + xOffset,
                                entry.Y + yOffset,
                                entry.Z + zOffset
                            );

                            if (!newEntries.Contains(possibleEntry) && !activeEntries.Contains(possibleEntry))
                            {
                                var neighbours = GetActiveNeighbours(activeEntries, possibleEntry);

                                if (neighbours == 3)
                                {
                                    newEntries.Add(possibleEntry);
                                }
                            }
                        }
                    }
                }
            }

            return newEntries;
        }

        private int GetActiveNeighbours(List<DataEntry> activeEntries, DataEntry entry)
        {
            return activeEntries
                .Count(c => c.X >= entry.X - 1 && c.X <= entry.X + 1 &&
                                c.Y >= entry.Y - 1 && c.Y <= entry.Y + 1 &&
                                c.Z >= entry.Z - 1 && c.Z <= entry.Z + 1 &&
                                !Equals(c, entry));
        }

        private void DoPart2()
        {

        }
    }

    public struct DataEntry
    {
        public DataEntry(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }
    }

}
