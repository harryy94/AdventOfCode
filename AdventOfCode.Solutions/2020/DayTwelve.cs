using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwelve : BaseProblem
    {
        public DayTwelve() : base(2020, 12)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"F10
N3
F7
R90
F11"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var instructions = new List<Instruction>();

            foreach (var item in input.SplitByLine())
            {
                if (string.IsNullOrEmpty(item))
                    continue;

                instructions.Add(new Instruction(item[0], int.Parse(item.Substring(1))));
            }

            var part1 = DoPart1(instructions);

            PartOneAnswer = part1.ToString();
            PartTwoAnswer = "N.A";
        }

        private int DoPart1(List<Instruction> instructions)
        {
            var direction = 'E';
            var x = 0;
            var y = 0;

            foreach (var instruction in instructions)
            {
                var directionToUse = instruction.InstructionType;

                if (directionToUse == 'F')
                {
                    directionToUse = direction;
                }

                if (directionToUse == 'R' || directionToUse == 'L')
                {
                    var turnsToTake = instruction.Number / 90;

                    for (var i = 0; i < turnsToTake; i++)
                    {
                        switch (direction)
                        {
                            case 'N':
                                direction = directionToUse == 'R' ? 'E' : 'W';
                                break;
                            case 'S':
                                direction = directionToUse == 'R' ? 'W' : 'E';
                                break;
                            case 'E':
                                direction = directionToUse == 'R' ? 'S' : 'N';
                                break;
                            case 'W':
                                direction = directionToUse == 'R' ? 'N' : 'S';
                                break;
                        }
                    }

                    continue;
                }

                if (directionToUse == 'N')
                {
                    y += instruction.Number;
                }
                if (directionToUse == 'S')
                {
                    y -= instruction.Number;
                }
                if (directionToUse == 'E')
                {
                    x += instruction.Number;
                }
                if (directionToUse == 'W')
                {
                    x -= instruction.Number;
                }
            }

            return Math.Abs(x) + Math.Abs(y);
        }
    }

    public enum Direction
    {
        North, East, South, West
    }

    public class Instruction
    {
        public Instruction(char instructionType, int number)
        {
            InstructionType = instructionType;
            Number = number;
        }

        public char InstructionType { get; }

        public int Number { get; }
    }
}
