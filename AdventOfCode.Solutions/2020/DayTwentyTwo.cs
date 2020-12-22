using System.Collections.Generic;
using System.Linq;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwentyTwo : BaseProblem
    {
        public DayTwentyTwo() : base(2020, 22)
        {
        }

        public override List<string> ExampleInput { get; }
        = new List<string>
        {
            @"Player 1:
9
2
6
3
1

Player 2:
5
8
4
7
10",
        };
        protected override void DoSolve(string input)
        {
            var decksPart1 = new Decks();
            var decksPart2 = new Decks();

            var inputs = input.SplitByLine();
            inputs.Reverse(0, inputs.Count);

            var nextPlayer = true;
            foreach (var line in inputs)
            {
                if (line.StartsWith("Player 2"))
                {
                    nextPlayer = false;
                    continue;
                }

                if (int.TryParse(line, out var num))
                {
                    if (nextPlayer)
                    {
                        decksPart1.Player2.Add(num);
                        decksPart2.Player2.Add(num);
                    }
                    else
                    {
                        decksPart1.Player1.Add(num);
                        decksPart2.Player1.Add(num);
                    }
                }
            }
            var part1Count = PlayPart1(decksPart1);
            var part2Count = PlayPart2(decksPart2);

            PartOneAnswer = part1Count.ToString();
            PartTwoAnswer = part2Count.ToString();
        }

        private long PlayPart2(Decks decks)
        {
            var storedSubDecks = new List<Decks>();
            while (decks.Player1.Count > 0 && decks.Player2.Count > 0)
            {
                PlayPart2Round(decks, storedSubDecks, 1);
            }
            var score = 0L;
            var winningDeck = decks.Player1.Count > 0 ? decks.Player1 : decks.Player2;

            var index = 0;
            foreach (var card in winningDeck)
            {
                index++;
                score += index * card;
            }

            return score;
        }

        private bool PlayPart2Round(Decks decks, List<Decks> storedDecks, int game)
        {
            var player1Card = decks.Player1.Last();
            var player2Card = decks.Player2.Last();

            foreach (var storedDeck in storedDecks)
            {
                if (storedDeck.DecksMatch(decks))
                {
                    return true;
                }
            }

            storedDecks.Add(decks.DeepCopy(decks.Player1.Count, decks.Player2.Count));

            //Console.WriteLine($"-- Game {game} --");
            //Console.WriteLine($"Player 1: {player1Card} ({string.Join(", ", decks.Player1)})");
            //Console.WriteLine($"Player 2: {player2Card} ({string.Join(", ", decks.Player2)})");
            //Console.WriteLine("---- END -----");

            decks.Player1.Remove(player1Card);
            decks.Player2.Remove(player2Card);


            if (player1Card <= decks.Player1.Count && player2Card <= decks.Player2.Count)
            {
                // if value of card is less or equal to remaining card count
                // do recursive game. Copy cards and then recurse.
                var subDecks = decks.DeepCopy(player1Card, player2Card);
                var storedSubDecks = new List<Decks>();
                var terminated = false;
                while (subDecks.Player1.Count > 0 && subDecks.Player2.Count > 0)
                { 
                    terminated = PlayPart2Round(subDecks, storedSubDecks, game + 1);
                    if (terminated)
                    {
                        break;
                    }
                }

                if (terminated || subDecks.Player1.Count > 0)
                {
                    decks.Player1 = decks.Player1.Prepend(player1Card).Prepend(player2Card).ToList();
                }
                else
                {
                    decks.Player2 = decks.Player2.Prepend(player2Card).Prepend(player1Card).ToList();
                }
            }
            else
            {
                // else, use rules as of part 1.
                if (player1Card > player2Card)
                {
                    decks.Player1 = decks.Player1.Prepend(player1Card).Prepend(player2Card).ToList();
                }
                else
                {
                    decks.Player2 = decks.Player2.Prepend(player2Card).Prepend(player1Card).ToList();
                }
            }

            return false;
        }

        private long PlayPart1(Decks decks)
        {
            while (decks.Player1.Count > 0 && decks.Player2.Count > 0)
            {
                PlayPart1Round(decks);
            }

            var score = 0L;
            var winningDeck = decks.Player1.Count > 0 ? decks.Player1 : decks.Player2;

            var index = 0;
            foreach (var card in winningDeck)
            {
                index++;
                score += index * card;
            }

            return score;
        }

        private void PlayPart1Round(Decks decks)
        {
            var player1Card = decks.Player1.Last();
            var player2Card = decks.Player2.Last();

            decks.Player1.Remove(player1Card);
            decks.Player2.Remove(player2Card);

            if (player1Card > player2Card)
            {
                decks.Player1 = decks.Player1.Prepend(player1Card).Prepend(player2Card).ToList();
            }
            else
            {
                decks.Player2 = decks.Player2.Prepend(player2Card).Prepend(player1Card).ToList();
            }
        }
    }

    public class Decks
    {
        public List<int> Player1 { get; set; } = new List<int>();

        public List<int> Player2 { get; set; } = new List<int>();

        public bool DecksMatch(Decks decks)
        {
            var matched = true;

            if (decks.Player1.Count != Player1.Count || decks.Player2.Count != Player2.Count)
                return false;

            for (var i = 0; i < Player1.Count; i++)
            {
                if (decks.Player1[i] != Player1[i])
                {
                    matched = false;
                    break;
                }
            }
            for (var i = 0; i < Player2.Count; i++)
            {
                if (decks.Player2[i] != Player2[i])
                {
                    matched = false;
                    break;
                }
            }

            return matched;
        }

        public Decks DeepCopy(int player1Count, int player2Count)
        {
            var player1 = new List<int>();

            for (var i = Player1.Count - player1Count; i < Player1.Count; i++)
            {
                player1.Add(Player1[i]);
            }

            var player2 = new List<int>();
            for (var i = Player2.Count - player2Count; i < Player2.Count; i++)
            {
                player2.Add(Player2[i]);
            }

            return new Decks
            {
                Player1 = player1,
                Player2 = player2
            };
        }
    }
}
