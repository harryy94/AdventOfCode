using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2019
{
    public class DayOne : BaseProblem
    {
        public DayOne() : base(2019, 1)
        {}

        public override List<string> ExampleInput { get; }

        protected override void DoSolve(string input)
        {
            var part1FuelSum = 0;
            var part2FuelSum = 0;

            var inputList = input.GetIntList();

            foreach (var entry in inputList)
            {
                //Part 1
                var part1FuelRequired = (entry / 3) - 2;
                part1FuelSum += part1FuelRequired;

                var part2FuelRequired = entry;
                while (part2FuelRequired > 0)
                {
                    part2FuelRequired = (part2FuelRequired / 3) - 2;

                    part2FuelRequired = part2FuelRequired < 0 ? 0 : part2FuelRequired;

                    part2FuelSum += part2FuelRequired;
                }
            }

            PartOneAnswer = part1FuelSum.ToString();
            PartTwoAnswer = part2FuelSum.ToString();
        }
    }
}
