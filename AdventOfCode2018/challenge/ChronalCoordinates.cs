using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class ChronalCoordinates : Challenge
    {
        public static int GetLargestArea()
        {
            List<Point> list = GetList();
            Map map = new Map();

            map.AddCoordinates(list);
            map.FillMap();

            return map.GetLargestAreaSize();
        }

        public static int GetLessThanArea()
        {
            List<Point> list = GetList();
            Map map = new Map();

            map.AddCoordinates(list);
            map.FillMap();

            return map.GetRegionWithLessThan10000Distance();
        }

        private static List<Point> GetList()
        {
            List<Point> list = new List<Point>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(6)))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(Point.Parse(sr.ReadLine()));
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

    public class Point
    {
        public int x;
        public int y;

        public static Point Parse(string line)
        {
            var coordinates = line.Split(',');
            return new Point()
            {
                x = int.Parse(coordinates.First().Trim(new char[] { ' '})),
                y = int.Parse(coordinates.Last().Trim(new char[] { ' ' }))
            };
        }
    }

    public class Map
    {
        private List<Point> points;
        private Point[,] map;
        private int[,] sumMap;

        public void AddCoordinates(List<Point> points)
        {
            map = new Point[points.Max(p => p.x) + 1, points.Max(p => p.y) + 1];
            sumMap = new int[points.Max(p => p.x) + 1, points.Max(p => p.y) + 1];

            this.points = points;
            foreach (Point point in points)
            {
                map[point.x, point.y] = point;
            }
        }

        public void FillMap()
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Dictionary<Point, double> distances = new Dictionary<Point, double>();
                    foreach (Point point in this.points)
                    {
                        distances.Add(point, Math.Abs(i - point.x) + Math.Abs(j - point.y));
                    }

                    map[i, j] = distances.OrderByDescending(d => d.Value).Last().Key;
                    sumMap[i, j] = distances.Sum(d => (int)(d.Value));
                }
            }
        }

        public int GetLargestAreaSize()
        {
            List<Point> excludes = new List<Point>();
            Dictionary<Point, int> regions = new Dictionary<Point, int>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i <= points.Min(p => p.x) - 1 || i >= points.Max(p => p.x) - 1) excludes.Add(map[i, j]);
                    if (j <= points.Min(p => p.y) - 1 || j >= points.Max(p => p.y) - 1) excludes.Add(map[i, j]);

                    if (regions.ContainsKey(map[i, j]))
                    {
                        regions[map[i, j]]++;
                    }
                    else regions.Add(map[i, j], 1);
                }
            }

            excludes = excludes.Distinct().ToList();
            return regions.Where(r => !excludes.Contains(r.Key)).Max(r => r.Value);
        }

        public int GetRegionWithLessThan10000Distance()
        {
            int amount = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (sumMap[i, j] < 10000)
                    {
                        amount++;
                    }
                }
            }

            return amount;
        }
    }
}
