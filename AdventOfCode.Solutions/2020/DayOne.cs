using System;
using System.Collections.Generic;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayOne : BaseProblem
    {
        public DayOne() : base(2020, 1)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>()
            {
                @"1721
979
366
299
675
1456"
            };


        protected override void DoSolve(string input)
        {
            var list = input.GetIntList();
            int result;
            foreach (var a in list)
            {
                foreach (var b in list)
                {
                    result = a + b;

                    if (result == 2020 && PartOneAnswer == null)
                    {
                        Console.WriteLine("Found sum of 2020.");
                        Console.WriteLine($"{a} * {b} == 2020");

                        var answer = a * b;
                        Console.WriteLine("Answer: " + answer);

                        PartOneAnswer = answer.ToString();
                    }

                    foreach (var c in list)
                    {
                        result = a + b + c;

                        if (result == 2020 && PartTwoAnswer == null)
                        {
                            Console.WriteLine("Found sum of 2020.");
                            Console.WriteLine($"{a} * {b} * {c} == 2020");

                            var answer = a * b * c;
                            Console.WriteLine("Answer: " + answer);

                            PartTwoAnswer = answer.ToString();
                        }
                    }
                }
            }
        }
    }
}
