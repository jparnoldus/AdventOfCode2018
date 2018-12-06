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

            return 0;
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

    public class Margins
    {
        public int top;
        public int left;
        public int right;
        public int bottom;

        public static Margins Parse(List<Point> points)
        {
            return new Margins()
            {
                top = points.Min(p => p.y),
                left = points.Min(p => p.x),
                right = points.Max(p => p.x),
                bottom = points.Max(p => p.y)
            };
        }
    }

    public class Map
    {
        private List<Point> activePoints;
        private Point[,] map;
        private Margins margins;

        public void AddCoordinates(List<Point> points)
        {
            margins = Margins.Parse(points);
            map = new Point[margins.bottom - margins.top, margins.right - margins.left];

            foreach (Point point in points)
            {
                map[point.x - margins.left, point.y - margins.top] = point;
            }

            activePoints = points;
        }

        public void FillMap()
        {
            while (activePoints.Count != 0)
            {
                FillMap2();
            }
        }

        private void FillMap2()
        {
            List<Point> freshlyAddedCoordinates = new List<Point>();

            foreach (Point point in activePoints)
            {
                List<bool> results = new List<bool>();
                results.Add(FillCoordinate(point.x + 1, point.y, freshlyAddedCoordinates, map[point.x, point.y]));
                results.Add(FillCoordinate(point.x - 1, point.y, freshlyAddedCoordinates, map[point.x, point.y]));
                results.Add(FillCoordinate(point.x, point.y + 1, freshlyAddedCoordinates, map[point.x, point.y]));
                results.Add(FillCoordinate(point.x, point.y - 1, freshlyAddedCoordinates, map[point.x, point.y]));

                if (results.Contains(false))
                {
                    activePoints.Remove(point);
                }
            }
        }

        private bool FillCoordinate(int x, int y, List<Point> freshlyAddedCoordinates, Point point)
        {
            if (map[x, y] == null)
            {
                map[x, y] = point;
                Point newPoint = new Point()
                {
                    x = x,
                    y = y
                };

                freshlyAddedCoordinates.Add(newPoint);
                activePoints.Add(point);

                return true;
            }
            else if (freshlyAddedCoordinates.Find(p => p.x == x && p.y == y) != null)
            {
                map[x, y] = new Point()
                {
                    x = -1,
                    y = -1
                };
            }

            return false;
        }
    }
}
