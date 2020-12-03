using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2020
{
    public class DayThree : BaseProblem
    {
        public DayThree() : base(2020, 3)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>()
            {
                @"..##.......
#...#...#..
.#....#..#.
..#.#...#.#
.#...##..#.
..#.##.....
.#.#.#....#
.#........#
#.##...#...
#...##....#
.#..#...#.#"
            };

        protected override void DoSolve(string input)
        {
            var puzzleInput = new List<List<bool>>();

            foreach (var line in input.Split('\n'))
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var lineList = (new List<bool>());

                puzzleInput.Add(lineList);

                foreach (var item in line)
                {
                    if (item == '#')
                        lineList.Add(true);

                    if (item == '.')
                        lineList.Add(false);
                }
            }

            /*
               Right 3, down 1. (This is the slope you already checked.)
               Right 1, down 1.
               Right 5, down 1.
               Right 7, down 1.
               Right 1, down 2.
             */
            var results = new int[5];

            results[0] = Traverse(3, 1, puzzleInput);
            results[1] = Traverse(1, 1, puzzleInput);
            results[2] = Traverse(5, 1, puzzleInput);
            results[3] = Traverse(7, 1, puzzleInput);
            results[4] = Traverse(1, 2, puzzleInput);

            PartOneAnswer = results[0].ToString();

            var multipliedResult = results.Aggregate(1, (current, result) => current * result);

            //PartOneAnswer = "N/A";
            PartTwoAnswer = multipliedResult.ToString();
        }

        private int Traverse(int right, int down, List<List<bool>> input)
        {
            var x = 0;
            var y = 0;

            var treeCount = 0;

            while (true)
            {
                x += right;
                y += down;

                if (y >= input.Count)
                {
                    // Finished
                    break;
                }

                if (x >= input[y].Count)
                {
                    input = DuplicateEntries(input);
                }

                if (input[y][x])
                {
                    treeCount++;
                }
            }

            return treeCount;
        }

        private List<List<bool>> DuplicateEntries(List<List<bool>> lines)
        {
            var newList = new List<List<bool>>();
            foreach (var line in lines)
            {
                var newListLine = new List<bool>();
                newListLine.AddRange(line);
                newListLine.AddRange(line);
                newList.Add(newListLine);
            }

            return newList;
        }
    }

    public class Grid
    {

        public bool Tree { get; }
    }
}
