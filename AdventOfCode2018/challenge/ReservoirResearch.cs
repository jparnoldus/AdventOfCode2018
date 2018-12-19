using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class ReservoirResearch : Challenge
    {
        public static void Do()
        {
            GetMap();
        }

        private static string GetMap()
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
                        {
                            xClays.Add(line.Replace('.', ',').Replace('=', ' ').Replace('x', ' ').Replace('y', ' ').Split(',').Where(c => c != string.Empty).Select(c => int.Parse(c.Trim(' '))).ToArray());
                        }
                        else
                        {
                            yClays.Add(line.Replace('.', ',').Replace('=', ' ').Replace('x', ' ').Replace('y', ' ').Split(',').Where(c => c != string.Empty).Select(c => int.Parse(c.Trim(' '))).ToArray());
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            char[,] map = new char[yClays.Max(c => c[2]) - yClays.Min(c => c[1]) + 2, xClays.Max(c => c[2])];
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    map[x, y] = '.';
                }
            }

            int xOffset = yClays.Min(c => c[1]) + 1;
            foreach (int[] clay in xClays)
            {
                
            }

            foreach (int[] clay in yClays)
            {

            }

            return null;
        }
    }
}
