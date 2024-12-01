using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayOne : BaseProblem
{
    public DayOne() : base(2024, 1)
    {
    }

    public override bool RunActual => true;

    public override List<string> ExampleInput { get; } = new()
    {
        @"3   4
4   3
2   5
1   3
3   9
3   3
"
    };
    protected override void DoSolve(string input)
    {
        var listA = new List<int>();
        var listB = new List<int>();
        
        foreach (var line in input.SplitByLine())
        {
            if (line.Length == 0)
            {
                continue;
            }
            
            var split = line.Split("   ");
            listA.Add(int.Parse(split[0]));
            listB.Add(int.Parse(split[1]));
        }
        
        listA = listA.OrderBy(x => x).ToList();
        listB = listB.OrderBy(x => x).ToList();

        var runningResult = 0;
        var runningSimilarity = 0;
        
        for (var i = 0; i < listA.Count; i++)
        {
            var timesInRightList = listB.Count(x => x == listA[i]);
            runningSimilarity += listA[i] * timesInRightList;
            
            if (listB[i] < listA[i])
            {
                runningResult += listA[i] - listB[i];
            }
            else
            {
                runningResult += listB[i] - listA[i];
            }
        }

        PartOneAnswer = runningResult.ToString();
        PartTwoAnswer = runningSimilarity.ToString();
    }
}