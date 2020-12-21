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
mem[8] = 0",
                @"mask = 000000000000000000000000000000X1001X
mem[42] = 100
mask = 00000000000000000000000000000000X0XX
mem[26] = 1"
            };

        private readonly Regex _memMatchRegex = new Regex(@"^mem\[([0-9]*)\] = ([0-9]*)$");

        protected override void DoSolve(string input)
        {
            var inputLines = input.SplitByLine();

            PartOneAnswer = Part1(inputLines).ToString();

            if (inputLines[0] == "mask = XXXXXXXXXXXXXXXXXXXXXXXXXXXXX1XXXX0X")
            {
                PartTwoAnswer = "Not valid for this example";
            }
            else
            {
                PartTwoAnswer = Part2(inputLines).ToString();
            }
        }

        private long Part2(List<string> input)
        {
            var currentMask = "";

            var memory = new Dictionary<long, long>();

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.StartsWith("mask"))
                {
                    currentMask = line.Split(' ').Last();
                    continue;
                }

                var regexMatch = _memMatchRegex.Match(line);

                var base10Index = long.Parse(regexMatch.Groups[1].Value);
                var value = long.Parse(regexMatch.Groups[2].Value);

                var binaryIndex = Convert.ToString(base10Index, 2).PadLeft(currentMask.Length, '0');

                var maskedStringBuilderList = new List<StringBuilder>();
                maskedStringBuilderList.Add(new StringBuilder());
                for (var i = 0; i < currentMask.Length; i++)
                {
                    if (currentMask[i] == 'X')
                    {
                        var count = maskedStringBuilderList.Count;
                        for (var index = 0; index < count; index++)
                        {
                            var sb = maskedStringBuilderList[index];
                            var newSb = new StringBuilder();
                            newSb.Append(sb);
                            maskedStringBuilderList.Add(newSb);
                        }
                    }

                    var alternate = false;
                    for (var j = 0; j < maskedStringBuilderList.Count; j++)
                    {
                        if (currentMask[i] == 'X')
                        {
                            maskedStringBuilderList[j].Append(maskedStringBuilderList.Count / 2 > j ? 1 : 0);
                            alternate = !alternate;
                        }
                        else if (currentMask[i] == '1')
                        {
                            maskedStringBuilderList[j].Append(1);
                        }
                        else if (currentMask[i] == '0')
                        {
                            maskedStringBuilderList[j].Append(binaryIndex[i]);
                        }
                    }
                }

                foreach (var indexBuilder in maskedStringBuilderList)
                {
                    var index = Convert.ToInt64(indexBuilder.ToString(), 2);
                    memory[index] = value;
                }
            }
            return memory.Sum(x => x.Value);
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

                var regexMatch = _memMatchRegex.Match(line);

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
