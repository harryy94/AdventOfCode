using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTen : BaseProblem
    {
        public DayTen() : base(2020, 10)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"16
10
15
5
1
11
7
19
6
12
4",
                @"28
33
18
42
31
14
46
20
48
47
24
23
49
45
19
38
39
11
1
32
25
35
8
17
7
9
4
2
34
10
3"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var joltAdapters = new List<long>
            {
                0 // The charger port
            };

            joltAdapters.AddRange(input.GetLongList().OrderBy(x => x).ToList());

            // Built in adapter (always +3)
            joltAdapters.Add(joltAdapters.Max() + 3);

            _knownCounts = new Dictionary<int, long>
            {
                [joltAdapters.Count - 1] = 0,
                [joltAdapters.Count - 2] = 1
            };

            var part2Answer = Iterate(joltAdapters, 0, 0);

            PartOneAnswer = DoPart1(joltAdapters).ToString();
            PartTwoAnswer = _knownCounts[0].ToString();

        }

        private IDictionary<int, long> _knownCounts;

        private long Iterate(List<long> input, int index, int iteration)
        {
            if (_knownCounts.ContainsKey(index))
            {
                return _knownCounts[index];
            }

            var newIterations = 0L;
            for (var i = 1; i <= 3; i++)
            {
                if (index + i >= input.Count)
                    continue;

                if (input[index + i] - input[index] <= 3)
                {
                    newIterations += Iterate(input, index + i, 0);
                }
            }

            _knownCounts.Add(index, newIterations);
            return newIterations;
        }


        private long DoPart1(List<long> joltAdapters)
        {
            var dict = new Dictionary<long, long>
            {
                {0, 0},
                {1, 0},
                {2, 0},
                {3, 0}
            };

            var joltage = 0L;

            foreach (var joltAdapter in joltAdapters)
            {
                var diff = joltAdapter - joltage;

                dict[diff]++;

                joltage += diff;
            }

            foreach (var entry in dict)
            {
                Console.WriteLine($"{entry.Key} : {entry.Value}");
            }

            return dict[1] * dict[3];
        }
    }
}
