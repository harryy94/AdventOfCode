using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2020
{
    public class DayFifteen : BaseProblem
    {
        public DayFifteen() : base(2020, 15)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                "0,3,6",
                "1,3,2",
                "2,1,3",
                "1,2,3",
                "2,3,1",
                "3,2,1",
                "3,1,2"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var numberSpoken = new Dictionary<int, NumberSpoken>();

            var startingNumbers = input.Split(',')
                .Select(int.Parse)
                .ToList();

            var numberEvaluated = 0;
            for (var i = 1; i <= 30000000; i++)
            {
                if (i <= startingNumbers.Count)
                {
                    // Use starting numbers
                    numberSpoken.Add(startingNumbers[i - 1], new NumberSpoken(i));
                    numberEvaluated = startingNumbers[i - 1];
                    continue;
                }

                numberEvaluated = numberSpoken.TryGetValue(numberEvaluated, out var numberSpokenItem)
                    ? numberSpokenItem.TurnDifference
                    : 0;

                // Store new number result
                if (numberSpoken.TryGetValue(numberEvaluated, out var newNumberItem))
                {
                    newNumberItem.SpokenAgain(i);
                }
                else
                {
                    numberSpoken.Add(numberEvaluated, new NumberSpoken(i));
                }

                if (i == 2020)
                {
                    PartOneAnswer = numberEvaluated.ToString();
                }
            }

            PartTwoAnswer = numberEvaluated.ToString();
        }
    }

    public class NumberSpoken
    {
        public NumberSpoken(int turn)
        {
            TimesSpoken = 1;
            MostRecentTurn = turn;
        }

        public void SpokenAgain(int turn)
        {
            TimesSpoken++;
            OlderTurn = MostRecentTurn;
            MostRecentTurn = turn;
        }

        public int TurnDifference
        {
            get
            {
                if (TimesSpoken == 1)
                    return 0;

                return MostRecentTurn - OlderTurn;
            }
        }

        public int TimesSpoken { get; private set; }

        public int MostRecentTurn { get; private set; }

        public int OlderTurn { get; private set; }
    }
}
