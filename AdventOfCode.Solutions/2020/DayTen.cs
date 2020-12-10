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

        public override bool RunActual { get; set; } = false;

        protected override void DoSolve(string input)
        {
            var joltAdapters = input.GetIntList().OrderBy(x => x).ToList();

            // Built in adapter
            joltAdapters.Add(joltAdapters.Max() + 3);

            

            PartOneAnswer = DoPart1(joltAdapters).ToString();
            PartTwoAnswer = FindIterations(joltAdapters).ToString();
        }

        private int FindIterations(List<int> input)
        {
            var iterations = 1;
            for (var i = 0; i < input.Count; i++)
            {
                var newIterations = 0;
                for (var j = 1; j <= 3; j++)
                {
                    if (i + j >= input.Count)
                        continue;

                    var diff = input[i + j] - input[i];

                    if (diff >= 1 && diff <= 3)
                    {
                        newIterations++;
                    }
                }

                if (newIterations > 0)
                {
                    iterations = newIterations * iterations;
                }
            }

            return iterations;
        }

        private int DoPart1(List<int> joltAdapters)
        {
            var dict = new Dictionary<int, int>
            {
                {1, 0},
                {2, 0},
                {3, 0}
            };

            var joltage = 0;

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
