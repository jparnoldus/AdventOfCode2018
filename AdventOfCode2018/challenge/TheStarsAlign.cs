using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class TheStarsAlign : Challenge
    {
        public static int AlignStars(int max)
        {
            List<Star> stars = GetStars();

            int seconds = 0;
            while (stars.Max(s => s.GetPositionAfter(seconds).x) - stars.Min(s => s.GetPositionAfter(seconds).x) > max)
            {
                seconds++;
            }

            do
            {
                for (int i = stars.Min(s => s.GetPositionAfter(seconds).x) - 1; i <= stars.Max(s => s.GetPositionAfter(seconds).x) + 1; i++)
                {
                    for (int j = stars.Min(s => s.GetPositionAfter(seconds).y) - 1; j <= stars.Max(s => s.GetPositionAfter(seconds).y) + 1; j++)
                    {
                        if (stars.Exists(s => s.GetPositionAfter(seconds).x == i && s.GetPositionAfter(seconds).y == j))
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }

                seconds++;
            }
            while (Console.ReadKey().KeyChar == 'n');

            return seconds - 1;
        }

        private static List<Star> GetStars()
        {
            List<Star> stars = new List<Star>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(10)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        stars.Add(new Star()
                        {
                            startPosition = Vector.Parse(line.Substring(line.IndexOf('<') + 1, line.IndexOf('>') - line.IndexOf('<') - 1).Trim(new char[] { ' ' })),
                            velocity = Vector.Parse(line.Substring(line.LastIndexOf('<') + 1, line.LastIndexOf('>') - line.LastIndexOf('<') - 1).Trim(new char[] { ' ' }))
                        });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return stars;
        }

        public class Star
        {
            public Vector startPosition;
            public Vector velocity;

            public Vector GetPositionAfter(int seconds)
            {
                return new Vector()
                {
                    x = startPosition.x + velocity.x * seconds,
                    y = startPosition.y + velocity.y * seconds
                };
            }
        }

        public class Vector
        {
            public int x;
            public int y;

            public static Vector Parse(string line)
            {
                string[] coordinates = line.Split(',');
                return new Vector()
                {
                    x = int.Parse(coordinates[1].Trim(new char[] { ' ' })),
                    y = int.Parse(coordinates[0].Trim(new char[] { ' ' }))
                };
            }
        }
    }
}
