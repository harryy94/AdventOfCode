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

        public static List<long> GetLongList(this string input)
        {
            var list = input.Split('\n');

            return list.Where(x => !string.IsNullOrEmpty(x))
                .Select(long.Parse)
                .ToList();
        }

        public static List<string> SplitByLine(this string input)
        {
            var list = input.Split('\n');

            return list.Select(x => x.Replace("\r", "")).ToList();
        }

        public static string ConvertToBase(this int base10, int baseToConvert)
        {
            const int bitsInLong = 64;
            const string digits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

            if (base10 == 0)
            {
                return "0";
            }

            var index = bitsInLong - 1;
            long currentNumber = Math.Abs(base10);
            var charArray = new char[bitsInLong];

            while (currentNumber != 0)
            {
                int remainder = (int)(currentNumber % baseToConvert);
                charArray[index--] = digits[remainder];
                currentNumber /= baseToConvert;
            }

            var result = new string(charArray, index + 1, bitsInLong - index - 1);
            if (base10 < 0)
            {
                result = "-" + result;
            }

            return result;
        }
    }
}
