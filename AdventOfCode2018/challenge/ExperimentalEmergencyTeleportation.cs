using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class ExperimentalEmergencyTeleportation : Challenge
    {
        public static int GetInRange()
        {
            List<NanoBot> nanobots = GetNanobots();
            List<NanoBot> inRange = nanobots.OrderByDescending(n => n.radius).First().GetInRange(nanobots);

            return inRange.Count;
        }

        public static int GetIdealPoint()
        {
            List<NanoBot> P = GetNanobots();

            Dictionary<NanoBot, List<NanoBot>> lookupFriends = new Dictionary<NanoBot, List<NanoBot>>();
            foreach (NanoBot nanobot in P)
            {
                lookupFriends.Add(nanobot, nanobot.GetInRange(P));
            }

            lookupFriends.OrderByDescending(n => n.Value.Count);

            List<List<NanoBot>> cliques = BronKerbosch(new List<NanoBot>(), P, new List<NanoBot>(), lookupFriends, new List<List<NanoBot>>());
            var ok = cliques.OrderByDescending(c => c.Count);

            var biggest = ok.First();

            var furthest = biggest.OrderByDescending(n => Math.Abs(n.position.x) + Math.Abs(n.position.y) + Math.Abs(n.position.z)).First();

            var answer = Math.Abs(furthest.position.x) + Math.Abs(furthest.position.y) + Math.Abs(furthest.position.z) - furthest.radius;

            return answer;
        }

        public static List<List<NanoBot>> BronKerbosch(IEnumerable<NanoBot> R, IEnumerable<NanoBot> P, IEnumerable<NanoBot> X, Dictionary<NanoBot, List<NanoBot>> friends, List<List<NanoBot>> cliques)
        {
            if (P.Count() == 0 && X.Count() == 0 && R.Count() > 0)
            {
                cliques.Add(R.ToList());
                Console.WriteLine("added clique of size " + R.ToList().Count);
                return cliques;
            }
            else
            {
                //Console.WriteLine(string.Format("{0}, {1}, {2}", R.Count(), P.Count(), X.Count()));
                List<NanoBot> botsCopy = P.Select(n => n.Clone()).ToList();
                foreach (NanoBot nanobot in botsCopy)
                {
                    List<NanoBot> vGraph = new List<NanoBot>();
                    vGraph.Add(nanobot);

                    List<NanoBot> neighbourGraph = new List<NanoBot>();
                    neighbourGraph.AddRange(friends[nanobot]);

                    P = P.Except(vGraph);

                    BronKerbosch(
                        R.Union(vGraph),
                        P.Intersect(neighbourGraph),
                        X.Intersect(neighbourGraph),
                        friends,
                        cliques);

                    X = X.Union(vGraph);
                }

                return cliques;
            }
        }

        private static List<NanoBot> GetNanobots()
        {
            List<NanoBot> nanobots = new List<NanoBot>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(23)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        nanobots.Add(NanoBot.Parse(line));
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return nanobots;
        }

        public class NanoBot
        {
            public Point position;
            public int radius;

            public NanoBot(Point position, int radius)
            {
                this.position = position;
                this.radius = radius;
            }

            public List<NanoBot> GetInRange(List<NanoBot> nanobots)
            {
                List<NanoBot> inRange = new List<NanoBot>();

                foreach (NanoBot nanobot in nanobots)
                {
                    if (!nanobot.Equals(this))
                    {
                        int distance = Math.Abs(this.position.x - nanobot.position.x) + Math.Abs(this.position.y - nanobot.position.y) + Math.Abs(this.position.z - nanobot.position.z);
                        if (distance < this.radius)
                        {
                            inRange.Add(nanobot);
                        }
                    }
                }

                return inRange;
            }

            public NanoBot Clone()
            {
                return new NanoBot(this.position.Clone(), this.radius);
            }

            public static NanoBot Parse(string line)
            {
                string positionLine = line.Substring(line.IndexOf('<') + 1, line.LastIndexOf('>') - (line.IndexOf('<') + 1));
                return new NanoBot(Point.Parse(positionLine), int.Parse(line.Substring(line.LastIndexOf('=') + 1)));
            }

            public override int GetHashCode()
            {
                return (this.position.GetHashCode() + this.radius).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                NanoBot other = obj as NanoBot;
                return other != null && (other.position.Equals(this.position) && other.radius == this.radius);
            }
        }

        public class Point
        {
            public int x;
            public int y;
            public int z;

            public Point(int x, int y, int z)
            {
                this.x = x;
                this.y = y;
                this.z = z;
            }

            public Point Clone()
            {
                return new Point(this.x, this.y, this.z);
            }

            public override int GetHashCode()
            {
                return (this.x + this.y + this.z).GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Point other = obj as Point;
                return other != null && (other.x == this.x && other.y == this.y && other.z == this.z);
            }

            public static Point Parse(string line)
            {
                int[] words = line.Split(',').Select(w => int.Parse(w)).ToArray();
                return new Point(words[0], words[1], words[2]);
            }
        }
    }
}
