using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2018.challenge
{
    class SubterraneanSustainability : Challenge
    {
        public static int GetPlantPopulation(int generation)
        {
            (string state, Dictionary<string, string> instructions) input = GetInput();
            string lastGeneration = input.state;

            int offset = 0;

            int previous = 0;
            for (int i = 0; i < lastGeneration.Length; i++)
            {
                if (lastGeneration[i] == '#')
                {
                    previous += i - offset;
                }
            }

            for (int i = 0; i < generation; i++)
            {
                string newState = "";
                string lastState = lastGeneration;

                int temp = 4 - lastState.IndexOf('#');
                for (int j = 0; j < temp; j++)
                {
                    lastState = "." + lastState;
                    offset++;
                }

                temp = lastState.LastIndexOf('#');
                int temp2 = lastState.Length;
                for (int j = 0; j < temp - (temp2 - 5); j++)
                {
                    lastState = string.Concat(lastState, ".");
                }

                for (int j = 2; j < lastState.Length - 2; j++)
                {
                    string localState = lastState.Substring(j - 2, 5);
                    if (input.instructions.ContainsKey(localState))
                        newState = string.Concat(newState, input.instructions[localState]);
                    else
                        newState = string.Concat(newState, lastState[j]);
                }

                offset -= 2;
                int current = 0;
                for (int c = 0; c < lastGeneration.Length; c++)
                {
                    if (lastGeneration[c] == '#')
                    {
                        current += c - offset;
                    }
                }

                Console.WriteLine(string.Format("Gen {0}:: Current: {1}, difference: {2}", i, current, current - previous));
                previous = current;
                lastGeneration = newState;
            }

            int answer = 0;
            for (int i = 0; i < lastGeneration.Length; i++)
            {
                if (lastGeneration[i] == '#')
                {
                    answer += i - offset;
                }
            }

            return answer;
        }

        private static (string state, Dictionary<string, string> instructions) GetInput()
        {
            string state = "";
            Dictionary<string, string> instructions = new Dictionary<string, string>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(12)))
                {
                    string line1 = sr.ReadLine();
                    state = line1.Substring(line1.IndexOf(' ', 10) + 1);

                    //empty line
                    sr.ReadLine();

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        instructions.Add(line.Substring(0, 5), line.Last().ToString());
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return (state, instructions);
        }
    }
}
