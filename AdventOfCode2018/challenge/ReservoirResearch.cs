using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class ReservoirResearch : Challenge
    {
        public static void Do()
        {
            char[,] map = GetMap();

            int yOffset = 0;
            for (int i = 0; i < 100; i++)
            {
                Console.WriteLine(i);
                bool next = false;
                for (int y = yOffset; y < map.GetLength(1); y++)
                {
                    if (next) break;
                    for (int x = 0; x < map.GetLength(0); x++)
                    {
                        if (map[x, y] == '+' || map[x, y] == '|')
                        {
                            if (map[x, y + 1] == '.')
                            {
                                map[x, y + 1] = '|';
                            }
                            else if (map[x, y + 1] == '#' || map[x, y + 1] == '~')
                            {
                                map[x, y] = '~';
                                int counter = 1;
                                while (map[x - counter, y] != '#' && map[x - counter, y + 1] != '.')
                                {
                                    map[x - counter, y] = '~';
                                    counter++;
                                }

                                if (map[x - counter, y] == '.')
                                {
                                    map[x - counter, y] = '|';
                                    yOffset = y;
                                    next = true;
                                }

                                counter = 1;
                                while (map[x + counter, y] != '#' && map[x + counter, y + 1] != '.')
                                {
                                    map[x + counter, y] = '~';
                                    counter++;
                                }

                                if (map[x + counter, y] == '.')
                                {
                                    map[x + counter, y] = '|';
                                    yOffset = y;
                                    next = true;
                                }
                            }
                        }
                    }
                }
            }

            PrintMap(map);
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
            
            char[,] map = new char[yClays.Max(c => c[2]) - yClays.Min(c => c[1]) + 2, xClays.Max(c => c[2]) + 1];
            for (int y = 0; y < map.GetLength(1); y++)  for (int x = 0; x < map.GetLength(0); x++) map[x, y] = '.';
            xClays.ForEach(clay => { for (int y = clay[1]; y <= clay[2]; y++) map[clay[0] - yClays.Min(c => c[1]) + 1, y] = '#'; });
            yClays.ForEach(clay => { for (int x = clay[1]; x <= clay[2]; x++) map[x - yClays.Min(c => c[1]) + 1, clay[0]] = '#'; });
            map[500 - yClays.Min(c => c[1]) + 1, 0] = '+';

            return map;
        }

        public static void PrintMap(char[,] map)
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
