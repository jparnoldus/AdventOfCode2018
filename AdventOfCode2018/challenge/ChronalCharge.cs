using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class ChronalCharge
    {
        public static int GetAnswer()
        {
            int input = 8444;

            int[,] map = new int[300, 300];
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    /**Find the fuel cell's rack ID, which is its X coordinate plus 10.
                    Begin with a power level of the rack ID times the Y coordinate.
                    Increase the power level by the value of the grid serial number(your puzzle input).
                    Set the power level to itself multiplied by the rack ID.
                    Keep only the hundreds digit of the power level(so 12345 becomes 3; numbers with no hundreds digit become 0).
                    Subtract 5 from the power level.*/
                    int rackId = x + 10;
                    int powerLevel = rackId * y;
                    powerLevel += input;
                    powerLevel *= rackId;
                    char[] temp = powerLevel.ToString().Reverse().ToArray();
                    powerLevel = int.Parse(temp[2].ToString());
                    powerLevel -= 5;
                    map[x, y] = powerLevel;
                }
            }

            Dictionary<Coor, int> sumMap = new Dictionary<Coor, int>();
            for (int x = 0; x < 298; x++)
            {
                for (int y = 0; y < 298; y++)
                {
                    int sum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            sum += map[x + i, y + j];
                        }
                    }
                    sumMap.Add(new Coor(x, y), sum);
                }
            }

            var answer = sumMap.OrderByDescending(s => s.Value).First();
            return 0;
        }

        public static int GetAnswer2()
        {
            int input = 8444;

            int[,] map = new int[300, 300];
            for (int x = 0; x < 300; x++)
            {
                for (int y = 0; y < 300; y++)
                {
                    /**Find the fuel cell's rack ID, which is its X coordinate plus 10.
                    Begin with a power level of the rack ID times the Y coordinate.
                    Increase the power level by the value of the grid serial number(your puzzle input).
                    Set the power level to itself multiplied by the rack ID.
                    Keep only the hundreds digit of the power level(so 12345 becomes 3; numbers with no hundreds digit become 0).
                    Subtract 5 from the power level.*/
                    int rackId = x + 10;
                    int powerLevel = rackId * y;
                    powerLevel += input;
                    powerLevel *= rackId;
                    char[] temp = powerLevel.ToString().Reverse().ToArray();
                    powerLevel = int.Parse(temp[2].ToString());
                    powerLevel -= 5;
                    map[x, y] = powerLevel;
                }
            }

            Dictionary<CoorE, int> sumMap = new Dictionary<CoorE, int>();
            for (int size = 0; size < 300; size++)
            {
                Console.WriteLine(size);
                for (int x = 0; x < 300-size + 1; x++)
                {
                    for (int y = 0; y < 300-size+1; y++)
                    {
                        int sum = 0;
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                sum += map[x + i, y + j];
                            }
                        }
                        sumMap.Add(new CoorE(x, y, size), sum);
                    }
                }
            }

            var answer = sumMap.OrderByDescending(s => s.Value).First();
            return 0;
        }

        public class Coor
        {
            public int x;
            public int y;

            public Coor(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public class CoorE
        {
            public int x;
            public int y;
            public int size;

            public CoorE(int x, int y, int size)
            {
                this.x = x;
                this.y = y;
                this.size = size;
            }
        }
    }
}
