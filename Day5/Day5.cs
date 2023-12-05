using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day5
{
    public class Range
    {
        public long DestStart;
        public long SourceStart;
        public long Increment;

        public Range(long destStart, long sourceStart, long increment)
        {
            DestStart = destStart;
            SourceStart = sourceStart;
            Increment = increment;

        }
    }

    public class Map
    {
        public string Name { get; set; } = string.Empty;
        public List<Range> Ranges { get; set; } = new List<Range>();

        public long FindValue(long seed)
        {
            foreach (Range rng in Ranges)
            {
                if ((seed >= rng.SourceStart) && (seed < rng.SourceStart + rng.Increment))
                {
                    return rng.DestStart + (seed - rng.SourceStart);

                }
            }

            return seed;
        }

        public Map()
        {

        }
    }

    internal class Day5
    {

        internal void Execute1(string fileName)
        {
            List<Map> maps = new List<Map>();
            List<long> seeds = new List<long>();

            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;
            Map currMap = new Map();

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("seeds:"))
                    {
                        seeds.AddRange(line.Substring(6).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)));
                    }
                    else if (line.Trim().EndsWith(':'))
                    {
                        Map map = new Map();
                        map.Name = line;
                        maps.Add(map);
                        currMap = map;
                    }
                    else
                    {
                        long[] splits = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();
                        Range rng = new Range(splits[0], splits[1], splits[2]);
                        currMap.Ranges.Add(rng);
                    }
                }
            }

            total = 999999999999999;
            foreach (long seedin in seeds)
            {
                long seed = seedin;

                foreach (Map map in maps)
                {
                    seed = map.FindValue(seed);
                }

                total = Math.Min(seed, total);
            }


            Console.WriteLine("1) Result is: " + total);
        }

        internal void Execute2(string fileName)
        {
            List<Map> maps = new List<Map>();
            List<Range> seeds = new List<Range>();

            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;
            Map currMap = new Map();

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.StartsWith("seeds:"))
                    {
                        long[] splits = line.Substring(6).Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();

                        for (int i = 0; i < splits.Length; i += 2)
                        {
                            Range rng = new Range(0, splits[i], splits[i+1]);
                            seeds.Add(rng);
                        }
                    }
                    else if (line.Trim().EndsWith(':'))
                    {
                        Map map = new Map();
                        map.Name = line;
                        maps.Add(map);
                        currMap = map;
                    }
                    else
                    {
                        long[] splits = line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToArray();
                        Range rng = new Range(splits[0], splits[1], splits[2]);
                        currMap.Ranges.Add(rng);
                    }
                }
            }

            DateTime start = DateTime.Now;

            ConcurrentBag<long> bag = new ConcurrentBag<long>();
            Parallel.ForEach(seeds, seedin =>
            {
                long min = 999999999999999;
                for (int i = 0; i < seedin.Increment; i++)
                {
                    long seed = seedin.SourceStart + i;

                    foreach (Map map in maps)
                    {
                        seed = map.FindValue(seed);
                    }

                    min = Math.Min(seed, min);
                }

                Console.Write(".");
                bag.Add(min);
            });


            Console.WriteLine(".");
            Console.WriteLine("1) Result is: " + bag.Min());
            Console.WriteLine("Time: " + (DateTime.Now - start).TotalMilliseconds);
        }

    }
}
