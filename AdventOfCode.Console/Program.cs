using System;
using AdventOfCode.Solutions;
using AdventOfCode.Solutions._2019;
using AdventOfCode.Solutions._2020;

namespace AdventOfCode.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            Configuration.Cookie =
                "";

            BaseProblem problem = new DayTwo();

            problem.Solve();

            System.Console.ReadLine();
        }
    }
}
