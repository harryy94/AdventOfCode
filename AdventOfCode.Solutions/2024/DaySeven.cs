using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DaySeven() : BaseProblem(2024, 7)
{
    public override bool RunActual { get; set; } = true;

    public override List<string> ExampleInput { get; }
        = new()
        {
            @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
"
        };
    
    protected override void DoSolve(string input)
    {
        var inputSplitter = input.SplitByLine()
            .Where(x => x.Length > 0)
            .ToList();

        var sums = new List<Sum>();
        
        foreach (var splitInput in inputSplitter)
        {
            var answerSplit = splitInput.Split(": ");

            var numbers = answerSplit[1].Split(' ')
                .Select(long.Parse)
                .ToList();
            
            sums.Add(new Sum(long.Parse(answerSplit[0]), numbers));
        }

        long totalSumPartOne = 0;
        long totalSumPartTwo = 0;

        var partOneDictionaryCache = new Dictionary<int, List<List<Operations>>>();
        var partTwoDictionaryCache = new Dictionary<int, List<List<Operations>>>();
        
        foreach (var sum in sums)
        {
            if (!partOneDictionaryCache.TryGetValue(sum.Numbers.Count - 1, out var opCombinationsPartOne))
            {
                opCombinationsPartOne = GetOperationCombinations(sum.Numbers.Count - 1, 2);
                partOneDictionaryCache.Add(sum.Numbers.Count - 1, opCombinationsPartOne);
            }
            
            if (!partTwoDictionaryCache.TryGetValue(sum.Numbers.Count - 1, out var opCombinationsPartTwo))
            {
                opCombinationsPartTwo = GetOperationCombinations(sum.Numbers.Count - 1, 3);
                partTwoDictionaryCache.Add(sum.Numbers.Count - 1, opCombinationsPartTwo);
            }

            var isMatchPartOne = false;
            foreach (var opCombo in opCombinationsPartOne)
            {
                var runningTotal = sum.Numbers[0];
                for (var i = 1; i < sum.Numbers.Count; i++)
                {
                    if (opCombo[i - 1] == Operations.Add)
                    {
                        runningTotal += sum.Numbers[i];
                    }
                    else
                    {
                        runningTotal *= sum.Numbers[i];
                    }
                }

                if (runningTotal == sum.Total)
                {
                    isMatchPartOne = true;
                }
            }

            if (isMatchPartOne)
            {
                totalSumPartOne += sum.Total;
            }
            
            var isMatchPartTwo = false;
            foreach (var opCombo in opCombinationsPartTwo)
            {
                var runningTotal = sum.Numbers[0];
                for (var i = 1; i < sum.Numbers.Count; i++)
                {
                    if (opCombo[i - 1] == Operations.Add)
                    {
                        runningTotal += sum.Numbers[i];
                    }
                    else if (opCombo[i - 1] == Operations.Multiply)
                    {
                        runningTotal *= sum.Numbers[i];
                    }
                    else
                    {
                        runningTotal = long.Parse(runningTotal.ToString() + sum.Numbers[i]);
                    }
                }

                if (runningTotal == sum.Total)
                {
                    isMatchPartTwo = true;
                }
            }

            if (isMatchPartTwo)
            {
                totalSumPartTwo += sum.Total;
            }
        }

        PartOneAnswer = totalSumPartOne.ToString();
        PartTwoAnswer = totalSumPartTwo.ToString();
    }

    private List<List<Operations>> GetOperationCombinations(int numberOf, int howManyOps)
    {
        var result = new List<List<Operations>>();
        
        for (var i = 0; i < Math.Pow(howManyOps, numberOf); i++)
        {
            var binary = i.ConvertToBase(howManyOps).PadLeft(numberOf, '0');
            var operationCombos = new List<Operations>();
            foreach (var c in binary)
            {
                switch (c)
                {
                    case '0':
                        operationCombos.Add(Operations.Add);
                        break;
                    case '1':
                        operationCombos.Add(Operations.Multiply);
                        break;
                    case '2':
                        operationCombos.Add(Operations.Concatenate);
                        break;
                }
            }
            result.Add(operationCombos);
        }
    
        return result;
    }

    private record Sum(long Total, List<long> Numbers);
    
    private enum Operations
    {
        Add,
        Multiply,
        Concatenate
    }
}