using System;
using System.Collections.Generic;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwentyFive : BaseProblem
    {
        public DayTwentyFive() : base(2020, 25)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"5764801
17807724"
            };

        public override bool RunActual { get; set; } = true;

        protected override void DoSolve(string input)
        {
            var lines = input.SplitByLine();

            var key  = long.Parse(lines[0]);
            var door = long.Parse(lines[1]);

            var keyLoopSize = GetSecretLoopSize(key);
            var doorLoopSize = GetSecretLoopSize(door);

            var keyEncryptionKey = TransformPublicKey(key, doorLoopSize);
            var doorEncryptionKey = TransformPublicKey(door, keyLoopSize);

            if (keyEncryptionKey == doorEncryptionKey)
            {
                PartOneAnswer = keyEncryptionKey.ToString();
            }
            else
            {
                PartOneAnswer = $"Mismatch. Key: {keyEncryptionKey}, Door: {doorEncryptionKey}";
            }

            PartTwoAnswer = "Merry Christmas!";
        }

        private long TransformPublicKey(long subject, long loopSize)
        {
            var loopSizeValue = 1L;
            for (var i = 0; i < loopSize; i++)
            {
                loopSizeValue *= subject;
                loopSizeValue %= 20201227;
            }

            return loopSizeValue;
        }

        private long GetSecretLoopSize(long key)
        {
            var count = 1L;
            var loopSizeValue = 1L;

            while (loopSizeValue != key)
            {
                loopSizeValue *= 7;
                loopSizeValue %= 20201227;

                if (loopSizeValue == key)
                {
                    break;
                }

                count++;
            }

            return count;
        }
    }
}
