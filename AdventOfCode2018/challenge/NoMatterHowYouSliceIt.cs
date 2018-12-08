using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class NoMatterHowYouSliceIt : Challenge
    {
        public static int GetOverlap()
        {
            int answer = 0;
            int[,] fabric = new int[1000, 1000];

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(3)))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        var margins = line.Substring(line.IndexOf('@') + 2, line.IndexOf(':') - line.IndexOf('@') - 2).Split(',').Select(m => int.Parse(m)).ToArray();
                        var size = line.Substring(line.IndexOf(':') + 2).Split('x').Select(s => int.Parse(s)).ToArray();

                        for (int i = margins[0]; i < margins[0] + size[0]; i++)
                        {
                            for (int j = margins[1]; j < margins[1] + size[1]; j++)
                            {
                                fabric.SetValue((int)fabric.GetValue(i, j) + 1, new int[] { i, j });
                            }
                        }
                    }

                    for (int i = 0; i < 1000; i++)
                    {
                        for (int j = 0; j < 1000; j++)
                        {
                            if ((int)fabric.GetValue(i, j) > 1)
                            {
                                answer++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return answer;
        }

        public static int GetNotOverlapped()
        {
            // It was early, ok?
            int[,] fabric = new int[1000, 1000];
            var list = GetList();

            foreach (var line in list)
            {
                var index = int.Parse(line.Substring(1, line.IndexOf('@') - 2));
                var margins = line.Substring(line.IndexOf('@') + 2, line.IndexOf(':') - line.IndexOf('@') - 2).Split(',').Select(m => int.Parse(m)).ToArray();
                var size = line.Substring(line.IndexOf(':') + 2).Split('x').Select(s => int.Parse(s)).ToArray();

                for (int i = margins[0]; i < margins[0] + size[0]; i++)
                {
                    for (int j = margins[1]; j < margins[1] + size[1]; j++)
                    {
                        if ((int)fabric.GetValue(i, j) == 0)
                        {
                            fabric.SetValue(index, new int[] { i, j });
                        }
                        else
                        {
                            fabric.SetValue(-1, new int[] { i, j });
                        }
                    }
                }
            }

            foreach (var line in list)
            {
                var index = int.Parse(line.Substring(1, line.IndexOf('@') - 2));
                var margins = line.Substring(line.IndexOf('@') + 2, line.IndexOf(':') - line.IndexOf('@') - 2).Split(',').Select(m => int.Parse(m)).ToArray();
                var size = line.Substring(line.IndexOf(':') + 2).Split('x').Select(s => int.Parse(s)).ToArray();

                bool notThisOne = false;
                for (int i = margins[0]; i < margins[0] + size[0]; i++)
                {
                    for (int j = margins[1]; j < margins[1] + size[1]; j++)
                    {
                        if ((int)fabric.GetValue(i, j) != index)
                        {
                            notThisOne = true;
                            break;
                        }
                    }

                    if (notThisOne) break;
                }

                if (!notThisOne) return index;
            }

            return 0;
        }

        private static List<string> GetList()
        {
            List<string> list = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(3)))
                {
                    while (!sr.EndOfStream)
                    {
                        list.Add(sr.ReadLine());
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
}
