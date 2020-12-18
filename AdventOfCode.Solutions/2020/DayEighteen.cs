using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayEighteen : BaseProblem
    {
        public DayEighteen() : base(2020, 18)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                "1 + 2 * 3 + 4 * 5 + 6",
                "2 * 3 + (4 * 5)",
                "5 + (8 * 3 + 9 + 3 * 4 * 3)",
                "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
                "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var sums = input.SplitByLine()
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            //var runningTotal = sums.Sum(CalculateSum);

            var part2OperatorList = new Dictionary<string, int>
            {
                {"+", 1},
                {"*", 1}
            };

            var runningTotalPart1 = 0L;
            foreach (var sum in sums)
            {
                runningTotalPart1+= SolveSum(sum, part2OperatorList);
            }

            part2OperatorList["+"] = 2;

            var runningTotalPart2 = 0L;
            foreach (var sum in sums)
            {
                runningTotalPart2 += SolveSum(sum, part2OperatorList);
            }


            PartOneAnswer = runningTotalPart1.ToString();
            PartTwoAnswer = runningTotalPart2.ToString();
        }


        private long SolveSum(string sum, Dictionary<string, int> operators)
        {
            // Solve with Shunting Yard, output being Postfix, rather than Infix

            var postFix = EvaluateToPostfix(sum, operators);

            var resultStack = new Stack<long>();

            foreach (var item in postFix)
            {
                switch (item)
                {
                    case "*":
                        resultStack.Push(resultStack.Pop() * resultStack.Pop());
                        break;
                    case "+":
                        resultStack.Push(resultStack.Pop() + resultStack.Pop());
                        break;
                    default:
                        resultStack.Push(long.Parse(item));
                        break;
                }
            }

            return resultStack.Pop();
        }
        //https://en.wikipedia.org/wiki/Shunting-yard_algorithm
        private List<string> EvaluateToPostfix(string sum, Dictionary<string, int> operators)
        {
            var tokens = sum
                .Replace("(", " ( ")
                .Replace(")", " ) ")
                .Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries);

            var stack = new Stack<string>();
            var output = new List<string>();
            foreach (var token in tokens)
            {
                if (operators.TryGetValue(token, out var op1))
                {
                    while (stack.Count > 0 && operators.TryGetValue(stack.Peek(), out var op2))
                    {
                        if (op1.CompareTo(op2) < 0)
                        {
                            output.Add(stack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }

                    stack.Push(token);
                }
                else if (token == "(")
                {
                    stack.Push(token);
                }
                else if (token == ")")
                {
                    string top;
                    while (stack.Count > 0 && (top = stack.Pop()) != "(")
                    {
                        output.Add(top);
                    }
                }
                else
                {
                    output.Add(token);
                }
            }

            while (stack.Count > 0)
            {
                output.Add(stack.Pop());
            }

            return output;
        }

    }
}
