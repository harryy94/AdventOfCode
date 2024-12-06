using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayTwo() : BaseProblem(2024, 2)
{
    public override bool RunExamples { get; set; } = true;

    public override List<string> ExampleInput { get; } =
        new()
        {
            @"7 6 4 2 1
1 2 7 8 9
9 7 6 2 1
1 3 2 4 5
8 6 4 4 1
1 3 6 7 9
"
        };
    
    protected override void DoSolve(string input)
    {
        var safeReportsPartOne = 0;
        var safeReportsPartTwo = 0;

        var lines = input.SplitByLine();
        
        foreach (var line in lines.Where(x => x.Length > 0))
        {
            var numbers = line.Split(" ").ToList();

            var result = CheckLine(numbers);

            if (result.isSafe)
            {
                safeReportsPartTwo++;
                safeReportsPartOne++;
                continue;
            }

            var thisReportIsSafe = false;

            for (var i = 0; i < numbers.Count; i++)
            {
                var newNumbers = new List<string>(numbers);
                newNumbers.RemoveAt(i);
                var newResult = CheckLine(newNumbers);

                if (newResult.isSafe)
                {
                    thisReportIsSafe = true;
                    break;
                }
            }
            
            if (thisReportIsSafe)
            {
                safeReportsPartTwo++;
            }
        }
        
        PartOneAnswer = safeReportsPartOne.ToString();
        PartTwoAnswer = safeReportsPartTwo.ToString();
    }

    private (bool isSafe, int lineFailedAt) CheckLine(List<string> numbers)
    {
        bool? increasing = null;
        
        for (var i = 1; i < numbers.Count; i++)
        {
            var previousNumber = int.Parse(numbers[i - 1]);
            var currentNumber = int.Parse(numbers[i]);
                
            if (previousNumber == currentNumber)
            {
                return (false, i);
            }
                
            var currentIncreasing = currentNumber > previousNumber;
                
            if (currentIncreasing != increasing && increasing != null)
            {
                return (false, i);
            }
                
            if (increasing == true && currentNumber <= previousNumber)
            {
                return (false, i);
            }
                
            if (increasing == false && currentNumber >= previousNumber)
            {
                return (false, i);
            }
                
            increasing = currentIncreasing;
                
            if (Math.Abs(previousNumber - currentNumber) > 3)
            {
                return (false, i);
            }
        }

        return (true, 0);
    }
}