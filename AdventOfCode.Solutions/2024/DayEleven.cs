using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2024;

public class DayEleven() : BaseProblem(2024, 11)
{
    public override List<string> ExampleInput { get; } =
    [
        @"125 17
"
    ];

    public override bool RunExamples { get; set; } = true;
    public override bool RunActual { get; set; } = true;


    protected override void DoSolve(string input)
    {
        var stones = input.Split(' ').Select(long.Parse).ToList();

        var stoneDictionary = new Dictionary<long, long>()
        {
            {0, 0},
            {1, 0}
        };

        foreach (var stone in stones)
        {
            if (!stoneDictionary.TryAdd(stone, 1))
            {
                stoneDictionary[stone]++;
            }
        }
        
        for (var i = 0; i < 75; i++)
        {
            var newDictionary = new Dictionary<long, long>(stoneDictionary);

            foreach (var stone in stoneDictionary)
            {
                if (stone.Key == 0)
                {
                    newDictionary[0] -= stone.Value;
                    newDictionary[1] += stone.Value;
                    continue;
                }

                var keyAsString = stone.Key.ToString();

                if (keyAsString.Length % 2 == 0)
                {
                    var left = long.Parse(keyAsString.Substring(0, keyAsString.Length / 2));
                    var right = long.Parse(keyAsString.Substring(keyAsString.Length / 2));

                    newDictionary[stone.Key] -= stone.Value;
                    
                    if (!newDictionary.TryAdd(left, stone.Value))
                    {
                        newDictionary[left] += stone.Value;
                    }

                    if (!newDictionary.TryAdd(right, stone.Value))
                    {
                        newDictionary[right] += stone.Value;
                    }

                    continue;
                }

                newDictionary[stone.Key]-= stone.Value;
                if (!newDictionary.TryAdd(stone.Key * 2024, stone.Value))
                {
                    newDictionary[stone.Key * 2024] += stone.Value;
                }
            }

            stoneDictionary = newDictionary;
            
            if (i == 24)
            {
                PartOneAnswer = stoneDictionary.Sum(x => x.Value).ToString();
            }
        }

        PartTwoAnswer = stoneDictionary.Sum(x => x.Value).ToString();
    }
}