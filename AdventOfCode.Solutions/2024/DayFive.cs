using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayFive() : BaseProblem(2024, 5)
{
    public override bool RunActual { get; set; } = true;

    public override List<string> ExampleInput { get; }
        = new()
        {
            @"47|53
97|13
97|61
97|47
75|29
61|13
75|53
29|13
97|29
53|29
61|53
97|53
61|29
47|13
75|47
97|75
47|61
75|61
47|29
75|13
53|13

75,47,61,53,29
97,61,53,29,13
75,29,13
75,97,47,61,53
61,13,29
97,13,75,29,47
"
        };
    
    protected override void DoSolve(string input)
    {
        var inputSplitter = input.SplitByLine();

        var rules = new List<PrintRule>();

        var prints = new List<List<int>>();

        var counter = 0;
        foreach (var splitInput in inputSplitter)
        {
            if (splitInput.Length == 0)
            {
                counter++;
            }
            else if (counter == 0)
            {
                var split = splitInput.Split("|");
                
                var rule = rules.FirstOrDefault(x => x.Number == int.Parse(split[0]));
                if (rule == null)
                {
                    rule = new PrintRule(int.Parse(split[0]));
                    rules.Add(rule);
                }
                
                rule.MustBePrintedAfter.Add(int.Parse(split[1]));
            }
            else if (counter == 1)
            {
                var split = splitInput.Split(",");
                prints.Add(split.Select(int.Parse).ToList());
            }
            else
            {
                break;
            }
        }

        var partOnePrintsSuccessful = 0;
        
        var successfulPrints = new List<List<int>>();
        var failedPrints = new List<List<int>>();
        
        foreach (var printStream in prints)
        {
            var correctOrder = CalculatePrintFromRules(rules, printStream);

            var successful = true;
            for (var i = 0; i < correctOrder.Count; i++)
            {
                if (correctOrder[i] != printStream[i])
                {
                    successful = false;
                    break;
                }
            }
            
            if (successful)
            {
                successfulPrints.Add(printStream);
            }
            else
            {
                failedPrints.Add(printStream);
            }
        }

        PartOneAnswer = successfulPrints.Sum(x => x[x.Count / 2]).ToString();

        var fixedFailedPrints =
            failedPrints
                .Select(failedPrint => CalculatePrintFromRules(rules, failedPrint))
                .ToList();

        PartTwoAnswer = fixedFailedPrints.Sum(x => x[x.Count / 2]).ToString();
    }

    private List<int> CalculatePrintFromRules(List<PrintRule> rules, List<int> numbers)
    {
        var newOrder = new List<(int number, int howManyRules)>();
        foreach (var number in numbers)
        {
            var rule = rules.SingleOrDefault(x => x.Number == number);
                
            rule ??= new PrintRule(number);
                
            var relevantAfterRules = rule.MustBePrintedAfter.Join(numbers, x => x, x => x, (x, y) => x).ToList();
                
            newOrder.Add((number, relevantAfterRules.Count));
        }

        return newOrder
            .OrderByDescending(x => x.howManyRules)
            .Select(x => x.number)
            .ToList();
    }

    private class PrintRule(int number)
    {
        public int Number { get; set; } = number;

        public HashSet<int> MustBePrintedAfter { get; } = new();
    }
}