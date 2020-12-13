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
7,13,x,x,59,x,31,19"
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
            PartTwoAnswer = FindSubsequentBusses(busIncrements).ToString();
        }

        private long FindSubsequentBusses(List<BusSchedule> busSchedules)
        {
            //for (var i = 0; i < busSchedules.Count; i++)
            //{
            //    busSchedules[i].IncrementUntilGreaterThan(100000000000000);
            //}

            var successfulRoutine = false;
            var loopBreaker = 0L;
            while (!successfulRoutine)
            {
                loopBreaker++;
                if (loopBreaker > 100000000000)
                {
                    Console.WriteLine("Infinite loop detected");
                    break;
                }

                busSchedules
                    .OrderBy(x => x.Increment)
                    .First()
                    .IncreaseIncrement();
                //for (var i = 0; i < busSchedules.Count; i++)
                //{
                //    busSchedules[i].IncreaseIncrement();
                //}

                successfulRoutine = true;

                var baseIncrement = busSchedules[0].Increment;

                for (var i = 1; i < busSchedules.Count; i++)
                {
                    if (busSchedules[i].Increment - busSchedules[i].Id != baseIncrement)
                    {
                        successfulRoutine = false;
                        break;
                    }
                }

                if (successfulRoutine)
                {
                    return baseIncrement;
                }
            }
            return busSchedules[0].Increment;
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
    }

    public class BusSchedule
    {
        public BusSchedule(long id, long schedule)
        {
            Id = id;
            Schedule = schedule;
            Increment = schedule;
        }

        public long Id { get; }

        public long Schedule { get; }

        public long Increment { get; private set; }

        public void IncreaseIncrement()
        {
            Increment += Schedule;
        }

        public void IncrementUntilGreaterThan(long value)
        {
            while (Increment < value)
            {
                IncreaseIncrement();
            }
        }
    }

}
