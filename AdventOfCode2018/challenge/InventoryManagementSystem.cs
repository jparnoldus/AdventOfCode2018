using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class InventoryManagementSystem : Challenge
    {
        public static int GetChecksum()
        {
            int doubleCount = 0, tripleCount = 0;

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(2)))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        line.GroupBy(c => c)
                            .ToDictionary(c => c.Key, c => c.Count())
                            .Where(c => c.Value > 1)
                            .Select(c => c.Value)
                            .Distinct()
                            .ToList()
                            .ForEach(c =>
                            {
                                doubleCount += c == 2 ? 1 : 0;
                                tripleCount += c == 3 ? 1 : 0;
                            });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return doubleCount * tripleCount;
        }

        public static string GetPrototype()
        {
            List<string> list1 = GetList(), list2 = GetList();
            foreach (string line1 in list1)
            {
                foreach (string line2 in list2)
                {
                    if (line1.Except(line2).Count() == 1)
                    {
                        for (int i = 0; i < line1.Length; i++)
                        {
                            for (int j = 0; j < line2.Length; j++) // :^)
                            {
                                if (line1.Remove(i, 1) == line2.Remove(j, 1))
                                {
                                    return line1.Remove(i, 1);
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        private static List<string> GetList()
        {
            List<string> list = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(2)))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return list;
        }
    }
}
