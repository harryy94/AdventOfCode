using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions._2020
{
    public class DayFive : BaseProblem
    {
        public DayFive() : base(2020, 5)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"FBFBBFFRLR
BFFFBBFRRR
FFFBBBFRRR
BBFFBBFRLL" 
                //: row 14, column 7, seat ID 119.
                //: row 70, column 7, seat ID 567.
                //: row 102, column 4, seat ID 820.
            };

        protected override void DoSolve(string input)
        {
            var resultList = new List<Seat>();

            foreach (var rowCode in input.Split('\n'))
            {
                var seatCode = DecodeSeatBinary(rowCode);
                //Console.WriteLine(seatCode);
                if (seatCode != null)
                {
                    resultList.Add(seatCode);
                }
            }

            PartOneAnswer = resultList.Max(x => x.SeatId).ToString();

            Seat result = null;

            for (var i = 8; i < 124; i++)
            {
                for (var j = 1; j < 7; j++)
                {
                    if (!resultList.Any(a => a.Row == i && a.Column == j))
                    {
                        //Console.WriteLine($"Seat {i} - {j} missing.");
                        if (result != null)
                        {
                            Console.WriteLine("Duplicate seat {i} - {j} missing.");
                        }
                        result = new Seat {Row = i, Column = j};
                    }
                }
            }

            PartTwoAnswer = result.SeatId.ToString();
        }

        private Seat DecodeSeatBinary(string seatCode)
        {
            if (string.IsNullOrEmpty(seatCode))
                return null;

            var sb = new StringBuilder();

            foreach (var x in seatCode)
            {
                sb.Append(x == 'B' || x == 'R' ? 1 : 0);
            }

            var binaryString = sb.ToString();

            return new Seat
            {
                Row = Convert.ToInt32(binaryString.Substring(0, 7), 2),
                Column = Convert.ToInt32(binaryString.Substring(7, 3), 2)
            };
        }

        /// <summary>
        /// Old way before I realised you could just use Binary (eye-roll).
        /// </summary>
        /// <param name="seatCode"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        private Seat DecodeSeat(string seatCode, int count)
        {
            if (string.IsNullOrEmpty(seatCode))
                return null;

            decimal lowerBound = 0;
            decimal upperBound = count;

            var rowCodeString = seatCode.Substring(0, 7);
            var seatCodeString = seatCode.Substring(7, 3);

            foreach (var code in rowCodeString)
            {
               // Console.WriteLine($"Bound {lowerBound}-{upperBound}");
                var halfway = Math.Ceiling((lowerBound + upperBound) / 2);
                if (code == 'B')
                {
                    lowerBound = halfway;
                }
                else if (code == 'F')
                {
                    upperBound = halfway;
                }
            }

            var seatRow = lowerBound;

            lowerBound = 0;
            upperBound = 7;

            foreach (var code in seatCodeString)
            {
                //Console.WriteLine($"Bound {lowerBound}-{upperBound}");
                var halfway = Math.Ceiling((lowerBound + upperBound) / 2);
                if (code == 'R')
                {
                    lowerBound = halfway;
                }
                else if (code == 'L')
                {
                    upperBound = halfway;
                }
            }

            var seatNum = lowerBound;

            /*
             * Every seat also has a unique seat ID: multiply the row by 8, then add the column.
             * In this example, the seat has ID 44 * 8 + 5 = 357.
             */

            return new Seat
            {
                Row = (int) seatRow,
                Column = (int) seatNum
            };
        }

        class Seat
        {
            public int Row { get; set; }

            public int Column { get; set; }

            public int SeatId => Row * 8 + Column;
        }
    }
}
