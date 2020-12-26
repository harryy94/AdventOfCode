using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwentyFour : BaseProblem
    {
        public DayTwentyFour() : base(2020, 24)
        {
        }

        public override List<string> ExampleInput { get; } =
            new List<string>
            {
                @"esew
nwwswee",
                @"sesenwnenenewseeswwswswwnenewsewsw
neeenesenwnwwswnenewnwwsewnenwseswesw
seswneswswsenwwnwse
nwnwneseeswswnenewneswwnewseswneseene
swweswneswnenwsewnwneneseenw
eesenwseswswnenwswnwnwsewwnwsene
sewnenenenesenwsewnenwwwse
wenwwweseeeweswwwnwwe
wsweesenenewnwwnwsenewsenwwsesesenwne
neeswseenwwswnwswswnw
nenwswwsewswnenenewsenwsenwnesesenew
enewnwewneswsewnwswenweswnenwsenwsw
sweneswneswneneenwnewenewwneswswnese
swwesenesewenwneswnwwneseswwne
enesenwswwswneneswsenwnewswseenwsese
wnwnesenesenenwwnenwsewesewsesesew
nenewswnwewswnenesenwnesewesw
eneswnwswnwsenenwnwnwwseeswneewsenese
neswnwewnwnwseenwseesewsenwsweewe
wseweeenwnesenwwwswnew"
            };

        private IDictionary<HexagonCoordinate, bool> _hexagonCoords;

        protected override void DoSolve(string input)
        {
            _hexagonCoords = new Dictionary<HexagonCoordinate, bool>();

            DoPart1(input);
            
            PartOneAnswer = _hexagonCoords.Count(x => x.Value).ToString();

            DoPart2();

            PartTwoAnswer = _hexagonCoords.Count(x => x.Value).ToString();
        }

        private List<HexagonCoordinate> _directionsToCheck = new List<HexagonCoordinate>
        {
            new HexagonCoordinate(2, 0), // East
            new HexagonCoordinate(-2, 0), // West
            new HexagonCoordinate(-1, -1), // North West
            new HexagonCoordinate(1, -1), // North East
            new HexagonCoordinate(-1, 1), // South West
            new HexagonCoordinate(1, 1), // South East
        };

        private void DoPart2()
        {
           // Console.WriteLine("Day 1: " + _hexagonCoords.Count(x => x.Value));

           for (var i = 0; i < 100; i++)
            {
                //Console.WriteLine($"Day {i}: " + _hexagonCoords.Count(x => x.Value));
                var dictionaryCache = _hexagonCoords
                    .ToDictionary(k
                            => k.Key,
                        v => v.Value);

                var coordinatesChecked = new HashSet<HexagonCoordinate>();

                foreach (var entry in dictionaryCache)
                {
                    // true is black, false is white
                    if (!coordinatesChecked.Add(entry.Key))
                        continue;

                    if (entry.Value)
                    {
                        // Black
                        var blackTilesFound = GetNeighbouringBlackTiles(dictionaryCache, entry.Key);

                        if (blackTilesFound == 0 || blackTilesFound > 2)
                        {
                            _hexagonCoords[entry.Key] = false;
                        }
                    }
                    else
                    {
                        // White
                        var blackTilesFound = GetNeighbouringBlackTiles(dictionaryCache, entry.Key);

                        if (blackTilesFound == 2)
                        {
                            _hexagonCoords[entry.Key] = true;
                        }
                    }

                    //Check neighbours for ones that may not have been registered in the dictionary
                    for (var j = 0; j < 6; j++)
                    {
                        var coordCheck = entry.Key.GetCoordDiff(_directionsToCheck[j]);
                        if (!dictionaryCache.ContainsKey(coordCheck))
                        {
                            if (coordinatesChecked.Add(coordCheck))
                            {
                                var blackTilesFound = GetNeighbouringBlackTiles(dictionaryCache, coordCheck);

                                if (blackTilesFound == 2)
                                {
                                    _hexagonCoords.Add(coordCheck, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        private int GetNeighbouringBlackTiles(Dictionary<HexagonCoordinate, bool> hexCoords,
            HexagonCoordinate coordinates)
        {
            var blackTilesFound = 0;
            for (var j = 0; j < 6; j++)
            {
                var coordCheck = coordinates.GetCoordDiff(_directionsToCheck[j]);
                if (hexCoords.ContainsKey(coordCheck))
                {
                    blackTilesFound += hexCoords[coordCheck] ? 1 : 0;
                }
            }

            return blackTilesFound;
        }

        private void DoPart1(string input)
        {
            var hexagonInstructions = ParseInput(input);

            foreach (var hexagonInstruction in hexagonInstructions)
            {
                var coords = new HexagonCoordinate(0, 0);

                foreach (var instruction in hexagonInstruction.Instructions)
                {
                    switch (instruction)
                    {
                        case HexagonInstructionDirection.East:
                            coords.X += 2;
                            break;
                        case HexagonInstructionDirection.SouthEast:
                            coords.X++;
                            coords.Y--;
                            break;
                        case HexagonInstructionDirection.SouthWest:
                            coords.X--;
                            coords.Y--;
                            break;
                        case HexagonInstructionDirection.West:
                            coords.X -= 2;
                            break;
                        case HexagonInstructionDirection.NorthWest:
                            coords.X--;
                            coords.Y++;
                            break;
                        case HexagonInstructionDirection.NorthEast:
                            coords.X++;
                            coords.Y++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (_hexagonCoords.ContainsKey(coords))
                {
                    _hexagonCoords[coords] = !_hexagonCoords[coords];
                }
                else
                {
                    _hexagonCoords.Add(coords, true);
                }
            }
        }

        private List<HexagonInstruction> ParseInput(string input)
        {
            var instructionList = new List<HexagonInstruction>();
            foreach (var line in input.SplitByLine())
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var instruction = new HexagonInstruction();
                for (var i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'w')
                    {
                        instruction.Instructions.Add(HexagonInstructionDirection.West);
                    }
                    else if (line[i] == 'e')
                    {
                        instruction.Instructions.Add(HexagonInstructionDirection.East);
                    }
                    else if (line[i] == 'n')
                    {
                        if (line[i + 1] == 'w')
                        {
                            instruction.Instructions.Add(HexagonInstructionDirection.NorthWest);
                        }
                        else if (line[i + 1] == 'e')
                        {
                            instruction.Instructions.Add(HexagonInstructionDirection.NorthEast);
                        }

                        i++;
                    }
                    else if (line[i] == 's')
                    {
                        if (line[i + 1] == 'w')
                        {
                            instruction.Instructions.Add(HexagonInstructionDirection.SouthWest);
                        }
                        else if (line[i + 1] == 'e')
                        {
                            instruction.Instructions.Add(HexagonInstructionDirection.SouthEast);
                        }

                        i++;
                    }
                }
                instructionList.Add(instruction);
            }

            return instructionList;
        }

    } //e, se, sw, w, nw, and ne

    public class HexagonInstruction
    {
        public List<HexagonInstructionDirection> Instructions { get; }
            = new List<HexagonInstructionDirection>();
    }

    public struct HexagonCoordinate
    {
        public HexagonCoordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }

        public int Y { get; set; }

        public HexagonCoordinate GetCoordDiff(HexagonCoordinate coords)
        {
            return new HexagonCoordinate(X + coords.X, Y + coords.Y);
        }
    }

    public enum HexagonInstructionDirection
    {
        East, SouthEast, SouthWest, West, NorthWest, NorthEast
    }
}
