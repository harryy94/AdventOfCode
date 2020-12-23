using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwentyThree : BaseProblem
    {
        public DayTwentyThree() : base(2020, 23)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                "389125467"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var linkedListPart1 = new LinkedList<long>();
            var linkedListPart2 = new LinkedList<long>();

            foreach (var item in input.Replace("\n", ""))
            {
                linkedListPart1.AddLast(int.Parse(item.ToString()));
                linkedListPart2.AddLast(int.Parse(item.ToString()));
            }

            for (var i = 10; i <= 1000000; i++)
            {
                linkedListPart2.AddLast(i);
            }

            var part1 = DoPart1(linkedListPart1);
            var part2 = DoPart2(linkedListPart2);

            PartOneAnswer = part1;
            PartTwoAnswer = part2.ToString();
        }

        // Part 2 was too slow for just a LinkedList so incorporated a Dictionary for lookups.
        private long DoPart2(LinkedList<long> cups)
        {
            var cupsDict = new Dictionary<long, CupLinkNode>(cups.Count);
            for (var node = cups.First; node != null; node = node.Next)
            {
                cupsDict.Add(node.Value, new CupLinkNode(node, true));
            }

            var selectedCup = cups.First;
            var cupsPickedUp = new LinkedListNode<long>[3];

            for (var count = 0; count < 10000000; count++)
            {
                var nextPickUp = GetNextOrFirst(selectedCup);
                for (var i = 0; i < 3; i++)
                {
                    cupsPickedUp[i] = nextPickUp;
                    nextPickUp = GetNextOrFirst(nextPickUp);
                    cups.Remove(cupsPickedUp[i]);
                    cupsDict[cupsPickedUp[i].Value].InPlay = false;
                }

                var destination = selectedCup.Value == 1 ? cups.Count + cupsPickedUp.Length : selectedCup.Value - 1;

                while (!cupsDict[destination].InPlay)
                {
                    destination = destination == 1 ? cups.Count + cupsPickedUp.Length : destination - 1;
                }

                var destinationNode = cupsDict[destination].Node;
                foreach (var entry in cupsPickedUp)
                {
                    cups.AddAfter(destinationNode, entry);
                    cupsDict[entry.Value].InPlay = true;
                    destinationNode = entry;
                }
                selectedCup = selectedCup.Next ?? cups.First;
            }

            var nodeOne = cups.Find(1);
            var firstCup = GetNextOrFirst(nodeOne);
            var secondCup = GetNextOrFirst(firstCup);
            return firstCup.Value * secondCup.Value;
        }

        private string DoPart1(LinkedList<long> cups)
        {
            var selectedCup = cups.First;
            for (var count = 0; count < 100; count++)
            {
                var cupsPickedUp = new List<long>();

                var cupToRemove = GetNextOrFirst(selectedCup);
                for (var i = 0; i < 3; i++)
                {
                    cupsPickedUp.Add(cupToRemove.Value);
                    var tmp = GetNextOrFirst(cupToRemove);
                    cups.Remove(cupToRemove);
                    cupToRemove = tmp;
                }

                var destination = selectedCup.Value <= 1 ? 9 : selectedCup.Value - 1;
                while (cupsPickedUp.Contains(destination))
                {
                    destination--;
                    if (destination == 0)
                    {
                        destination = 9;
                    }
                }

                var destinationNode = cups.Find(destination);
                foreach (var entry in cupsPickedUp)
                {
                    destinationNode = cups.AddAfter(destinationNode, entry);
                }

                selectedCup = GetNextOrFirst(selectedCup);
            }

            var sb = new StringBuilder();
            // Print result
            var node = GetNextOrFirst(cups.Find(1));
            while (node.Value != 1)
            {
                sb.Append(node.Value);
                node = GetNextOrFirst(node);
            }

            return sb.ToString();
        }

        private LinkedListNode<long> GetNextOrFirst(LinkedListNode<long> list)
        {
            return list.Next ?? list.List.First;
        }
    }

    public class CupLinkNode
    {
        public CupLinkNode(LinkedListNode<long> node, bool inPlay)
        {
            InPlay = inPlay;
            Node = node;
        }
        public bool InPlay { get; set; }

        public LinkedListNode<long> Node { get; set; }
    }
}
