using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class SettlersOfTheNorthPole : Challenge
    {
        public static int GetResourceValue(int minutes)
        {
            int width = 50, height = 50;
            char[,] map = GetMap();
            
            int result = 0, limit = (minutes > 1000) ? 1000 : minutes, m = 0;
            for (m = 0; m < limit; m++)
            {
                map = StepMap(map, width, height);
                result = GetValue(map, width, height);
            }

            int magicNumber = 35;
            while(m % magicNumber != minutes % magicNumber)
            {
                map = StepMap(map, width, height);
                result = GetValue(map, width, height);
                m++;
            }

            return result;
        }

        private static char[,] StepMap(char[,] map, int height, int width)
        {
            char[,] newMap = map.Clone() as char[,];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<Char> surroundings = new List<char>();

                    if (x == 0 && y == 0)
                    {
                        surroundings.Add(map[x + 1, y]);
                        surroundings.Add(map[x, y + 1]);
                        surroundings.Add(map[x + 1, y + 1]);
                    }
                    else if (x == width - 1 && y == 0)
                    {
                        surroundings.Add(map[x - 1, y]);
                        surroundings.Add(map[x, y + 1]);
                        surroundings.Add(map[x - 1, y + 1]);
                    }
                    else if (x == 0 && y == height - 1)
                    {
                        surroundings.Add(map[x + 1, y]);
                        surroundings.Add(map[x, y - 1]);
                        surroundings.Add(map[x + 1, y - 1]);
                    }
                    else if (x == width - 1 && y == height - 1)
                    {
                        surroundings.Add(map[x - 1, y]);
                        surroundings.Add(map[x, y - 1]);
                        surroundings.Add(map[x - 1, y - 1]);
                    }
                    else if (y == 0)
                    {
                        surroundings.Add(map[x + 1, y]);
                        surroundings.Add(map[x - 1, y]);
                        surroundings.Add(map[x + 1, y + 1]);
                        surroundings.Add(map[x, y + 1]);
                        surroundings.Add(map[x - 1, y + 1]);
                    }
                    else if (y == height - 1)
                    {
                        surroundings.Add(map[x + 1, y]);
                        surroundings.Add(map[x - 1, y]);
                        surroundings.Add(map[x + 1, y - 1]);
                        surroundings.Add(map[x, y - 1]);
                        surroundings.Add(map[x - 1, y - 1]);
                    }
                    else if (x == 0)
                    {
                        surroundings.Add(map[x, y - 1]);
                        surroundings.Add(map[x, y + 1]);
                        surroundings.Add(map[x + 1, y - 1]);
                        surroundings.Add(map[x + 1, y]);
                        surroundings.Add(map[x + 1, y + 1]);
                    }
                    else if (x == width - 1)
                    {
                        surroundings.Add(map[x, y - 1]);
                        surroundings.Add(map[x, y + 1]);
                        surroundings.Add(map[x - 1, y - 1]);
                        surroundings.Add(map[x - 1, y]);
                        surroundings.Add(map[x - 1, y + 1]);
                    }
                    else
                    {
                        surroundings.Add(map[x + 1, y + 1]);
                        surroundings.Add(map[x, y + 1]);
                        surroundings.Add(map[x - 1, y + 1]);
                        surroundings.Add(map[x - 1, y]);
                        surroundings.Add(map[x + 1, y]);
                        surroundings.Add(map[x + 1, y - 1]);
                        surroundings.Add(map[x, y - 1]);
                        surroundings.Add(map[x - 1, y - 1]);
                    }

                    if (map[x, y] == '.')
                    {
                        if (surroundings.Count(s => s == '|') >= 3)
                            newMap[x, y] = '|';
                    }
                    else if (map[x, y] == '|')
                    {
                        if (surroundings.Count(s => s == '#') >= 3)
                            newMap[x, y] = '#';
                    }
                    else if (map[x, y] == '#')
                    {
                        if (surroundings.Count(s => s == '#') >= 1 && surroundings.Count(s => s == '|') >= 1)
                            newMap[x, y] = '#';
                        else newMap[x, y] = '.';
                    }
                }
            }

            return newMap;
        }

        private static int GetValue(char[,] map, int height, int width)
        {
            int lumberyardCount = 0, woodedCount = 0;
            for (int y = 0; y < width; y++)
            {
                for (int x = 0; x < height; x++)
                {
                    if (map[x, y] == '#')
                        lumberyardCount++;
                    else if (map[x, y] == '|')
                        woodedCount++;
                }
            }

            return lumberyardCount * woodedCount;
        }

        private static char[,] GetMap()
        {
            char[,] map = new char[50, 50];

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(18)))
                {
                    int y = 0;
                    while (!sr.EndOfStream)
                    {
                        int x= 0;
                        string line = sr.ReadLine();
                        foreach (char letter in line)
                        {
                            map[x, y] = letter;

                            x++;
                        }

                        y++;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return map;
        }
    }
}
