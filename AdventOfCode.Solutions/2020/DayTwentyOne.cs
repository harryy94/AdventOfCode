using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AdventOfCode.Solutions.Common;

namespace AdventOfCode.Solutions._2020
{
    public class DayTwentyOne : BaseProblem
    {
        public DayTwentyOne() : base(2020, 21)
        {
        }

        public override List<string> ExampleInput { get; } = new List<string>
        {
            @"mxmxvkd kfcds sqjhc nhms (contains dairy, fish)
trh fvjkl sbzzf mxmxvkd (contains dairy)
sqjhc fvjkl (contains soy)
sqjhc mxmxvkd sbzzf (contains fish)"
        };

        private Regex _inputLineMatch = new Regex(@"(.*)\((.*)\)");

        protected override void DoSolve(string input)
        {
            var knownAllergens = new List<KnownAllergen>();
            var allIngredients = new Dictionary<string, int>();

            foreach (var line in input.SplitByLine())
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                var regex = _inputLineMatch.Match(line);

                var ingredients = regex.Groups[1].Value
                    .Split(' ')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .ToList();

                foreach (var ingredient in ingredients)
                {
                    if (allIngredients.ContainsKey(ingredient))
                    {
                        allIngredients[ingredient]++;
                    }
                    else
                    {
                        allIngredients.Add(ingredient, 1);
                    }
                }

                var allergens = regex.Groups[2].Value
                    .Replace("contains", "")
                    .Split(',')
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .ToList();

                foreach (var allergen in allergens)
                {
                    var currentAllergen = knownAllergens.SingleOrDefault(x => x.Allergen == allergen);

                    if (currentAllergen == null)
                    {
                        knownAllergens.Add(new KnownAllergen(allergen, ingredients));
                    }
                    else
                    {
                        var newList = currentAllergen.Ingredients.Intersect(ingredients).ToList();
                        currentAllergen.Ingredients = newList;
                    }
                }
            }

            PartOneAnswer = SolvePart1(allIngredients, knownAllergens).ToString();
            PartTwoAnswer = SolvePart2(knownAllergens);
        }

        private string SolvePart2(List<KnownAllergen> knownAllergens)
        {
            var knownAllergensDictionary = new SortedDictionary<string, string>();

            while (knownAllergens.Any(x => x.Ingredients.Count > 0))
            {
                var newKnownAllergens = knownAllergens
                    .Where(x => x.Ingredients.Count == 1)
                    .ToDictionary(x => x.Allergen, v => v.Ingredients.First());

                foreach (var entry in newKnownAllergens)
                {
                    knownAllergensDictionary.Add(entry.Key, entry.Value);
                }

                foreach (var item in knownAllergens)
                {
                    var knownAllergen = knownAllergens.Single(x => x.Allergen == item.Allergen);
                    knownAllergen.Ingredients = item.Ingredients.Except(newKnownAllergens.Values).ToList();
                }
            }

            var result = string.Join(",", knownAllergensDictionary.Select(x => x.Value));

            return result;
        }

        private int SolvePart1(Dictionary<string, int> allIngredients, List<KnownAllergen> knownAllergens)
        {
            var excludedIngredients = 0;
            foreach (var ingredient in allIngredients)
            {
                if (!knownAllergens.Any(item => item.Ingredients.Contains(ingredient.Key)))
                {
                    excludedIngredients += ingredient.Value;
                }
            }

            return excludedIngredients;
        }
    }

    public class KnownAllergen
    {
        public KnownAllergen(string allergen, List<string> ingredients)
        {
            Allergen = allergen;
            Ingredients = ingredients;
        }

        public string Allergen { get; set; }

        public List<string> Ingredients { get; set; }
    }
}
