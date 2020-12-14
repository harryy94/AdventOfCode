using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayFourteen : BaseProblem
    {
        public DayFourteen() : base(2020, 14)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>()
            {
                @"mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X
mem[8] = 11
mem[7] = 101
mem[8] = 0"
            };

        private readonly Regex MemMatchRegex = new Regex(@"^mem\[([0-9]*)\] = ([0-9]*)$");

        protected override void DoSolve(string input)
        {
            var inputLines = input.SplitByLine();

            PartOneAnswer = Part1(inputLines).ToString();

            PartTwoAnswer = "N.A";
        }

        private long Part1(List<string> input)
        {
            var currentMask = "";

            var memory = new long[999999];

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.StartsWith("mask"))
                {
                    currentMask = line.Split(' ').Last();
                    continue;
                }

                var regexMatch = MemMatchRegex.Match(line);

                var index = long.Parse(regexMatch.Groups[1].Value);
                var base10Value = long.Parse(regexMatch.Groups[2].Value);

                var binary = Convert.ToString(base10Value, 2).PadLeft(currentMask.Length, '0');

                var newBinaryStringBuilder = new StringBuilder();
                for (var i = 0; i < currentMask.Length; i++)
                {
                    if (currentMask[i] == 'X')
                    {
                        newBinaryStringBuilder.Append(binary[i]);
                    }
                    else
                    {
                        newBinaryStringBuilder.Append(currentMask[i]);
                    }
                }

                var finalBase10 = Convert.ToInt64(newBinaryStringBuilder.ToString(), 2);

                memory[index] = finalBase10;
            }

            return memory.Where(x => x > 0).Sum();
        }
    }
}
