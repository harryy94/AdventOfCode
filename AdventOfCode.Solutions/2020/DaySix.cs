using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DaySix : BaseProblem
    {
        public DaySix() : base(2020, 6)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"abc

a
b
c

ab
ac

a
a
a
a

b"
            };

        protected override void DoSolve(string input)
        {
            var hashSet = new HashSet<char>();
            var dict = new Dictionary<char, int>();

            var groupCount = 0;

            var part1SumResult = 0;
            var part2SumResult = 0;

            foreach (var item in input.SplitByLine())
            {
                if (string.IsNullOrEmpty(item))
                {
                    part1SumResult += hashSet.Count;

                    hashSet.Clear();

                    part2SumResult += ValidAnswers(groupCount, dict);

                    groupCount = 0;

                    dict.Clear();

                    continue;
                }

                groupCount++;

                foreach (var letter in item)
                {
                    hashSet.Add(letter);

                    if (dict.ContainsKey(letter))
                    {
                        dict[letter]++;
                    }
                    else
                    {
                        dict.Add(letter, 1);
                    }
                }
            }
            part1SumResult += hashSet.Count;
            part2SumResult += ValidAnswers(groupCount, dict);


            PartOneAnswer = part1SumResult.ToString();
            PartTwoAnswer = part2SumResult.ToString();
        }

        private int ValidAnswers(int count, Dictionary<char, int> dict)
        {
            return dict.Count(c => c.Value == count);
        }
    }
}
