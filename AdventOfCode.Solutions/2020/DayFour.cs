using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions._2020
{
    public class DayFour : BaseProblem
    {
        public DayFour() : base(2020, 4)
        {
        }

        public override List<string> ExampleInput { get; }
            = new List<string>
            {
                @"ecl:gry pid:860033327 eyr:2020 hcl:#fffffd
byr:1937 iyr:2017 cid:147 hgt:183cm

iyr:2013 ecl:amb cid:350 eyr:2023 pid:028048884
hcl:#cfa07d byr:1929

hcl:#ae17e1 iyr:2013
eyr:2024
ecl:brn pid:760753108 byr:1931
hgt:179cm

hcl:#cfa07d eyr:2025 pid:166559648
iyr:2011 ecl:brn hgt:59in",

                @"eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007",
                @"pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719"
            };

        protected override void DoSolve(string input)
        {
            var passportList = new List<Dictionary<string, string>>();

            var passport = new Dictionary<string, string>();

            foreach (var line in input.Split('\n'))
            {
                if (string.IsNullOrEmpty(line) || line == "\r")
                {
                    // Ready for new passport
                    passportList.Add(passport);

                    passport = new Dictionary<string, string>();

                    continue;
                }

                foreach (var entry in line.Split(new[]{" "}, StringSplitOptions.RemoveEmptyEntries))
                {
                    var keyValueSplit = entry.Split(':');

                    passport.Add(keyValueSplit[0], keyValueSplit[1].Replace("\r", ""));
                }
            }

            var validPassportsPartOne = 0;
            var validPassportsPartTwo = 0;

            foreach (var passportToCheck in passportList)
            {
                if (IsPassportValidPartOne(passportToCheck))
                {
                    validPassportsPartOne++;
                }

                if (IsPassportValidPartTwo(passportToCheck))
                {
                    validPassportsPartTwo++;
                }
            }

            PartOneAnswer = validPassportsPartOne.ToString();
            PartTwoAnswer = validPassportsPartTwo.ToString();
        }

        private bool IsPassportValidPartOne(Dictionary<string, string> passport)
        {
            var requiredFields = new List<string>
            {
                "byr",
                "iyr",
                "eyr",
                "hgt",
                "hcl",
                "ecl",
                "pid",
                //"cid",
            };

            return requiredFields.All(passport.ContainsKey);
        }

        private bool IsPassportValidPartTwo(Dictionary<string, string> passport)
        {
            if (!IsPassportValidPartOne(passport))
                return false;


            int.TryParse(passport["byr"], out var byr);
            if (byr < 1920 || byr > 2002)
                return false;

            int.TryParse(passport["iyr"], out var iyr);
            if (iyr < 2010 || iyr > 2020)
                return false;

            int.TryParse(passport["eyr"], out var eyr);
            if (eyr < 2020 || eyr > 2030)
                return false;

            // Height
            if (passport["hgt"].EndsWith("cm"))
            {
                int.TryParse(passport["hgt"].Substring(0, passport["hgt"].Length - 2), out var height);

                if (height < 150 || height > 193)
                {
                    return false;
                }
            }
            else if (passport["hgt"].EndsWith("in"))
            {
                int.TryParse(passport["hgt"].Substring(0, passport["hgt"].Length - 2), out var height);

                if (height < 59 || height > 76)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            // HCL
            if (passport["hcl"].Length != 7 || !passport["hcl"].StartsWith("#"))
            {
                return false;
            }

            var hcl = passport["hcl"].Substring(1).ToLower();

            foreach (var letter in hcl)
            {
                if (letter >= '0' && letter <= '9')
                {
                    continue;
                }
                if (letter >= 'a' && letter <= 'f')
                {
                    continue;
                }

                return false;
            }

            if (!new List<string>
            {
                "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
            }.Contains(passport["ecl"]))
            {
                return false;
            }

            if (passport["pid"].Length != 9 || !int.TryParse(passport["pid"], out _))
            {
                return false;
            }

            return true;
            /*
             * byr (Birth Year) - four digits; at least 1920 and at most 2002.
                 iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                 eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                 hgt (Height) - a number followed by either cm or in:
                 If cm, the number must be at least 150 and at most 193.
                 If in, the number must be at least 59 and at most 76.
                 hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                 ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                 pid (Passport ID) - a nine-digit number, including leading zeroes.
                 cid (Country ID) - ignored, missing or not.
             */
        }
    }
}
