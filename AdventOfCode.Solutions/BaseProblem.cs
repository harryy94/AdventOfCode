using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace AdventOfCode.Solutions
{
    public abstract class BaseProblem
    {
        protected Action<string> LoggingAction = Console.WriteLine;

        protected BaseProblem(int year, int day)
        {
            Year = year;
            Day = day;
        }

        public int Year { get; }

        public int Day { get; }

        public abstract List<string> ExampleInput { get; }

        public string PartOneAnswer { get; protected set; }

        public string PartTwoAnswer { get; protected set;  }

        public virtual bool RunActual { get; set; } = true;

        public virtual bool RunExamples { get; set; } = true;

        public void Solve(string name, string input)
        {
            PartOneAnswer = null;
            PartTwoAnswer = null;

            LoggingAction($"-> Solve({Year}, {Day}, \"{name}\")");

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            DoSolve(input);

            stopWatch.Stop();

            LoggingAction($"Answers: [Part 1: {PartOneAnswer}] | [Part 2: {PartTwoAnswer}]");
            LoggingAction($"<- Solve({Year}, {Day}, \"{name}\") - Execution time: {stopWatch.ElapsedMilliseconds}ms");
        }

        protected abstract void DoSolve(string input);

        public void Solve()
        {
            if (ExampleInput != null && RunExamples)
            {
                for (var i = 0; i < ExampleInput.Count; i++)
                {
                    Solve("Example " + i, ExampleInput[i]);
                }
            }

            var actualInput = GetActualPuzzleInput();

            if (RunActual)
            {
                Solve("Actual", actualInput);
            }
        }

        private string GetActualPuzzleInput()
        {
            // Check a file cache first. Only pull from AoC if we don't already know it.

            var fileName = $"PuzzleInput_{Year}_{Day}.txt";

            if (!Directory.Exists(Configuration.PuzzleDirectory))
            {
                Directory.CreateDirectory(Configuration.PuzzleDirectory);
            }

            var dir = Path.Combine(Configuration.PuzzleDirectory, fileName);

            if (File.Exists(dir))
            {
                return File.ReadAllText(dir);
            }

            //Get from AoC
            string input;

            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, Configuration.Cookie);
                input = client.DownloadString($"{Configuration.BaseUrl}/{Year}/day/{Day}/input");
            }

            File.WriteAllText(dir, input);

            return input;
        }
    }
}
