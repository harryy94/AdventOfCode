using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayFive : BaseProblem
{
    public DayFive() : base(2024, 5)
    {
    }

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
                
                var rule1 = rules.FirstOrDefault(x => x.Number == int.Parse(split[0]));
                if (rule1 == null)
                {
                    rule1 = new PrintRule(int.Parse(split[0]));
                    rules.Add(rule1);
                }
                
                rule1.MustBePrintedAfter.Add(int.Parse(split[1]));
                
                var rule2 = rules.FirstOrDefault(x => x.Number == int.Parse(split[1]));
                if (rule2 == null)
                {
                    rule2 = new PrintRule(int.Parse(split[1]));
                    rules.Add(rule2);
                }
                
                rule2.MustBePrintedBefore.Add(int.Parse(split[0]));
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
        
        var failedPrints = new List<List<int>>();
        
        foreach (var printStream in prints)
        {
            var printSuccess = true;
            var alreadyPrinted = new List<int>();
            foreach (var print in printStream)
            {
                var rule = rules.SingleOrDefault(x => x.Number == print);

                if (rule == null)
                {
                    continue;
                }

                var relevantAfterRules = rule.MustBePrintedAfter.Join(printStream, x => x, x => x, (x, y) => x).ToList();
                
                if (alreadyPrinted.Any(x => relevantAfterRules.Contains(x)))
                {
                    printSuccess = false;
                    break;
                }
                
                alreadyPrinted.Add(print);
            }

            if (printSuccess)
            {
                partOnePrintsSuccessful += printStream[printStream.Count / 2];
            }
            else
            {
                failedPrints.Add(printStream);
            }
        }
        
        PartOneAnswer = partOnePrintsSuccessful.ToString();
        
        var partTwoPrintsSuccessful = 0;
        foreach (var failedPrint in failedPrints)
        {
            var newOrder = new List<(int number, int howManyRules)>();
            foreach (var number in failedPrint)
            {
                var rule = rules.Single(x => x.Number == number);
                
                var relevantAfterRules = rule.MustBePrintedAfter.Join(failedPrint, x => x, x => x, (x, y) => x).ToList();
                
                newOrder.Add((number, relevantAfterRules.Count));
            }

            newOrder = newOrder.OrderBy(x => x.howManyRules).ToList();
            partTwoPrintsSuccessful += newOrder[newOrder.Count / 2].number;
        }
        
        PartTwoAnswer = partTwoPrintsSuccessful.ToString();

        Console.WriteLine("Got input");
    }

    private class PrintRule
    {
        public PrintRule(int number)
        {
            Number = number;
        }
        
        public int Number { get; set; }

        public HashSet<int> MustBePrintedAfter { get; } = new();
        
        public HashSet<int> MustBePrintedBefore { get; } = new();
    }
}