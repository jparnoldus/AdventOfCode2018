using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class BeverageBandits : Challenge
    {
        public static void GetOutcome()
        {
            (Tile[,] map, Dictionary<Point, Character> characters)  state = GetStartingState();
        }

        private static (Tile[,] map, Dictionary<Point, Character> characters) GetStartingState()
        {
            Tile[,] map = new Tile[32, 32];
            Dictionary<Point, Character> characters = new Dictionary<Point, Character>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(15)))
                {
                    int y = 0;
                    while (!sr.EndOfStream)
                    {
                        int x = 0;
                        string line = sr.ReadLine();
                        foreach (char tile in line)
                        {
                            Point newPoint = new Point(x, y);

                            if (tile == '#')
                                map[x, y] = new Tile(Tile.Type.WALL, newPoint);
                            else
                                map[x, y] = new Tile(Tile.Type.FLOOR, newPoint);

                            if (tile == 'G')
                                characters.Add(newPoint, new Character(Character.Type.GOBLIN, newPoint));
                            else if (tile == 'E')
                                characters.Add(newPoint, new Character(Character.Type.ELF, newPoint));

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

            return (map, characters);
        }

        public class Point
        {
            public int x;
            public int y;

            public Point(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point Clone()
            {
                return new Point(this.x, this.y);
            }

            public override int GetHashCode()
            {
                return string.Format("{0},{1}", this.x, this.y).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null && (other.x == this.x && other.y == this.y);
            }
        }

        class Tile
        {
            public enum Type {
                WALL,
                FLOOR
            }

            public Type type;
            public Point location;

            public Tile(Type type, Point location)
            {
                this.type = type;
                this.location = location;
            }
        }

        class Character
        {
            public enum Type {
                ELF,
                GOBLIN
            }

            public Type type;
            public Point location;
            public int hitpoints = 200;
            public int attack = 3;

            public Character(Type type, Point location)
            {
                this.type = type;
                this.location = location;
            }
        }
    }
}
