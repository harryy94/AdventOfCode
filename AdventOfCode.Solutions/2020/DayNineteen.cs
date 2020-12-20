using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayNineteen : BaseProblem
    {
        public DayNineteen() : base(2020, 19)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"0: 4 1 5
1: 2 3 | 3 2
2: 4 4 | 5 5
3: 4 5 | 5 4
4: ""a""
5: ""b""

ababbb
bababa
abbbab
aaabbb
aaaabbb",
                @"42: 9 14 | 10 1
9: 14 27 | 1 26
10: 23 14 | 28 1
1: ""a""
11: 42 31
5: 1 14 | 15 1
19: 14 1 | 14 14
12: 24 14 | 19 1
16: 15 1 | 14 14
31: 14 17 | 1 13
6: 14 14 | 1 14
2: 1 24 | 14 4
0: 8 11
13: 14 3 | 1 12
15: 1 | 14
17: 14 2 | 1 7
23: 25 1 | 22 14
28: 16 1
4: 1 1
20: 14 14 | 1 15
3: 5 14 | 16 1
27: 1 6 | 14 18
14: ""b""
21: 14 1 | 1 14
25: 1 1 | 1 14
22: 14 14
8: 42
26: 14 22 | 1 20
18: 15 15
7: 14 5 | 1 21
24: 14 1

abbbbbabbbaaaababbaabbbbabababbbabbbbbbabaaaa
bbabbbbaabaabba
babbbbaabbbbbabbbbbbaabaaabaaa
aaabbbbbbaaaabaababaabababbabaaabbababababaaa
bbbbbbbaaaabbbbaaabbabaaa
bbbababbbbaaaaaaaabbababaaababaabab
ababaaaaaabaaab
ababaaaaabbbaba
baabbaaaabbaaaababbaababb
abbbbabbbbaaaababbbbbbaaaababb
aaaaabbaabaaaaababaa
aaaabbaaaabbaaa
aaaabbaabbaaaaaaabbbabbbaaabbaabaaa
babaaabbbaaabaababbaabababaaab
aabbbbbaabbbaaaaaabbbbbababaaaaabbaaabba"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var part1 = Run(ParseInput(input), false);
            var part2 = Run(ParseInput(input), true);

            PartOneAnswer = part1.ToString();
            PartTwoAnswer = part2.ToString();
        }

        private int Run(Input input, bool isPart2)
        {
            var rules = input.Rules.ToList();
            if (isPart2)
            {
                rules.Add(new SequenceRule
                {
                    Id = 8, SubRules = new List<int>
                    {
                        42, 8
                    }
                });
                rules.Add(new SequenceRule
                {
                    Id = 11, SubRules = new List<int> {42, 11, 31}
                });
            }
            var matcher = new Matcher(rules);
            int count = 0;
            foreach (var msg in input.Messages)
            {
                if (matcher.IsMatch(msg))
                {
                    count++;
                }
            }
            return count;
        }

        class Matcher
        {
            private ILookup<int, Rule> _rules;

            public Matcher(IEnumerable<Rule> rules)
            {
                _rules = rules.ToLookup(r => r.Id);
            }

            public bool IsMatch(string input)
            {
                foreach (var end in Match(input, 0, 0))
                {
                    if (end == input.Length) return true;
                }
                return false;
            }
            private IEnumerable<int> Match(string input, int num, int pos)
            {
                foreach (var rule in _rules[num])
                {
                    if (rule is LiteralRule lit)
                    {
                        foreach (var end in MatchLit(input, lit, pos))
                        {
                            yield return end;
                        }
                    }
                    else if (rule is SequenceRule seq)
                    {
                        foreach (var end in MatchSeq(input, seq, pos, 0))
                        {
                            yield return end;
                        }
                    }
                    else
                    {
                        throw new ArgumentException(nameof(rule));
                    }
                }
            }

            private IEnumerable<int> MatchLit(string input, LiteralRule literal, int pos)
            {
                if (string.CompareOrdinal(input, pos, literal.LiteralValue, 0, literal.LiteralValue.Length) == 0)
                {
                    yield return pos + literal.LiteralValue.Length;
                }
            }

            private IEnumerable<int> MatchSeq(string input, SequenceRule sequence, int pos, int index)
            {
                if (index == sequence.SubRules.Count)
                {
                    yield return pos;
                    yield break;
                }
                foreach (var end in Match(input, sequence.SubRules[index], pos))
                {
                    foreach (var end2 in MatchSeq(input, sequence, end, index + 1))
                    {
                        yield return end2;
                    }
                }
            }
        }

        private Regex _regex = new Regex(@"^(\d+):(?: ""([^""]*)""|(?: ((?:\| )?)(\d+))+)\s*$");
        private Input ParseInput(string input)
        {
            var result = new Input();

            var lines = input.Trim().SplitByLine();

            foreach (var line in lines)
            {
                if (_regex.IsMatch(line))
                {
                    var match = _regex.Match(line);
                    if (match.Groups[2].Success)
                    {
                        result.Rules.Add(new LiteralRule
                        {
                            Id = int.Parse(match.Groups[1].Value),
                            LiteralValue = match.Groups[2].Value
                        });
                    }
                    else
                    {
                        var seq = new List<int>();
                        var alt = new List<List<int>> { seq };
                        for (var i = 0; i < match.Groups[4].Captures.Count; i++)
                        {
                            if (match.Groups[3].Captures[i].Length != 0)
                            {
                                alt.Add(seq = new List<int>());
                            }
                            seq.Add(int.Parse(match.Groups[4].Captures[i].Value));
                        }
                        foreach (var seql in alt)
                        {
                            result.Rules.Add(new SequenceRule
                            {
                                Id = int.Parse(match.Groups[1].Value),
                                SubRules = seql
                            });
                        }
                    }
                }
                else
                {
                    result.Messages.Add(line);
                }
            }
            return result;
        }
    }
    public abstract class Rule
    {
        public int Id { get; set; }
    }

    public class LiteralRule : Rule
    {
        public string LiteralValue { get; set; }
    }

    public class SequenceRule : Rule
    {
        public List<int> SubRules { get; set; }
    }

    public class Input
    {
        public List<Rule> Rules { get; } = new List<Rule>();
        public List<string> Messages { get; } = new List<string>();
    }
}
