using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayEight : BaseProblem
    {
        public DayEight() : base(2020, 8)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"nop +0
acc +1
jmp +4
acc +3
jmp -3
acc -99
acc +1
jmp -4
acc +6"
            };

        protected override void DoSolve(string input)
        {
            var instructions = input.SplitByLine();

            var part1 = RunProgram(instructions);

            (bool successfulRun, int accumulator) part2Result = (false, 0);

            for (var i = 0; i < instructions.Count; i++)
            {
                var oldInstruction = instructions[i];
                if (instructions[i].Substring(0, 3) == "jmp")
                {
                    instructions[i] = "nop" + instructions[i].Substring(3);
                    part2Result = RunProgram(instructions);
                }
                else if (instructions[i].Substring(0, 3) == "nop")
                {
                    instructions[i] = "jmp" + instructions[i].Substring(3);
                    part2Result = RunProgram(instructions);
                }

                // Revert back
                instructions[i] = oldInstruction;

                if (part2Result.successfulRun)
                {
                    break;
                }
            }

            PartOneAnswer = part1.accumlulator.ToString();
            PartTwoAnswer = part2Result.accumulator.ToString();
        }

        private (bool success, int accumlulator) RunProgram(List<string> instructions)
        {
            var instructionLine = 0;

            var instructionsRun = new HashSet<int>();

            var accumulator = 0;

            var runSuccessfully = true;

            var i = 0;
            while (true)
            {
                i++;
                if (i > 1000000)
                {
                    Console.WriteLine("Infinite loop detected");
                    break;
                }

                if (instructionLine == instructions.Count)
                {
                    // Natural end of program.
                    break;
                }

                if (!instructionsRun.Add(instructionLine))
                {
                    runSuccessfully = false;
                    break;
                }
                var splitter = instructions[instructionLine].Split(' ');

                switch (splitter[0])
                {
                    case "acc":
                        if (splitter[1].Substring(0, 1) == "-")
                        {
                            accumulator -= int.Parse(splitter[1].Substring(1));
                        }
                        else
                        {
                            accumulator += int.Parse(splitter[1].Substring(1));
                        }

                        instructionLine++;
                        break;
                    case "jmp":
                        if (splitter[1].Substring(0, 1) == "-")
                        {
                            instructionLine -= int.Parse(splitter[1].Substring(1));
                        }
                        else
                        {
                            instructionLine += int.Parse(splitter[1].Substring(1));
                        }

                        break;
                    default:
                        instructionLine++;
                        break;
                }
            }

            return (runSuccessfully, accumulator);
        }
    }
}
