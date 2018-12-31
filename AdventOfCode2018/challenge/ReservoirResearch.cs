using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class ReservoirResearch : Challenge
    {
        public static int Do()
        {
            char[,] map = GetMap();

            for (int rounds = 0; rounds < 100; rounds++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        if (map[x, y] == '+' || map[x, y] == '|')
                        {
                            if (map[x, y + 1] == '.')
                            {
                                while (map[x, y + 1] != '#' && y + 1 < map.GetLength(1) - 1)
                                {
                                    map[x, y + 1] = '|';
                                    y++;
                                }

                                if (y + 1 == map.GetLength(1) - 1)
                                    continue;

                                int rightBound = 0, leftBound = 0;
                                do
                                {
                                    rightBound = leftBound = 0;
                                    while (map[x + rightBound, y] != '#' && map[x + rightBound, y + 1] != '.')
                                    {
                                        rightBound++;
                                    }

                                    while (map[x - leftBound, y] != '#' && map[x - leftBound, y + 1] != '.')
                                    {
                                        leftBound++;
                                    }

                                    for (int t = x - leftBound + 1; t < x + rightBound; t++)
                                    {
                                        map[t, y] = '~';
                                    }

                                    y--;
                                }
                                while (map[x - leftBound, y] == '#' && map[x + rightBound, y] == '#');

                                for (int t = x - leftBound; t <= x + rightBound; t++)
                                {
                                    if (map[t, y] == '.')
                                        map[t, y] = '|';
                                }

                                if (map[x - leftBound, y] == '|')
                                {
                                    map[x - leftBound - 1, y] = '|';
                                }

                                if (map[x + rightBound, y] == '|')
                                {
                                    map[x + rightBound + 1, y] = '|';
                                }

                                rightBound = leftBound = 0;
                                while (x + rightBound < map.GetLength(0) - 1 && map[x + rightBound, y] != '#')
                                {
                                    rightBound++;
                                }

                                while (x - leftBound > 0 && map[x - leftBound, y] != '#')
                                {
                                    leftBound++;
                                }

                                List<char> thing = new List<char>();
                                for (int t = x - leftBound; t < x + rightBound + 1; t++)
                                {
                                    thing.Add(map[t, y]);
                                }

                                if (thing.Count(c => c == '#') == 2 && thing.Count(c => c == '|' || c == '~') == thing.Count - 2)
                                {
                                    rightBound = leftBound = 0;
                                    do
                                    {
                                        rightBound = leftBound = 0;
                                        while (map[x + rightBound, y] != '#')
                                        {
                                            rightBound++;
                                        }

                                        while (map[x - leftBound, y] != '#')
                                        {
                                            leftBound++;
                                        }

                                        for (int t = x - leftBound + 1; t < x + rightBound; t++)
                                        {
                                            map[t, y] = '~';
                                        }

                                        y--;
                                    }
                                    while (map[x - leftBound, y] == '#' && map[x + rightBound, y] == '#');

                                    for (int t = x - leftBound; t <= x + rightBound; t++)
                                    {
                                        if (map[t, y] == '.')
                                            map[t, y] = '|';
                                    }

                                    if (map[x - leftBound, y] == '|')
                                    {
                                        map[x - leftBound - 1, y] = '|';
                                    }

                                    if (map[x + rightBound, y] == '|')
                                    {
                                        map[x + rightBound + 1, y] = '|';
                                    }
                                }
                            }
                            else if (map[x, y + 1] == '#')
                            {
                                map[x - 1, y] = '|';
                                map[x + 1, y] = '|';

                                int rightBound = 0, leftBound = 0;
                                while (x + rightBound < map.GetLength(0) - 1 && map[x + rightBound, y] != '#')
                                {
                                    rightBound++;
                                }

                                while (x - leftBound > 0 && map[x - leftBound, y] != '#')
                                {
                                    leftBound++;
                                }

                                List<char> thing = new List<char>();
                                for (int t = x - leftBound; t < x + rightBound + 1; t++)
                                {
                                    thing.Add(map[t, y]);
                                }

                                if (thing.Count(c => c == '#') == 2 && thing.Count(c => c == '|' || c == '~') == thing.Count - 2)
                                {
                                    rightBound = leftBound = 0;
                                    do
                                    {
                                        rightBound = leftBound = 0;
                                        while (map[x + rightBound, y] != '#')
                                        {
                                            rightBound++;
                                        }

                                        while (map[x - leftBound, y] != '#')
                                        {
                                            leftBound++;
                                        }

                                        for (int t = x - leftBound + 1; t < x + rightBound; t++)
                                        {
                                            map[t, y] = '~';
                                        }

                                        y--;
                                    }
                                    while (map[x - leftBound, y] == '#' && map[x + rightBound, y] == '#');

                                    for (int t = x - leftBound; t <= x + rightBound; t++)
                                    {
                                        if (map[t, y] == '.')
                                            map[t, y] = '|';
                                    }

                                    if (map[x - leftBound, y] == '|')
                                    {
                                        map[x - leftBound - 1, y] = '|';
                                    }

                                    if (map[x + rightBound, y] == '|')
                                    {
                                        map[x + rightBound + 1, y] = '|';
                                    }
                                }
                            }
                        }
                    }
                }
            }

            PrintMap(map);
            List<List<char>> converted = new List<List<char>>();
            for (int y = 0; y < map.GetLength(1); y++)
            {
                List<char> list = new List<char>();
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    list.Add(map[x, y]);
                }

                converted.Add(list);
            }

            converted = converted.SkipWhile(c => !c.Contains('#')).ToList();
            converted.Reverse();
            converted = converted.SkipWhile(c => !c.Contains('#')).ToList();

            int sum = 0;
            foreach (List<char> list in converted)
            {
                sum += list.Count(c => c == '~');
            }

            return sum;
        }

        private static char[,] GetMap()
        {
            List<int[]> xClays = new List<int[]>();
            List<int[]> yClays = new List<int[]>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(17)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        if (line.StartsWith("x"))
                            xClays.Add(line.Replace('.', ',').Replace('=', ' ').Replace('x', ' ').Replace('y', ' ').Split(',').Where(c => c != string.Empty).Select(c => int.Parse(c.Trim(' '))).ToArray());
                        else yClays.Add(line.Replace('.', ',').Replace('=', ' ').Replace('x', ' ').Replace('y', ' ').Split(',').Where(c => c != string.Empty).Select(c => int.Parse(c.Trim(' '))).ToArray());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            char[,] map = new char[yClays.Max(c => c[2]) - yClays.Min(c => c[1]) + 2, xClays.Max(c => c[2]) + 10];
            for (int y = 0; y < map.GetLength(1); y++) for (int x = 0; x < map.GetLength(0); x++) map[x, y] = '.';
            xClays.ForEach(clay => { for (int y = clay[1]; y <= clay[2]; y++) map[clay[0] - yClays.Min(c => c[1]) + 1, y] = '#'; });
            yClays.ForEach(clay => { for (int x = clay[1]; x <= clay[2]; x++) map[x - yClays.Min(c => c[1]) + 1, clay[0]] = '#'; });
            map[500 - yClays.Min(c => c[1]) + 1, 0] = '+';

            return map;
        }

        private static void PrintMap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y]);
                }

                Console.WriteLine();
            }
        }
    }
}
