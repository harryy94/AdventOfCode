using System.Collections.Generic;
using System.Text.RegularExpressions;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2024;

public class DayThree() : BaseProblem(2024, 3)
{
    public override bool RunExamples { get; set; } = true;

    public override List<string> ExampleInput { get; }
        = new()
        {
            "xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))",
            "xmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))"
        };
    
    protected override void DoSolve(string input)
    {
        var inputSplit = input.SplitByLine();
        input = "";
        foreach (var line in inputSplit)
        {
            input += line;
        }
        
        const string mulExtractorRegexString = @"mul\(([0-9]*)\,([0-9]*)\)";
        const string dontExtractorRegexString = @"don't\(\).*?do\(\)";

        var mulExtractorRegex = new Regex(mulExtractorRegexString);
        var dontExtractorRegex = new Regex(dontExtractorRegexString);
        
        var matches = mulExtractorRegex.Matches(input);

        var result = 0;
        
        foreach (Match match in matches)
        {
            result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }

        PartOneAnswer = result.ToString();
        
        var newInput = dontExtractorRegex.Replace(input, "");

        result = 0;

        var newMatches = mulExtractorRegex.Matches(newInput);
        
        for (var i = 0; i < newMatches.Count; i++)
        {
            result += int.Parse(newMatches[i].Groups[1].Value) * int.Parse(newMatches[i].Groups[2].Value);
        }
        
        PartTwoAnswer = result.ToString();
    }
}