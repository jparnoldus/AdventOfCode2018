using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AdventOfCode2018.challenge
{
    class ReposeRecord : Challenge
    {
        public static int DoStrategy1()
        {
            var list = GetList();
            list.Sort(SortGuardTimes);
            var shifts = Objectify(list);

            var guardAsleepSum = new Dictionary<int, int>();
            foreach (var shift in shifts)
            {
                if (guardAsleepSum.ContainsKey(shift.guard))
                    guardAsleepSum[shift.guard] += shift.sumAsleep;
                else
                    guardAsleepSum.Add(shift.guard, shift.sumAsleep);
            }

            var sleepyGuard = guardAsleepSum.OrderByDescending(g => g.Value).First().Key;
            var sleepyGuardAsleeps = shifts.Where(s => s.guard == sleepyGuard).SelectMany(s => s.asleep);

            var asleepMinutes = new Dictionary<int, int>();
            foreach (var asleep in sleepyGuardAsleeps)
            {
                for (int i = asleep.start.Minute; i < asleep.end.Minute; i++)
                {
                    if (asleepMinutes.ContainsKey(i))
                        asleepMinutes[i] += 1;
                    else
                        asleepMinutes.Add(i, 1);
                }
            }

            var minute = asleepMinutes.OrderByDescending(m => m.Value).First().Key;

            return minute * sleepyGuard;
        }

        public static List<Shift> Objectify(List<string> list)
        {
            List<Shift> shifts = new List<Shift>();
            Shift newShift = new Shift();
            Asleep newAsleep = new Asleep();

            foreach (string line in list)
            {
                if (line.Contains("begins shift"))
                {
                    if (newShift.guard != 0)
                    {
                        newShift.sumAsleep = newShift.asleep.Sum(a => a.minutes);
                        shifts.Add(newShift);
                    }

                    newShift = new Shift();
                    newShift.guard = int.Parse(line.Substring(line.IndexOf('#') + 1, line.IndexOf(' ', line.IndexOf('#')) - line.IndexOf('#')));
                    DateTime.TryParseExact(line.Substring(1, line.IndexOf(']') - 1), "yyyy-MM-dd HH:mm", null, DateTimeStyles.None, out newShift.start);
                }

                if (line.Contains("falls asleep"))
                {
                    newAsleep = new Asleep();
                    DateTime.TryParseExact(line.Substring(1, line.IndexOf(']') - 1), "yyyy-MM-dd HH:mm", null, DateTimeStyles.None, out newAsleep.start);
                }

                if (line.Contains("wakes up"))
                {
                    DateTime.TryParseExact(line.Substring(1, line.IndexOf(']') - 1), "yyyy-MM-dd HH:mm", null, DateTimeStyles.None, out newAsleep.end);
                    newAsleep.minutes = (newAsleep.end - newAsleep.start).Minutes;
                    newShift.asleep.Add(newAsleep);
                }
            }

            return shifts;
        }

        public static int SortGuardTimes(string x, string y)
        {
            if (x == null | x == "") return -1;
            if (y == null | y == "") return 1;

            var datetime_x = double.Parse(new String(x.Substring(1, x.IndexOf(']') - 1).Where(c => char.IsDigit(c)).ToArray()));
            var datetime_y = double.Parse(new String(y.Substring(1, y.IndexOf(']') - 1).Where(c => char.IsDigit(c)).ToArray()));

            return datetime_x > datetime_y ? 1 : -1;
        }

        private static List<string> GetList()
        {
            List<string> list = new List<string>();

            try
            {
                using (StreamReader sr = new StreamReader(GetPath(4)))
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

    public class Shift
    {
        public int guard;
        public DateTime start;
        public List<Asleep> asleep = new List<Asleep>();
        public int sumAsleep;
    }

    public class Asleep
    {
        public DateTime start;
        public DateTime end;
        public int minutes;
    }
}
