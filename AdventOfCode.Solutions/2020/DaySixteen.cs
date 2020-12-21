using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DaySixteen : BaseProblem
    {
        public DaySixteen() : base(2020, 16)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"class: 1-3 or 5-7
row: 6-11 or 33-44
seat: 13-40 or 45-50

your ticket:
7,1,14

nearby tickets:
7,3,47
40,4,50
55,2,20
38,6,12",
                @"class: 0-1 or 4-19
row: 0-5 or 8-19
seat: 0-13 or 16-19

your ticket:
11,12,13

nearby tickets:
3,9,18
15,1,5
5,14,9"
            };

        private readonly Regex _validationMatcher = new Regex("^([A-z| ]*): ([0-9]*)-([0-9]*) or ([0-9]*)-([0-9]*)$");

        protected override void DoSolve(string input)
        {
            var step = 0;

            var rules = new List<ValidationRule>();

            var myTicket = new List<int>();

            var otherTickets = new List<List<int>>();

            foreach (var line in input.SplitByLine())
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.Trim() == "your ticket:")
                {
                    step = 1;
                }

                else if (line.Trim() == "nearby tickets:")
                {
                    step = 2;
                }

                else if (step == 0)
                {
                    var matching = _validationMatcher.Match(line);

                    rules.Add(new ValidationRule(
                        matching.Groups[1].Value,
                        int.Parse(matching.Groups[2].Value),
                        int.Parse(matching.Groups[3].Value),
                        int.Parse(matching.Groups[4].Value),
                        int.Parse(matching.Groups[5].Value)
                    ));
                }
                else if (step == 1)
                {
                    myTicket.AddRange(line.Split(',').Select(int.Parse));
                }
                else if (step == 2)
                {
                    var newTicket = line.Split(',').Select(int.Parse).ToList();
                    otherTickets.Add(newTicket);
                }
            }

            var part1 = SolvePart1(rules, otherTickets);
            PartOneAnswer = part1.scanningErrorRate.ToString();
            PartTwoAnswer = SolvePart2(rules, myTicket, part1.validatedRows).ToString();
        }

        private long SolvePart2(List<ValidationRule> rules, List<int> myTicket, List<List<int>> nearbyTickets)
        {
            var resultDictionary = new Dictionary<string, long>();
            var ruleDictionary = rules.ToDictionary(k => k, v => true);
            while (resultDictionary.Count < myTicket.Count)
            {
                for (var i = 0; i < myTicket.Count; i++)
                {
                    for (var j = 0; j < ruleDictionary.Count; j++)
                    {
                        var dictionaryEntry = ruleDictionary.ElementAt(j);
                        if (!dictionaryEntry.Value)
                        {
                            continue;
                        }

                        for (var k = 0; k < nearbyTickets.Count; k++)
                        {
                            if (!dictionaryEntry.Key.Validate(nearbyTickets[k][i]))
                            {
                                ruleDictionary[dictionaryEntry.Key] = false;
                                break;
                            }
                        }
                    }

                    if (ruleDictionary.Count(x => x.Value) == 1)
                    {
                        // one rule left, must be that
                        resultDictionary.Add(ruleDictionary.First(x => x.Value).Key.Name, myTicket[i]);
                    }

                    foreach (var key in ruleDictionary.Keys.ToList())
                    {
                        // Reset everything
                        if (resultDictionary.All(a => a.Key != key.Name))
                        {
                            ruleDictionary[key] = true;
                        }
                        else
                        {
                            ruleDictionary[key] = false;
                        }
                    }
                }
            }

            return resultDictionary
                .Where(x => x.Key.StartsWith("departure"))
                .Aggregate(1L, (current, item) => current * item.Value);
        }

        private (List<List<int>> validatedRows, int scanningErrorRate) SolvePart1(List<ValidationRule> rules, List<List<int>> nearbyTickets)
        {
            var scanningErrorRate = 0;
            var newList = new List<List<int>>();
            foreach (var ticket in nearbyTickets)
            {
                var failed = false;
                foreach (var item in ticket)
                {
                    if (!rules.Any(a => a.Validate(item)))
                    {
                        failed = true;
                        scanningErrorRate += item;
                    }
                }

                if (!failed)
                {
                    newList.Add(ticket);
                }
            }

            return (newList, scanningErrorRate);
        }

        class ValidationRule
        {
            public ValidationRule(
                string name,
                int firstLowerRange,
                int firstUpperRange, 
                int secondLowerRange, 
                int secondUpperRange)
            {
                Name = name;
                FirstLowerRange = firstLowerRange;
                FirstUpperRange = firstUpperRange;
                SecondLowerRange = secondLowerRange;
                SecondUpperRange = secondUpperRange;
            }
            public string Name { get; }

            public int FirstLowerRange { get; }

            public int FirstUpperRange { get; }

            public int SecondLowerRange { get; }

            public int SecondUpperRange { get; }

            public bool Validate(int number)
            {
                return (number >= FirstLowerRange && number <= FirstUpperRange) ||
                       (number >= SecondLowerRange && number <= SecondUpperRange);
            }
        }

    }
}
