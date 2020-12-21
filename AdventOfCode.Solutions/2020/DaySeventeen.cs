using System.Collections.Generic;
using System.Linq;
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
            var part2 = DoPart2(input);

            PartOneAnswer = part1.ToString();
            PartTwoAnswer = part2.ToString();
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
                entries = RunPart1Cycle(entries);
            }

            return entries.Count;
        }

        private List<DataEntry> RunPart1Cycle(List<DataEntry> activeEntries)
        {
            var newEntries = new List<DataEntry>();

            foreach (var entry in activeEntries)
            {
                var neighbourCount = GetActiveNeighbours(activeEntries, entry, false);
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
                                var neighbours = GetActiveNeighbours(activeEntries, possibleEntry, false);

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

        private int DoPart2(string input)
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
                        entries.Add(new DataEntry(x, y, 0, 0));
                    }

                    y++;
                }

                x++;
            }

            for (var i = 0; i < 6; i++)
            {
                entries = RunPart2Cycle(entries);
            }

            return entries.Count;
        }

        private List<DataEntry> RunPart2Cycle(List<DataEntry> activeEntries)
        {
            var newEntries = new List<DataEntry>();

            foreach (var entry in activeEntries)
            {
                var neighbourCount = GetActiveNeighbours(activeEntries, entry, true);
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
                            for (var wOffset = -1; wOffset <= 1; wOffset++)
                            {
                                var possibleEntry = new DataEntry(
                                    entry.X + xOffset,
                                    entry.Y + yOffset,
                                    entry.Z + zOffset,
                                    entry.W + wOffset
                                );

                                if (!newEntries.Contains(possibleEntry) && !activeEntries.Contains(possibleEntry))
                                {
                                    var neighbours = GetActiveNeighbours(activeEntries, possibleEntry, true);

                                    if (neighbours == 3)
                                    {
                                        newEntries.Add(possibleEntry);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return newEntries;
        }

        private int GetActiveNeighbours(List<DataEntry> activeEntries, DataEntry entry, bool includeFourthDimension)
        {
            var countQuery = activeEntries
                .Where(c => c.X >= entry.X - 1 && c.X <= entry.X + 1 &&
                            c.Y >= entry.Y - 1 && c.Y <= entry.Y + 1 &&
                            c.Z >= entry.Z - 1 && c.Z <= entry.Z + 1 &&
                            !Equals(c, entry));

            if (includeFourthDimension)
            {
                countQuery = countQuery.Where(c => c.W >= entry.W - 1 && c.W <= entry.W + 1);
            }

            return countQuery.Count();
        }
    }

    public struct DataEntry
    {
        public DataEntry(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            W = 0;
        }
        public DataEntry(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public int X { get; }

        public int Y { get; }

        public int Z { get; }
        
        public int W { get; }
    }

}
