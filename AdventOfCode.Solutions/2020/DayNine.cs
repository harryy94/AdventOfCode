using System;
using System.Collections.Generic;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayNine : BaseProblem
    {
        public DayNine() : base(2020, 9)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"1
2
3
4
5
6
7
8
9
10
11
12
13
14
15
16
17
18
19
20
21
22
23
24
25
26
49
100",
                @"35
20
15
25
47
40
62
55
65
95
102
117
150
182
127
219
299
277
309
576"
            };

        private readonly int[] _preambleCounts = {25, 5, 25};
        private int _preambleCountIndex = 0;

        public override bool DisableActual { get; } = false;

        protected override void DoSolve(string input)
        {
            var part1 = FindNonSummedNumber(input.GetLongList(), _preambleCounts[_preambleCountIndex]);
            _preambleCountIndex++;

            var part2 = FindContiguousRange(input.GetLongList(), part1);

            PartOneAnswer = part1.ToString();
            PartTwoAnswer = part2.ToString();
        }

        private long FindContiguousRange(List<long> numbers, long previousCountCheck)
        {
            for (var i = 0; i < numbers.Count; i++)
            {
                var sum = numbers[i];
                for (var j = i + 1; j < numbers.Count; j++)
                {
                    if (numbers[i] == numbers[j])
                    {
                        // Same number doesn't count
                        continue;
                    }
                    sum += numbers[j];

                    if (sum == previousCountCheck)
                    {
                        // Found range
                        return numbers[i] + numbers[j - 1];
                    }

                    if (sum > previousCountCheck)
                    {
                        // number too high
                        break;
                    }
                }
            }

            return -1;
        }

        private long FindNonSummedNumber(List<long> numbers, int previousCountCheck)
        {
            var index = 25;

            for (var i = previousCountCheck; i < numbers.Count; i++)
            {
                var paired = CheckForSummingPairs(numbers, i - previousCountCheck, i);

                if (!paired)
                {
                    return numbers[i];
                }
            }

            return -1;
        }

        private bool CheckForSummingPairs(List<long> numbers, int indexStart, int indexEnd)
        {
            for (var i = indexStart; i < indexEnd; i++)
            {
                for (var j = indexStart; j < indexEnd; j++)
                {
                    if (numbers[i] == numbers[j])
                    {
                        // Same number doesn't count
                        continue;
                    }

                    if (numbers[i] + numbers[j] == numbers[indexEnd])
                    {
                        // Valid sum
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
