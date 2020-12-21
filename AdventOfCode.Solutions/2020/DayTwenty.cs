using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwenty : BaseProblem
    {
        public DayTwenty() : base(2020, 20)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>()
            {
                @"Tile 2311:
..##.#..#.
##..#.....
#...##..#.
####.#...#
##.##.###.
##...#.###
.#.#.#..##
..#....#..
###...#.#.
..###..###

Tile 1951:
#.##...##.
#.####...#
.....#..##
#...######
.##.#....#
.###.#####
###.##.##.
.###....#.
..#.#..#.#
#...##.#..

Tile 1171:
####...##.
#..##.#..#
##.#..#.#.
.###.####.
..###.####
.##....##.
.#...####.
#.##.####.
####..#...
.....##...

Tile 1427:
###.##.#..
.#..#.##..
.#.##.#..#
#.#.#.##.#
....#...##
...##..##.
...#.#####
.#.####.#.
..#..###.#
..##.#..#.

Tile 1489:
##.#.#....
..##...#..
.##..##...
..#...#...
#####...#.
#..#.#.#.#
...#.#.#..
##.#...##.
..##.##.##
###.##.#..

Tile 2473:
#....####.
#..#.##...
#.##..#...
######.#.#
.#...#.#.#
.#########
.###.#..#.
########.#
##...##.#.
..###.#.#.

Tile 2971:
..#.#....#
#...###...
#.#.###...
##.##..#..
.#####..##
.#..####.#
#..#.#..#.
..####.###
..#.#.###.
...#.#.#.#

Tile 2729:
...#.#.#.#
####.#....
..#.#.....
....#..#.#
.##..##.#.
.#.####...
####.#.#..
##.####...
##..#.##..
#.##...##.

Tile 3079:
#.#.#####.
.#..######
..#.......
######....
####.#..#.
.#...#.##.
#.#####.##
..#.###...
..#.......
..#.###..."
            };

        protected override void DoSolve(string input)
        {
            var part1 = PartOne(input);
            var part2 = PartTwo(input);

            PartOneAnswer = part1.ToString();
            PartTwoAnswer = part2.ToString();
        }

        private long PartOne(string input)
        {
            var puzzle = AssemblePuzzle(input);
            var size = puzzle.GetLength(0);

            return (long)puzzle[0, 0].Id *
                puzzle[size - 1, size - 1].Id *
                puzzle[0, size - 1].Id *
                puzzle[size - 1, 0].Id;
        }

        private long PartTwo(string input)
        {
            var image = CombineTiles(-1, AssemblePuzzle(input));

            var monster = new[]{
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            while (true)
            {
                var monsterCount = MatchCount(image, monster);
                if (monsterCount > 0)
                {
                    var hashCountInMonster = string.Join("\n", monster).Count(ch => ch == '#');
                    return image.GetHashCount() - monsterCount * hashCountInMonster;
                }
                image.ChangeOrientation();
            }
        }

        private Tile[] Parse(string input)
        {
            var tiles = input
                .Replace("\r", "")
                .Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries)
                .ToList();

            var blocks = tiles.Select(x =>
            {
                var lines = x.SplitByLine();
                var id = lines[0].Trim(':').Split(' ')[1];
                var image = lines.Skip(1).Where(w => !string.IsNullOrEmpty(w)).ToArray();
                return new Tile(int.Parse(id), image);
            }).ToArray();

            return blocks;
        }

        private Tile[,] AssemblePuzzle(string input)
        {
            var tiles = Parse(input);

            var pairs = new Dictionary<string, List<Tile>>();
            foreach (var tile in tiles)
            {
                for (var i = 0; i < 8; i++)
                {
                    var pattern = tile.Top;
                    if (!pairs.ContainsKey(pattern))
                    {
                        pairs[pattern] = new List<Tile>();
                    }
                    pairs[pattern].Add(tile);
                    tile.ChangeOrientation();
                }
            }

            var size = (int)Math.Sqrt(tiles.Length);
            var tileSet = new Tile[size, size];
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    var tileAbove = i == 0 ? null : tileSet[i - 1, j];
                    var tileLeft = j == 0 ? null : tileSet[i, j - 1];
                    tileSet[i, j] = AssembleMatchingTiles(pairs, tileAbove, tileLeft, tiles);
                }
            }

            return tileSet;
        }

        private Tile AssembleMatchingTiles(Dictionary<string, List<Tile>> pairs, Tile tileAbove, Tile tileLeft, Tile[] tiles)
        {
            if (tileAbove == null && tileLeft == null)
            {
                foreach (var tile in tiles)
                {
                    for (var i = 0; i < 8; i++)
                    {
                        if (GetConnectingTile(tile, tile.Top, pairs) == null &&
                            GetConnectingTile(tile, tile.Left, pairs) == null)
                        {
                            return tile;
                        }

                        tile.ChangeOrientation();
                    }
                }
            }
            else
            {
                var tile = tileAbove != null
                    ? GetConnectingTile(tileAbove, tileAbove.Bottom, pairs)
                    : GetConnectingTile(tileLeft, tileLeft.Right, pairs);
                for (var i = 0; i < 8; i++)
                {
                    var topMatch = tileAbove == null
                        ? GetConnectingTile(tile, tile.Top, pairs) == null
                        : tile.Top == tileAbove.Bottom;
                    var leftMatch = tileLeft == null
                        ? GetConnectingTile(tile, tile.Left, pairs) == null
                        : tile.Left == tileLeft.Right;

                    if (topMatch && leftMatch)
                    {
                        return tile;
                    }

                    tile.ChangeOrientation();
                }
            }

            throw new Exception("Something wrong with the input");
        }

        private Tile GetConnectingTile(Tile tile, string pattern, Dictionary<string, List<Tile>> pairs)
        {
            return pairs[pattern].SingleOrDefault(tileB => tileB != tile);
        }

        private Tile CombineTiles(int id, Tile[,] tiles)
        {
            var image = new List<string>();
            var tileSize = tiles[0, 0].Size;
            for (var i = 0; i < tiles.GetLength(0); i++)
            {
                for (var j = 1; j < tileSize - 1; j++)
                {
                    var sb = new StringBuilder();
                    for (var col1 = 0; col1 < tiles.GetLength(1); col1++)
                    {
                        sb.Append(tiles[i, col1].GetSectionByRow(j).Substring(1, tileSize - 2));
                    }
                    image.Add(sb.ToString());
                }
            }
            return new Tile(id, image.ToArray());
        }

        private int MatchCount(Tile image, params string[] pattern)
        {
            var res = 0;
            for (var row1 = 0; row1 < image.Size; row1++)
            {
                for (var col1 = 0; col1 < image.Size; col1++)
                {
                    if (TileMatchesPattern(image, pattern, row1, col1))
                    {
                        res++;
                    }
                }
            }
            return res;
        }

        private bool TileMatchesPattern(Tile tile, string[] pattern, int row1, int col1)
        {
            var columnLength = pattern[0].Length;
            var rowLength = pattern.Length;

            if (row1 + rowLength >= tile.Size)
            {
                return false;
            }

            if (col1 + columnLength >= tile.Size)
            {
                return false;
            }

            for (var i = 0; i < columnLength; i++)
            {
                for (var j = 0; j < rowLength; j++)
                {
                    if (pattern[j][i] == '#' && tile.GetGridItem(row1 + j, col1 + i) != '#')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public class Tile
    {
        private readonly string[] _image;

        public Tile(int title, string[] image)
        {
            Id = title;
            Size = image.Length;

            _image = image;
        }

        private int _orientation;
        public void ChangeOrientation()
        {
            _orientation++;
            _orientation = _orientation % 8;
        }

        public int Id { get; }

        public int Size { get; }

        public char GetGridItem(int row1, int col1)
        {
            for (var i = 0; i < _orientation % 4; i++)
            {
                (row1, col1) = (col1, Size - 1 - row1);
            }

            if (_orientation % 8 >= 4)
            {
                col1 = Size - 1 - col1;
            }

            return _image[row1][col1];
        }

        public string GetSectionByRow(int row1)
        {
            return GetSection(row1, 0, 0, 1);
        }

        public string Top => GetSection(0, 0, 0, 1);

        public string Right => GetSection(0, Size - 1, 1, 0);

        public string Left => GetSection(0, 0, 1, 0);

        public string Bottom => GetSection(Size - 1, 0, 0, 1);

        public int GetHashCount()
        {
            var hashCount = 0;
            for (var i = 0; i < Size; i++)
            {
                hashCount += GetSectionByRow(i).Count(x => x == '#');
            }

            return hashCount;
        }

        private string GetSection(int row1, int col1, int row2, int col2)
        {
            var sb = new StringBuilder();
            for (var i = 0; i < Size; i++)
            {
                sb.Append(GetGridItem(row1, col1));
                row1 += row2;
                col1 += col2;
            }
            return sb.ToString();
        }
    }
}
