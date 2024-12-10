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

        var memoryPartOne = new List<int?>(memoryList);
        var runningLength = memoryList.Count;
        var counter = 0;
        while (counter < runningLength)
        {
            if (memoryPartOne[counter] == null)
            {
                while (memoryPartOne[runningLength - 1] == null)
                {
                    runningLength--;
                }

                memoryPartOne[counter] = memoryPartOne[runningLength - 1];
                memoryPartOne[runningLength - 1] = null;

                runningLength--;
            }

            counter++;
        }
        
        long runningTotal = 0;
        for (var i = 0; i < memoryPartOne.Count; i++)
        {
            if (memoryPartOne[i] == null)
            {
                continue;
            }

            runningTotal += i * (int)memoryPartOne[i];
        }
        
        //00...111...2...333.44.5555.6666.777.888899
        //0099811188827773336446555566
        
        // foreach (var i in memoryPartTwo)
        // {
        //     Console.Write(i == null ? "." : i.ToString());
        // }
        
        PartOneAnswer = runningTotal.ToString();
        PartTwoAnswer = "0";
    }
}






// var partOneString = sbFull.ToString();
// var runningLength = partOneString.Length;
// var counter = 0;
// var part1ResultBuilder = new StringBuilder();
// while (counter < runningLength)
// {
//     if (partOneString[counter] == '.')
//     {
//         while (partOneString[runningLength - 1] == '.')
//         {
//             runningLength--;
//         }
//         
//         part1ResultBuilder.Append(partOneString[runningLength - 1]);
//
//         runningLength--;
//     }
//     else
//     {
//         part1ResultBuilder.Append(partOneString[counter]);
//     }
//
//     counter++;
// }
// var part1Result = part1ResultBuilder.ToString();
// long runningTotal = 0;
// for (var i = 0; i < part1Result.Length; i++)
// {
//     if (part1Result[i] == '.')
//     {
//         continue;
//     }
//     
//     var add =((int)part1Result[i]) >= 46 ? (int)part1Result[i] - 1 : (int)part1Result[i];
//     
//     runningTotal += i * (int)part1Result[i];
// }