using System;
using System.Linq;
using System.Reflection;
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

            RunSpecificTest(new DayNine());
            //RunAWholeYear(2020);

            System.Console.ReadLine();
        }

        private static void RunSpecificTest(BaseProblem problem)
        {
            problem.Solve();
        }

        private static void RunAWholeYear(int year)
        {
            var types = Assembly.GetAssembly(typeof(BaseProblem)).GetTypes()
                .Where(x => x.Namespace != null &&
                            x.Namespace.EndsWith(year.ToString()) &&
                            x.BaseType != null &&
                            x.BaseType == typeof(BaseProblem))
                .Select(Activator.CreateInstance)
                .Cast<BaseProblem>()
                .OrderBy(x => x.Day)
                .ToList();

            foreach (var type in types)
            {
                type.RunExamples = false;
                type.Solve();
            }
        }
    }
}
