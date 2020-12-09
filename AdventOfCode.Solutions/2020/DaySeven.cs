using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DaySeven : BaseProblem
    {
        public DaySeven() : base(2020, 7)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"light red bags contain 1 bright white bag, 2 muted yellow bags.
dark orange bags contain 3 bright white bags, 4 muted yellow bags.
bright white bags contain 1 shiny gold bag.
muted yellow bags contain 2 shiny gold bags, 9 faded blue bags.
shiny gold bags contain 1 dark olive bag, 2 vibrant plum bags.
dark olive bags contain 3 faded blue bags, 4 dotted black bags.
vibrant plum bags contain 5 faded blue bags, 6 dotted black bags.
faded blue bags contain no other bags.
dotted black bags contain no other bags.",
                @"shiny gold bags contain 2 dark red bags.
dark red bags contain 2 dark orange bags.
dark orange bags contain 2 dark yellow bags.
dark yellow bags contain 2 dark green bags.
dark green bags contain 2 dark blue bags.
dark blue bags contain 2 dark violet bags.
dark violet bags contain no other bags."
            };

        protected override void DoSolve(string input)
        {
            var lines = input.SplitByLine();

            _bagRules = new Dictionary<string, Bag>();

            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;
                var leftToRightSplit = line.Split(new string[] { "contain" }, StringSplitOptions.RemoveEmptyEntries);
                if (leftToRightSplit[1].Trim() == "no other bags.")
                {
                    continue;
                }

                var leftBag = GetOrNew(leftToRightSplit[0]
                    .Replace(".", "")
                    .Replace("bags", "")
                    .Replace("bag", "")
                    .Trim());

                foreach (var item in leftToRightSplit[1].Split(','))
                {
                    var rightBagString = item
                        .Replace(".", "")
                        .Replace("bags", "")
                        .Replace("bag", "")
                        .Trim()
                        .Substring(2);

                    var quantity = int.Parse(item.Substring(0, 2));

                    leftBag.Rules.Add(new BagRule(GetOrNew(rightBagString), quantity));
                }
            }

            var part1Result = FindBagRuleHits("shiny gold");
            var part2Result = CountHowManyBagsWithin(GetOrNew("shiny gold"));

            PartOneAnswer = part1Result.ToString();
            PartTwoAnswer = part2Result.ToString();
        }

        private int CountHowManyBagsWithin(Bag bag)
        {
            var hitCount = 0;

            foreach (var item in bag.Rules)
            {
                hitCount += item.Quantity;
                hitCount += CountHowManyBagsWithin(item.Contains) * item.Quantity;
            }

            return hitCount;
        }

        private int FindBagRuleHits(string bagName)
        {
            var bagHits = 0;

            var hashSet = new HashSet<string>();

            foreach (var rule in _bagRules)
            {
                if (rule.Key == bagName)
                {
                    continue;
                }

                if (TraverseAndLookForBagHits(rule.Value, bagName))
                {
                    bagHits++;
                    hashSet.Add(rule.Key);
                }
            }

            return hashSet.Count;
        }

        private bool TraverseAndLookForBagHits(Bag bag, string bagName)
        {
            if (bag.Name == bagName)
            {
                return true;
            }

            foreach (var item in bag.Rules)
            {
                if (TraverseAndLookForBagHits(item.Contains, bagName))
                {
                    return true;
                }
            }

            return false;
        }

        private IDictionary<string, Bag> _bagRules;

        private Bag GetOrNew(string bagName)
        {
            if (_bagRules.ContainsKey(bagName))
            {
                return _bagRules[bagName];
            }

            var bag = new Bag(bagName);
            _bagRules.Add(bagName, bag);
            return bag;
        }
    }

    public class Bag
    {
        public Bag(string name)
        {
            Name = name;
            Rules = new List<BagRule>();
        }
        public string Name { get; }

        public List<BagRule> Rules { get; }
    }

    public class BagRule
    {
        public BagRule(Bag bag, int qty)
        {
            Quantity = qty;
            Contains = bag;
        }

        public int Quantity { get; }

        public Bag Contains { get; }
    }
}
