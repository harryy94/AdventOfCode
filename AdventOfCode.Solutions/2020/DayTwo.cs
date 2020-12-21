using System.Collections.Generic;
using System.Linq;
using StringSplitOptions = System.StringSplitOptions;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwo : BaseProblem
    {
        public DayTwo() : base(2020, 2)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>()
            {
                @"1-3 a: abcde
                1-3 b: cdefg
                2-9 c: ccccccccc"
            };

        protected override void DoSolve(string input)
        {
            var passwords = input.Split(new[] {"\n"}, StringSplitOptions.RemoveEmptyEntries);

            var partOneResult = 0;
            var partTwoResult = 0;

            foreach (var password in passwords)
            {
                var validation = GetValidation(password);

                var charCheck = validation.Password.Count(x => x == validation.CharCheck);

                if (charCheck >= validation.Min && charCheck <= validation.Max)
                {
                    partOneResult++;
                }

                var check = 0;

                check += validation.Password[validation.Min - 1] == validation.CharCheck ? 1 : 0;
                check += validation.Password[validation.Max - 1] == validation.CharCheck ? 1 : 0;

                if (check == 1)
                    partTwoResult++;
            }

            PartOneAnswer = partOneResult.ToString();
            PartTwoAnswer = partTwoResult.ToString();
        }

        private PasswordValidation GetValidation(string input)
        {
            var result = new PasswordValidation();

            var spaceSplit = input.Trim().Split(' ');

            // Min max
            var minMaxSplit = spaceSplit[0].Split('-');
            result.Min = int.Parse(minMaxSplit[0]);
            result.Max = int.Parse(minMaxSplit[1]);

            result.CharCheck = spaceSplit[1][0];

            result.Password = spaceSplit[2];

            return result;
        }
    }

    public class PasswordValidation
    {
        public int Min { get; set; }

        public int Max { get; set; }

        public char CharCheck { get; set; }

        public string Password { get; set; }
    }
}
