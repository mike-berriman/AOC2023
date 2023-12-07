using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    public class Race
    {
        public long TotalTime { get; set; }
        public long Record { get; set; }

        public long Dist(int millisHeld)
        {
            long speed = millisHeld;
            long dist = (TotalTime - millisHeld) * speed;

            return dist;
        }

        public long CalculateValue()
        {
            long total = 0;
            for (int i = 0; i < TotalTime; i++)
            {
                long dist = Dist(i);
                if (dist > Record)
                {
                    total ++;
                }
            }

            return total;
        }
    }

    internal class Day6
    {
        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            line = rdr.ReadLine();
            long[] raceTimes = line.Substring(5).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();

            line = rdr.ReadLine();
            long[] distances = line.Substring(9).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();

            List<Race> races = new List<Race>();
            for (int i = 0; i < raceTimes.Length; i++)
            {
                Race r = new Race();
                r.TotalTime = raceTimes[i];
                r.Record = distances[i];
                races.Add(r);
            }

            total = 1;
            foreach (Race r in races)
            {
                total *= r.CalculateValue();
            }

            Console.WriteLine("1) Result is: " + total);
        }

        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            line = rdr.ReadLine();
            line = line.Replace(" ", "");
            long[] raceTimes = line.Substring(5).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();

            line = rdr.ReadLine();
            line = line.Replace(" ", "");
            long[] distances = line.Substring(9).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();

            List<Race> races = new List<Race>();
            for (int i = 0; i < raceTimes.Length; i++)
            {
                Race r = new Race();
                r.TotalTime = raceTimes[i];
                r.Record = distances[i];
                races.Add(r);
            }

            total = 1;
            foreach (Race r in races)
            {
                total *= r.CalculateValue();
            }

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
