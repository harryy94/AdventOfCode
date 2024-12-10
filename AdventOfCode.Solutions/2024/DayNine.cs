using System.Collections.Generic;

namespace AdventOfCode.Solutions._2024;

public class DayNine() : BaseProblem(2024, 9)
{
    public override List<string> ExampleInput { get; } =
    [
        @"2333133121414131402
"
    ];

    public override bool RunActual { get; set; } = true;

    protected override void DoSolve(string input)
    {
        var currentNumber = 0;

        var memoryList = new List<int?>();

        for (var i = 0; i < input.Length - 1; i++)
        {
            if (!int.TryParse(input[i].ToString(), out _))
            {
                continue;
            }

            if (i % 2 == 0)
            {
                for (var j = 0; j < int.Parse(input[i].ToString()); j++)
                {
                    memoryList.Add(currentNumber);
                }

                currentNumber++;
            }
            else
            {
                for (var j = 0; j < int.Parse(input[i].ToString()); j++)
                {
                    memoryList.Add(null);
                }
            }
        }
        
        PartOneAnswer = SolvePartOne(memoryList).ToString();
        PartTwoAnswer = SolvePartTwo(memoryList).ToString();
    }

    private long SolvePartTwo(List<int?> initialMemoryList)
    {
        var memoryList = new List<int?>(initialMemoryList);
        var index = memoryList.Count - 1;
        
        while (index > 0)
        {
            if (memoryList[index] == null)
            {
                index--;
                continue;
            }

            var numbersToMove = 0;
            while (memoryList[index - numbersToMove] == memoryList[index])
            {
                numbersToMove++;
                
                if (index - numbersToMove < 0)
                {
                    break;
                }
            }

            var freeSpaceIndex = 0;
            var freeSpaceCount = 0;
            
            var foundFreeSpace = false;
            while (freeSpaceIndex < index)
            {
                if (memoryList[freeSpaceIndex] == null)
                {
                    freeSpaceCount++;
                    
                    if (freeSpaceCount == numbersToMove)
                    {
                        foundFreeSpace = true;
                        break;
                    }
                }
                else
                {
                    freeSpaceCount = 0;
                }

                freeSpaceIndex++;
            }

            if (foundFreeSpace)
            {
                freeSpaceIndex -= numbersToMove;
                for (var i = 0; i < numbersToMove; i++)
                {
                    memoryList[freeSpaceIndex + i + 1] = memoryList[index];
                    memoryList[index] = null;
                    index--;
                }

                continue;
            }

            index -= numbersToMove;
        }
        
        long runningTotal = 0;
        for (var i = 0; i < memoryList.Count; i++)
        {
            if (memoryList[i] == null)
            {
                continue;
            }

            runningTotal += i * (int)memoryList[i];
        }

        return runningTotal;
    }
    

    private long SolvePartOne(List<int?> initialMemoryList)
    {
        var memoryList = new List<int?>(initialMemoryList);
        var runningLength = memoryList.Count - 1;
        var counter = 0;
        while (counter < runningLength)
        {
            if (memoryList[counter] == null)
            {
                while (memoryList[runningLength] == null)
                {
                    runningLength--;
                }
                
                memoryList[counter] = memoryList[runningLength];
                memoryList[runningLength] = null;

                runningLength--;
            }

            counter++;
        }
        
        long runningTotal = 0;
        for (var i = 0; i < memoryList.Count; i++)
        {
            if (memoryList[i] == null)
            {
                continue;
            }

            runningTotal += i * (int)memoryList[i];
        }

        return runningTotal;
    }
}