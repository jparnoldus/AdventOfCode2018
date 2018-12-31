using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class ModeMaze : Challenge
    {
        public static int GetRiskLevel()
        {
            int depth = 7740;
            int targetX = 12;
            int targetY = 763;
            int[,] map = GenerateMap(depth, targetX, targetY, 0, 0);

            int riskLevel = 0;
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    riskLevel += map[x, y];
                }
            }

            return riskLevel;
        }

        public static int GetTraversalTime()
        {
            // 6969
            // 9, 796
            // 1087

            int depth = 50;
            Point target = new Point(10, 10);
            int[,] map = GenerateMap(depth, target.x, target.y, 30, 40);

            return Dijkstra(map, new Point(0, 0), target);
            //return BFS(new Point(0,0), target, map);
        }

        private static int Dijkstra(int[,] map, Point source, Point target)
        {
            Dictionary<Point, int> set = new Dictionary<Point, int>();
            Dictionary<Point, int> distances = new Dictionary<Point, int>();
            Dictionary<Point, Point> previous = new Dictionary<Point, Point>();
            Dictionary<Point, State> states = new Dictionary<Point, State>();

            states.Add(source, new State());

            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Point point = new Point(x, y);

                    distances.Add(point, int.MaxValue);
                    previous.Add(point, null);
                    set.Add(point, map[x, y]);
                }
            }

            distances[source] = 0;

            while (set.Any())
            {
                if (set.Count % 100 == 0)
                    Console.WriteLine(set.Count);

                Point current = distances.Where(d => set.ContainsKey(d.Key)).OrderBy(d => d.Value).Select(d => d.Key).ToArray()[0];
                set.Remove(current);

                //if (current.Equals(target))
                    //break;

                List<Point> neighbours = GetNeighbours(current, map);
                foreach (Point neighbour in neighbours)
                {
                    (State newState, int timeSpend) result = GetTimeSpend(map, current, neighbour, states[current], target);
                    int alt = distances[current] + result.timeSpend;
                    if (alt < distances[neighbour])
                    {
                        states[neighbour] = result.newState;
                        distances[neighbour] = alt;
                        previous[neighbour] = current;
                    }
                }
            }
            
            List<Point> sequence = new List<Point>();
            Point pointer = target;
            while (!pointer.Equals(source))
            {
                sequence.Add(pointer);
                pointer = previous[pointer];
            }

            sequence.Add(new Point(0, 0));
            sequence.Reverse();

            Point[] temp = sequence.ToArray();

            int time = 0;
            State state1 = new State();
            for (int i = 1; i < temp.Length; i++)
            {
                (State state, int time) result = GetTimeSpend(map, temp[i], temp[i - 1], state1, target);
                state1 = result.state;
                time += result.time;
            }

            if (!state1.torchEquipped) time += 7;

            List<int> distanceSequence = new List<int>();
            foreach (Point point in sequence)
                distanceSequence.Add(distances[point]);

            int ok = distances[target];
            return time;
        }

        private static List<Point> GetNeighbours(Point point, int[,] map)
        {
            List<Point> neighbours = new List<Point>();

            if (point.Equals(new Point(0, 0)))
            {
                neighbours.Add(new Point(1, 0));
                neighbours.Add(new Point(0, 1));
            }
            else if (point.Equals(new Point(map.GetLength(0) - 1, 0)))
            {
                neighbours.Add(new Point(point.x - 1, 0));
                neighbours.Add(new Point(point.x, 1));
            }
            else if (point.Equals(new Point(0, map.GetLength(1) - 1)))
            {
                neighbours.Add(new Point(0, point.y - 1));
                neighbours.Add(new Point(1, point.y));
            }
            else if (point.Equals(new Point(map.GetLength(0) - 1, map.GetLength(1) - 1)))
            {
                neighbours.Add(new Point(point.x, point.y - 1));
                neighbours.Add(new Point(point.x - 1, point.y));
            }
            else if (point.y == 0)
            {
                neighbours.Add(new Point(point.x - 1, 0));
                neighbours.Add(new Point(point.x, 1));
                neighbours.Add(new Point(point.x + 1, 0));
            }
            else if (point.x == 0)
            {
                neighbours.Add(new Point(0, point.y - 1));
                neighbours.Add(new Point(1, point.y));
                neighbours.Add(new Point(0, point.y + 1));
            }
            else if (point.y == map.GetLength(1) - 1)
            {
                neighbours.Add(new Point(point.x - 1, point.y));
                neighbours.Add(new Point(point.x, point.y - 1));
                neighbours.Add(new Point(point.x + 1, point.y));
            }
            else if (point.x == map.GetLength(0) - 1)
            {
                neighbours.Add(new Point(point.x, point.y - 1));
                neighbours.Add(new Point(point.x - 1, point.y));
                neighbours.Add(new Point(point.x, point.y + 1));
            }
            else
            {
                neighbours.Add(new Point(point.x - 1, point.y));
                neighbours.Add(new Point(point.x, point.y - 1));
                neighbours.Add(new Point(point.x + 1, point.y));
                neighbours.Add(new Point(point.x, point.y + 1));
            }

            return neighbours;
        }

        private static (State, int) GetTimeSpend(int[,] map, Point current, Point neighbour, State oldState, Point target)
        {
            int timeSpend = 1;
            State newState = new State()
            {
                torchEquipped = oldState.torchEquipped,
                climbingGearEquipped = oldState.climbingGearEquipped
            };

            // If currently in rocky area and want to go to wet area, if the torch is equipped, gotta change
            // Can't change to neither, but can change to climbing gear
            if (map[current.x, current.y] == 0 && map[neighbour.x, neighbour.y] == 1 && oldState.torchEquipped)
            {
                timeSpend += 6;
                newState = new State()
                {
                    torchEquipped = false,
                    climbingGearEquipped = true
                };
            }
            // If currently in rocky area and want to go to narrow area, if the climbing gear is equipped, gotta change
            // Can't change to neither, but can change to the torch
            else if (map[current.x, current.y] == 0 && map[neighbour.x, neighbour.y] == 2 && oldState.climbingGearEquipped)
            {
                timeSpend += 6;
                newState = new State()
                {
                    torchEquipped = true,
                    climbingGearEquipped = false
                };
            }
            // If currently in wet area and want to go to rocky area, if neither is equipped, gotta change
            // Can't change to torch, but can change to the climbing gear
            else if (map[current.x, current.y] == 1 && map[neighbour.x, neighbour.y] == 0 && !oldState.torchEquipped && !oldState.climbingGearEquipped)
            {
                timeSpend += 6;
                newState = new State()
                {
                    torchEquipped = false,
                    climbingGearEquipped = true
                };
            }
            // If currently in wet area and want to go to narrow area, if the climbing gear is equipped, gotta change
            // Can't change to toch, but can change to the neither
            else if (map[current.x, current.y] == 1 && map[neighbour.x, neighbour.y] == 2 && oldState.climbingGearEquipped)
            {
                timeSpend += 6;
                newState = new State()
                {
                    torchEquipped = false,
                    climbingGearEquipped = false
                };
            }
            // If currently in narrow area and want to go to rocky area, if neither is equipped, gotta change
            // Can't change to climbing gear, but can change to the torch
            else if (map[current.x, current.y] == 2 && map[neighbour.x, neighbour.y] == 0 && !oldState.torchEquipped && !oldState.climbingGearEquipped)
            {
                timeSpend += 6;
                newState = new State()
                {
                    torchEquipped = true,
                    climbingGearEquipped = false
                };
            }
            // If currently in narrow area and want to go to wet area, if the toch is equipped, gotta change
            // Can't change to climbing gear, but can change to the torch
            else if (map[current.x, current.y] == 2 && map[neighbour.x, neighbour.y] == 1 && oldState.torchEquipped)
            {
                timeSpend += 6;
                newState = new State()
                {
                    torchEquipped = false,
                    climbingGearEquipped = false
                };
            }

            if (neighbour.Equals(target) && !oldState.torchEquipped)
            {
                timeSpend += 7;
                newState = new State()
                {
                    torchEquipped = true,
                    climbingGearEquipped = false
                };
            }

            return (newState, timeSpend);
        }

        public class State
        {
            public bool torchEquipped = true;
            public bool climbingGearEquipped = false;
        }

        private static int[,] GenerateMap(int depth, int targetX, int targetY, int paddingX, int paddingY)
        {
            int[,] erosionMap = new int[targetX + 1 + paddingX, targetY + 1 + paddingY];

            for (int y = 0; y < erosionMap.GetLength(1); y++)
            {
                for (int x = 0; x < erosionMap.GetLength(0); x++)
                {
                    int geologicalIndex = 0;
                    if (x == 0 && y == 0)
                    {
                        geologicalIndex = 0;
                    }
                    else if (x == targetX && y == targetY)
                    {
                        geologicalIndex = 0;
                    }
                    else if (y == 0)
                    {
                        geologicalIndex = x * 16807;
                    }
                    else if (x == 0)
                    {
                        geologicalIndex = y * 48271;
                    }
                    else
                    {
                        geologicalIndex = erosionMap[x - 1, y] * erosionMap[x, y - 1];
                    }

                    erosionMap[x, y] = (geologicalIndex + depth) % 20183;
                }
            }

            int[,] typeMap = new int[erosionMap.GetLength(0), erosionMap.GetLength(1)];

            for (int y = 0; y < erosionMap.GetLength(1); y++)
            {
                for (int x = 0; x < erosionMap.GetLength(0); x++)
                {
                    typeMap[x, y] = erosionMap[x, y] % 3;
                }
            }

            return typeMap;
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
                return (this.x + this.y).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null && (other.x == this.x && other.y == this.y);
            }
        }
    }
}
