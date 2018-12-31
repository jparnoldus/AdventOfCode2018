using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class ARegularMap : Challenge
    {
        public static int GetFurthestPathSize()
        {
            string directions = GetDirections();

            List<Room> rooms = new List<Room>();
            Room room = new Room();
            Stack<Room> crossRoads = new Stack<Room>();
            GetRooms(directions, room, rooms, crossRoads);

            var result = BreadthFirstTopDownTraversal(room);
            return result.Max(r => r.depth);
        }

        public static int CountPathsOfSize(int size)
        {
            string directions = GetDirections();

            List<Room> rooms = new List<Room>();
            Room room = new Room();
            Stack<Room> crossRoads = new Stack<Room>();
            GetRooms(directions, room, rooms, crossRoads);

            var result = BreadthFirstTopDownTraversal(room);
            return result.Count(r => r.depth > size);
        }

        public static IEnumerable<Room> BreadthFirstTopDownTraversal(Room room)
        {
            Queue<Room> traverseOrder = new Queue<Room>();

            Queue<Room> Q = new Queue<Room>();
            HashSet<Room> S = new HashSet<Room>();
            Q.Enqueue(room);
            S.Add(room);
            room.depth = 0;
            
            while (Q.Count > 0)
            {
                Room e = Q.Dequeue();
                traverseOrder.Enqueue(e);

                foreach (Room emp in e.GetChildren())
                {
                    emp.depth = e.depth + 1;
                    if (!S.Contains(emp))
                    {
                        Q.Enqueue(emp);
                        S.Add(emp);
                    }
                }
            }

            return traverseOrder;
        }

        private static string GetDirections()
        {
            string directions = null;

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(20)))
                {
                    while (!sr.EndOfStream)
                    {
                        directions = sr.ReadLine().Trim(new char[] { '^', '$' });
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return directions;
        }

        private static string GetRooms(string line, Room currentRoom, List<Room> rooms, Stack<Room> crossRoads)
        {
            Stack<Room> added = new Stack<Room>();
            string passed = "U";
            while (line.Any())
            {
                Room room = new Room();
                room.depth = currentRoom.depth + 1;
                char first = line.First();

                switch (first)
                {
                    case 'N':
                        line = string.Concat(line.Skip(1));
                        if (passed.Last() != 'S')
                        {
                            rooms.Add(room);
                            added.Push(room);
                            currentRoom.north = room;
                            currentRoom = room;
                            passed += first;
                        }
                        else
                        {
                            if (added.Count > 0)
                            {
                                currentRoom = added.Pop();
                                passed = String.Concat(passed.Take(passed.Length - 1));
                                if (added.Count() == 0)
                                    return line;
                            }
                            else
                            {
                                return line;
                            }
                        }
                        break;
                    case 'S':
                        line = string.Concat(line.Skip(1));
                        if (passed.Last() != 'N')
                        {
                            rooms.Add(room);
                            added.Push(room);
                            currentRoom.south = room;
                            currentRoom = room;
                            passed += first;
                        }
                        else
                        {
                            if (added.Count > 0)
                            {
                                currentRoom = added.Pop();
                                passed = String.Concat(passed.Take(passed.Length - 1));
                                if (added.Count() == 0)
                                    return line;
                            }
                            else
                            {
                                return line;
                            }
                        }
                        break;
                    case 'W':
                        line = string.Concat(line.Skip(1));
                        if (passed.Last() != 'E')
                        {
                            rooms.Add(room);
                            added.Push(room);
                            currentRoom.west = room;
                            currentRoom = room;
                            passed += first;
                        }
                        else
                        {
                            if (added.Count > 0)
                            {
                                currentRoom = added.Pop();
                                passed = String.Concat(passed.Take(passed.Length - 1));
                                if (added.Count() == 0)
                                    return line;
                            }
                            else
                            {
                                return line;
                            }
                        }
                        break;
                    case 'E':
                        line = string.Concat(line.Skip(1));
                        if (passed.Last() != 'W')
                        {
                            rooms.Add(room);
                            added.Push(room);
                            currentRoom.east = room;
                            currentRoom = room;
                            passed += first;
                        }
                        else
                        {
                            if (added.Count > 0)
                            {
                                currentRoom = added.Pop();
                                passed = String.Concat(passed.Take(passed.Length - 1));
                                if (added.Count() == 0)
                                    return line;
                            }
                            else
                            {
                                return line;
                            }
                        }
                        break;
                    case '(':
                        line = string.Concat(line.Skip(1));
                        crossRoads.Push(currentRoom);
                        line = GetRooms(line, currentRoom, rooms, crossRoads);
                        break;
                    case '|':
                        line = string.Concat(line.Skip(1));
                        line = GetRooms(line, crossRoads.Peek(), rooms, crossRoads);
                        break;
                    case ')':
                        currentRoom = crossRoads.Pop();
                        line = string.Concat(line.Skip(1));
                        return line;
                }
                
            }

            return line;
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

        public class Room
        {
            public Room north;
            public Room south;
            public Room west;
            public Room east;

            public int depth;

            public List<Room> GetChildren()
            {
                List<Room> children = new List<Room>();
                if (this.north != null)
                    children.Add(this.north);
                if (this.south != null)
                    children.Add(this.south);
                if (this.west != null)
                    children.Add(this.west);
                if (this.east != null)
                    children.Add(this.east);
                return children;
            }
        }
    }
}
