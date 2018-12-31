using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class BeverageBandits : Challenge
    {
        public static int GetOutcome()
        {
            (Tile[,] map, Dictionary<Point, Character> characters) state = GetStartingState();
            int rounds = 0;
            while (state.characters.Select(c => c.Value.type).Distinct().Count() > 1)
            {
                PrintState(state);
                state = Round(state);
                rounds++;
            }

            return (rounds - 1) * state.characters.Sum(c => c.Value.hitpoints);
        }

        public static int GetBetterOutcome()
        {
            (Tile[,] map, Dictionary<Point, Character> characters) state;
            int newAttackValue = 4, rounds = 0, elfCount = 0;
            do
            {
                state = GetStartingState();
                elfCount = state.characters.ToList().Where(c => c.Value.type == Character.Type.ELF).Count();
                state.characters.ToList().Where(c => c.Value.type == Character.Type.ELF).ToList().ForEach(c => c.Value.attack = newAttackValue);

                rounds = 0;
                while (state.characters.Select(c => c.Value.type).Distinct().Count() > 1)
                {
                    state = Round(state);
                    rounds++;
                }

                newAttackValue++;
            }
            while (!state.characters.Values.ToList().TrueForAll(c => c.type == Character.Type.ELF) || elfCount != state.characters.Values.Count);

            return (rounds - 1) * state.characters.Sum(c => c.Value.hitpoints);
        }

        private static (Tile[,] map, Dictionary<Point, Character> characters) Round((Tile[,] map, Dictionary<Point, Character> characters) state)
        {
            for (int y = 0; y < state.map.GetLength(1); y++)
            {
                for (int x = 0; x < state.map.GetLength(0); x++)
                {
                    Point point = new Point(x, y);
                    if (state.characters.ContainsKey(point))
                    {
                        Character current = state.characters[point];
                        if (!current.moved)
                        {
                            List<Point> targets = new List<Point>();
                            List<Character> inRange = new List<Character>();
                            foreach (Character character in state.characters.Values.Where(c => c.type != current.type))
                            {
                                List<Point> characterTargets = Character.GetTargets(state.map, character);
                                if (characterTargets.Contains(current.location))
                                {
                                    inRange.Add(character);
                                }

                                targets.AddRange(characterTargets);
                            }

                            if (inRange.Count > 0)
                            {
                                Character target = inRange.OrderBy(c => c.hitpoints).First();
                                target.hitpoints = target.hitpoints - current.attack;

                                if (target.hitpoints < 1)
                                {
                                    state.characters.Remove(state.characters.Where(c => c.Value.Equals(target)).First().Key);
                                }
                            }
                            else
                            {
                                targets.Except(state.characters.Keys);
                                targets.Sort(Point.Compare);

                                List<Point> visited = new List<Point>();
                                visited.Add(current.location);

                                List<Point> stepped = Step(state, current.location, visited);
                                while (!stepped.Exists(s => targets.Contains(s)) && stepped.Count > 0)
                                {
                                    List<Point> newStepped = new List<Point>();
                                    foreach (Point step in stepped)
                                    {
                                        newStepped.AddRange(Step(state, step, visited));
                                    }

                                    stepped = newStepped;
                                };

                                if (stepped.Count > 0)
                                {
                                    List<Point> found = stepped.Where(s => targets.Contains(s)).ToList();
                                    found.Sort(Point.Compare);
                                    
                                    Point next = GetNextMove(state, current.location, found.First());

                                    state.characters.Remove(current.location);
                                    current.location = next;
                                    current.moved = true;
                                    state.characters.Add(next, current);
                                }

                                inRange = new List<Character>();
                                foreach (Character character in state.characters.Values.Where(c => c.type != current.type))
                                {
                                    List<Point> characterTargets = Character.GetTargets(state.map, character);
                                    if (characterTargets.Contains(current.location))
                                    {
                                        inRange.Add(character);
                                    }

                                    targets.AddRange(characterTargets);
                                }

                                if (inRange.Count > 0)
                                {
                                    Character target = inRange.OrderBy(c => c.hitpoints).First();
                                    target.hitpoints = target.hitpoints - current.attack;

                                    if (target.hitpoints < 1)
                                    {
                                        state.characters.Remove(state.characters.Where(c => c.Value.Equals(target)).First().Key);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            state.characters.ToList().ForEach(c => c.Value.moved = false);
            return state;
        }

        private static List<Point> Step((Tile[,] map, Dictionary<Point, Character> characters) state, Point point, List<Point> visited)
        {
            List<Point> possibleSteps = new List<Point>();
            possibleSteps.Add(new Point(point.x + 1, point.y));
            possibleSteps.Add(new Point(point.x - 1, point.y));
            possibleSteps.Add(new Point(point.x, point.y + 1));
            possibleSteps.Add(new Point(point.x, point.y - 1));

            possibleSteps = possibleSteps.Except(visited).ToList();
            possibleSteps = possibleSteps.Except(possibleSteps.Where(s => state.map[s.x, s.y].type == Tile.Type.WALL)).ToList();
            possibleSteps = possibleSteps.Except(state.characters.Keys).ToList();

            if (possibleSteps.Count > 1)
                possibleSteps.Sort(Point.Compare);

            visited.AddRange(possibleSteps);
            return possibleSteps;
        }

        private static Point GetNextMove((Tile[,] map, Dictionary<Point, Character> characters) state, Point current, Point target)
        {
            List<Point> possibleSteps = new List<Point>();
            possibleSteps.Add(new Point(current.x + 1, current.y));
            possibleSteps.Add(new Point(current.x - 1, current.y));
            possibleSteps.Add(new Point(current.x, current.y + 1));
            possibleSteps.Add(new Point(current.x, current.y - 1));

            if (possibleSteps.Contains(target))
                return target;

            possibleSteps = possibleSteps.Except(possibleSteps.Where(s => state.map[s.x, s.y].type == Tile.Type.WALL)).ToList();
            possibleSteps = possibleSteps.Except(state.characters.Keys).ToList();
            possibleSteps.Sort(Point.Compare);

            Dictionary<Point, int> stepsFromPoint = new Dictionary<Point, int>();
            foreach (Point possibleStep in possibleSteps)
            {
                List<Point> visited = new List<Point>();
                visited.Add(possibleStep);

                int count = 0;
                List<Point> stepped = Step(state, possibleStep, visited);

                if (stepped.Count == 0)
                {
                    continue;
                }

                while (!stepped.Contains(target) && stepped.Count != 0)
                {
                    if (stepped.Contains(target))
                        break;

                    List<Point> newStepped = new List<Point>();
                    foreach (Point step in stepped)
                    {
                        newStepped.AddRange(Step(state, step, visited));
                    }

                    count++;
                    stepped = newStepped;
                };

                if (stepped.Contains(target))
                    stepsFromPoint.Add(possibleStep, count);
            }

            return stepsFromPoint.OrderBy(s => s.Value).First().Key;
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

            public static int Compare(Point x, Point y)
            {
                if (x == null)
                {
                    if (y == null)
                    {
                        return 0;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else if (x.Equals(y))
                {
                    return 0;
                }
                else if (x.y == y.y && y.x < x.x)
                {
                    return 1;
                }
                else if (x.y > y.y)
                {
                    return 1;
                }

                return -1;
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
            public bool moved = false;

            public Character(Type type, Point location)
            {
                this.type = type;
                this.location = location;
            }

            public static List<Point> GetTargets(Tile[,] map, Character character)
            {
                List<Point> targets = new List<Point>();
                targets.Add(new Point(character.location.x + 1, character.location.y));
                targets.Add(new Point(character.location.x - 1, character.location.y));
                targets.Add(new Point(character.location.x, character.location.y + 1));
                targets.Add(new Point(character.location.x, character.location.y - 1));

                return targets.Where(t => map[t.x, t.y].type != Tile.Type.WALL).ToList();
            }
        }

        private static void PrintState((Tile[,] map, Dictionary<Point, Character> characters) state)
        {
            for (int y = 0; y < state.map.GetLength(1); y++)
            {
                for (int x = 0; x < state.map.GetLength(0); x++)
                {
                    Point point = new Point(x, y);
                    if (state.characters.ContainsKey(point))
                    {
                        if (state.characters[point].type == Character.Type.ELF)
                            Console.Write('E');
                        else Console.Write('G');
                    }
                    else
                    {
                        if (state.map[x, y].type == Tile.Type.FLOOR)
                            Console.Write('.');
                        else Console.Write('#');
                    }
                }

                Console.WriteLine();
            }
        }
    }
}
