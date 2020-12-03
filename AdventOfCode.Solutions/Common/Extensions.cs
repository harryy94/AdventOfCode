using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Common
{
    public static class Extensions
    {
        public static List<int> GetIntList(this string input)
        {
            var list = input.Split('\n');

            return list.Where(x => !string.IsNullOrEmpty(x))
                .Select(int.Parse)
                .ToList();
        }
    }
}
