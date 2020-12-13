using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayThirteen : BaseProblem
    {
        public DayThirteen() : base(2020, 13)
        {
        }

        public override List<string> ExampleInput { get; }
        = new List<string>
        {
            @"939
7,13,x,x,59,x,31,19",
            @"0
67,x,7,59,61",
            @"0
67,7,x,59,61",
            @"0
1789,37,47,1889"
        };

        protected override void DoSolve(string input)
        {
            var inputSplitter = input.SplitByLine();

            var timeReady = int.Parse(inputSplitter[0]);

            var busIncrements = new List<BusSchedule>();

            var id = 0;
            foreach (var bus in inputSplitter[1].Split(','))
            {
                if (bus != "x")
                {
                    busIncrements.Add(new BusSchedule(id, int.Parse(bus)));
                }

                id++;
            }

            var part1Input = busIncrements.Select(x => x.Schedule).ToList();

            //var busIncrements = inputSplitter[1]
            //    .Split(',')
            //    .Where(x => x != "x")
            //    .Select(int.Parse)
            //    .ToList();

            PartOneAnswer = FindBestBus(timeReady, part1Input).ToString();
            PartTwoAnswer = Calculate(busIncrements).ToString();
        }

        public long Calculate(List<BusSchedule> busSchedules)
        {
            var timestamp = busSchedules.First().Schedule - busSchedules.First().Id;
            var period = busSchedules.First().Schedule;
            for (var busIndex = 1; busIndex <= busSchedules.Count; busIndex++)
            {
                while (busSchedules
                    .Take(busIndex)
                    .Any(a => (timestamp + a.Id) % a.Schedule != 0)
                )
                {
                    timestamp += period;
                }

                period = busSchedules
                    .Take(busIndex)
                    .Select(t => t.Schedule)
                    .Aggregate(LCM);
            }

            return timestamp;
        }

        private long FindBestBus(int timeReady, List<long> busIncrements)
        {
            var bestBus = 0L;
            var bestBusTime = 9999999L;

            foreach (var bus in busIncrements)
            {
                var busIncrement = bus;
                while (busIncrement < timeReady)
                {
                    busIncrement += bus;
                }

                if (bestBusTime > busIncrement)
                {
                    bestBusTime = busIncrement;
                    bestBus = bus;
                }
            }

            return (bestBusTime - timeReady) * bestBus;
        }

        public long GCD(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public long LCM(long a, long b)
        {
            return (a / GCD(a, b)) * b;
        }
    }

    public class BusSchedule
    {
        public BusSchedule(long id, long schedule)
        {
            Id = id;
            Schedule = schedule;
        }

        public long Id { get; }

        public long Schedule { get; }
    }

}
