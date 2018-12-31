using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class Four_DimensionalAdventure : Challenge
    {
        public static int CountConstellations()
        {
            List<Star> stars = GetStars();
            List<Constellation> constellations = new List<Constellation>();

            foreach (Star star1 in stars)
            {
                bool found = false;
                foreach (Constellation constellation in constellations)
                {
                    foreach (Star star2 in constellation.stars)
                    {
                        if (Math.Abs(star1.a - star2.a) + Math.Abs(star1.b - star2.b) + Math.Abs(star1.c - star2.c) + Math.Abs(star1.d - star2.d) <= 3)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found)
                    {
                        constellation.stars.Add(star1);
                        break;
                    }
                }

                if (!found)
                {
                    Constellation constellation = new Constellation();
                    constellation.stars.Add(star1);
                    constellations.Add(constellation);
                }
            }

            foreach (Constellation constellation1 in constellations)
            {
                foreach (Constellation constellation2 in constellations)
                {
                    bool connected = false;
                    if (constellation1 != constellation2)
                    {
                        foreach (Star star1 in constellation1.stars)
                        {
                            foreach (Star star2 in constellation2.stars)
                            {
                                if (Math.Abs(star1.a - star2.a) + Math.Abs(star1.b - star2.b) + Math.Abs(star1.c - star2.c) + Math.Abs(star1.d - star2.d) <= 3)
                                {
                                    connected = true;
                                }
                            }
                        }
                    }

                    if (connected)
                    {
                        constellation1.stars.AddRange(constellation2.stars);
                        constellation2.stars.Clear();
                    }
                }
            }

            constellations = constellations.Where(c => c.stars.Count > 0).ToList();

            foreach (Constellation constellation1 in constellations)
            {
                foreach (Constellation constellation2 in constellations)
                {
                    bool connected = false;
                    if (constellation1 != constellation2)
                    {
                        foreach (Star star1 in constellation1.stars)
                        {
                            foreach (Star star2 in constellation2.stars)
                            {
                                if (Math.Abs(star1.a - star2.a) + Math.Abs(star1.b - star2.b) + Math.Abs(star1.c - star2.c) + Math.Abs(star1.d - star2.d) <= 3)
                                {
                                    connected = true;
                                }
                            }
                        }
                    }

                    if (connected)
                    {
                        constellation1.stars.AddRange(constellation2.stars);
                        constellation2.stars.Clear();
                    }
                }
            }

            constellations = constellations.Where(c => c.stars.Count > 0).ToList();

            foreach (Constellation constellation1 in constellations)
            {
                foreach (Constellation constellation2 in constellations)
                {
                    bool connected = false;
                    if (constellation1 != constellation2)
                    {
                        foreach (Star star1 in constellation1.stars)
                        {
                            foreach (Star star2 in constellation2.stars)
                            {
                                if (Math.Abs(star1.a - star2.a) + Math.Abs(star1.b - star2.b) + Math.Abs(star1.c - star2.c) + Math.Abs(star1.d - star2.d) <= 3)
                                {
                                    connected = true;
                                }
                            }
                        }
                    }

                    if (connected)
                    {
                        constellation1.stars.AddRange(constellation2.stars);
                        constellation2.stars.Clear();
                    }
                }
            }

            constellations = constellations.Where(c => c.stars.Count > 0).ToList();

            return constellations.Count;
        }

        private static List<Star> GetStars()
        {
            List<Star> stars = new List<Star>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(25)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] coordinatesString = line.Split(',');
                        string[] coordinatesTrimmed = coordinatesString.Select(s => s.Trim(' ')).ToArray();
                        int[] coordinates = coordinatesTrimmed.Select(s => int.Parse(s)).ToArray();
                        stars.Add(new Star(coordinates[0], coordinates[1], coordinates[2], coordinates[3]));
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
            public int a;
            public int b;
            public int c;
            public int d;

            public Star(int a, int b, int c, int d)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
            }
        }

        public class Constellation
        {
            public List<Star> stars = new List<Star>();
        }
    }
}
