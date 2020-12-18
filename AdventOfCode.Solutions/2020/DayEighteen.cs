using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var sums = input.SplitByLine();

            var runningTotal = sums.Sum(CalculateSum);

            PartOneAnswer = runningTotal.ToString();
            PartTwoAnswer = "N.A";
        }

        private long CalculateSum(string sum)
        {
            sum = sum.Replace(" ", "");

            var summingStack = new Stack<SummedScope>();

            summingStack.Push(new SummedScope(Operation.Add, 0));
            var total = 0L;
            var nextOperation = Operation.Add;
            foreach (var item in sum)
            {
                if (item == '+')
                {
                    nextOperation = Operation.Add;
                }
                else if (item == '*')
                {
                    nextOperation = Operation.Multiply;
                }
                else if (item == '(')
                {
                    summingStack.Push(new SummedScope(nextOperation, total));
                    total = 0;
                    nextOperation = Operation.Add;
                }
                else if (item == ')')
                {
                    var oldScope = summingStack.Pop();
                    var newTotal = oldScope.CurrentOperation == Operation.Add
                        ? oldScope.RunningTotal + total
                        : oldScope.RunningTotal * total;

                    total = newTotal;
                }
                else
                {
                    var nextNumber = long.Parse(item.ToString());

                    total = nextOperation == Operation.Add ? total + nextNumber : total * nextNumber;
                }
            }

            return total;
        }

        struct SummedScope
        {
            public SummedScope(Operation operation, long runningTotal)
            {
                CurrentOperation = operation;
                RunningTotal = runningTotal;
            }
            public Operation CurrentOperation { get; }

            public long RunningTotal { get; }
        }


        private enum Operation
        {
            Add, Multiply
        }

    }
}
