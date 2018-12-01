using System;
using System.Collections.Generic;
using System.IO;

namespace AdventOfCode2018.Challenge
{
    class ChronalCalibration : Challenge
    {
        public static int CalibrateChronal(bool findRepeat)
        {
            int answer = 0;
            HashSet<int> frequencies = new HashSet<int>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(1)))
                {
                    while (!sr.EndOfStream)
                    {
                        String line = sr.ReadLine();
                        int change = ParseFrequencyChange(line.Substring(1));

                        answer = line.StartsWith("+") ? answer + change : answer - change;

                        if (!frequencies.Add(answer) && findRepeat)
                        {
                            break;
                        }
                        else if (sr.EndOfStream && findRepeat)
                        {
                            sr.DiscardBufferedData();
                            sr.BaseStream.Seek(0, SeekOrigin.Begin);
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

        private static int ParseFrequencyChange(string supposedFrequencyChange)
        {
            if (int.TryParse(supposedFrequencyChange, out int frequencyChange))
            {
                return frequencyChange;
            }
            else
            {
                throw new Exception(string.Format("Frequency change could not be read: {0}", supposedFrequencyChange));
            }
        }
    }
}