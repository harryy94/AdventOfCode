namespace AdventOfCode.Solutions.Common;

public record GridEntry
{
    public int X { get; init; }
    
    public int Y { get; init; }
    
    public char Value { get; init; }
}