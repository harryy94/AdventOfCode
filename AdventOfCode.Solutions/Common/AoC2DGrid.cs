using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Common;

public class AoC2DGrid
{
    private char[,] _grid;

    public int Width { get; }
    public int Height { get; }
    
    public AoC2DGrid(int width, int height, char fillChar = '.')
    {
        Width = width;
        Height = height;
        _grid = new char[width, height];
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                _grid[x, y] = fillChar;
            }
        }
    }
    
    public static AoC2DGrid CreateWithLineInput(List<string> fillChar)
    {
        fillChar = fillChar.Where(x => x.Length > 0).ToList();
        
        var height = fillChar.Count;
        var width = fillChar[0].Length;
        
        var grid = new AoC2DGrid(width, height);
        
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                grid[x, y] = fillChar[y][x];
            }
        }

        return grid;
    }
    
    public char this[int x, int y]
    {
        get => _grid[x, y];
        set => _grid[x, y] = value;
    }
    
    public GridEntry Swap(int x1, int y1, int x2, int y2)
    {
        var temp = this[x1, y1];
        this[x1, y1] = this[x2, y2];
        this[x2, y2] = temp;
        
        return new GridEntry
        {
            X = x2,
            Y = y2,
            Value = this[x2, y2]
        };
    }
    
    public List<GridEntry> FindChars(char c)
    {
        var entries = new List<GridEntry>();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (this[x, y] == c)
                {
                    entries.Add(new GridEntry
                    {
                        X = x,
                        Y = y,
                        Value = c
                    });
                }
            }
        }

        return entries;
    }
    
    public GridEntry FindCharFirst(char c)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (this[x, y] == c)
                {
                    return new GridEntry
                    {
                        X = x,
                        Y = y,
                        Value = c
                    };
                }
            }
        }

        throw new NullReferenceException("Char not found");
    }
    
    public GridEntry? FindNeighbour(int x, int y, int xOffset, int yOffset)
    {
        var newX = x + xOffset;
        var newY = y + yOffset;

        if (newX < 0 || newX >= Width || newY < 0 || newY >= Height)
        {
            return null;
        }

        return new GridEntry
        {
            X = newX,
            Y = newY,
            Value = this[newX, newY]
        };
    }

    public GridEntry? FindNeighbourAbove(int x, int y, int offset = 1) => FindNeighbour(x, y, 0, -offset);
    public GridEntry? FindNeighbourAboveRight(int x, int y, int offset = 1)=> FindNeighbour(x, y, offset, -offset);
    public GridEntry? FindNeighbourRight(int x, int y, int offset = 1)=> FindNeighbour(x, y, offset, 0);
    public GridEntry? FindNeighbourBelowRight(int x, int y, int offset = 1)=> FindNeighbour(x, y, offset, offset);
    public GridEntry? FindNeighbourBelow(int x, int y, int offset = 1)=> FindNeighbour(x, y, 0, offset);
    public GridEntry? FindNeighbourBelowLeft(int x, int y, int offset = 1)=> FindNeighbour(x, y, -offset, offset);
    public GridEntry? FindNeighbourLeft(int x, int y, int offset = 1)=> FindNeighbour(x, y, -offset, 0);
    public GridEntry? FindNeighbourAboveLeft(int x, int y, int offset = 1)=> FindNeighbour(x, y, -offset, -offset);
    

    public void PrintToConsole()
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Console.Write(_grid[x,y]);
            }
            Console.WriteLine("");
        }
    }
}