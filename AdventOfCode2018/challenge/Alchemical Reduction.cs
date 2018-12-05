using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode2018.challenge
{
    class AlchemicalReduction : Challenge
    {
        public static int AlchemicallyReduce()
        {
            string polymer = GetPolymer();
            
            string reducedPolymer = Reduce(polymer, CreatePattern());

            return reducedPolymer.Length;
        }

        public static int GetBestOnceReduce()
        {
            string polymer = GetPolymer();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            List<string> reduced = new List<string>();
            foreach (char letter in alphabet)
            {
                string newPolymer = polymer.Replace(Char.ToLower(letter).ToString(), "");
                newPolymer = newPolymer.Replace(Char.ToUpper(letter).ToString(), "");
                reduced.Add(newPolymer);
            }

            int shortest = int.MaxValue;
            foreach (string reduction in reduced)
            {
                string reducedPolymer = Reduce(reduction, CreatePattern());
                if (shortest > reducedPolymer.Length) shortest = reducedPolymer.Length;
            }

            return shortest;
        }

        private static List<string> CreatePattern()
        {
            List<string> pattern = new List<string>();
            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            foreach (char letter in alphabet)
            {
                pattern.Add(letter.ToString() + Char.ToUpper(letter));
                pattern.Add(Char.ToUpper(letter) + letter.ToString());
            }
            
            return pattern;
        }

        private static string Reduce(string polymer, List<string> pattern)
        {
            int remember = polymer.Length;

            // Avoiding regex at all costs
            foreach (string thing in pattern)
            {
                polymer = polymer.Replace(thing, "");
            }

            return remember != polymer.Length ? Reduce(polymer, pattern) : polymer;
        }

        private static string GetPolymer()
        {
            string polymer;

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(5)))
                {
                    polymer = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return polymer;
        }
    }
}
