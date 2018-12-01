using System;
using System.IO;

namespace AdventOfCode2018.Challenge
{
    public class Challenge
    {
        protected static string GetPath(int day)
        {
            return String.Format("{0}/day{1}.txt", Program.INPUT_FILE_DIR, day);
        }
    }
}
