using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using AOCShared;

namespace Day14
{
    internal class AdventClass
    {
        public AdventClass(List<string> allLines, bool part2)
        {
        }

        public List<string> RotateLines(List<string> lines)
        {
            List<string> rotatedLines = new List<string>();

            for (int i = 0; i < lines.Count; i++)
            {
                rotatedLines.Add(new string(lines.Select(x => x[i]).ToArray()));
            }

            return rotatedLines;
        }

        public List<string> RotateLinesOtherWay(List<string> lines)
        {
            List<string> rotatedLines = new List<string>();

            for (int i = lines.Count-1; i >= 0; i--)
            {
                rotatedLines.Add(new string(lines.Select(x => x[i]).ToArray()));
            }

            return rotatedLines;
        }

        public long CalculateResult(List<string> lines)
        {
            long total = 0;

            foreach (string line in lines)
            {
                long length = line.Length;

                for (int i = 0; i < length; i++)
                {
                    if (line[i] == 'O')
                    {
                        total += length - i;
                    }
                }
            }


            return total;
        }

        public List<string> MoveRocks(List<string> lines)
        {
            List<string> newlines = new List<string>();
            long total = 0;

            foreach (string line in lines)
            {
                string[] splits = line.Split('#');
                StringBuilder result = new StringBuilder();

                int splitCount = 0;
                foreach (string split in splits)
                {
                    if (split.Length > 0)
                    {
                        int count = split.Count(x => x == 'O');

                        if (count > 0)
                        {
                            result.Append('O', count);
                        }

                        if (split.Length-count > 0)
                        {
                            result.Append('.', split.Length - count);
                        }
                    }

                    if (++splitCount < splits.Length)
                    {
                        result.Append('#');
                    }
                }

                newlines.Add(result.ToString());
            }

            return newlines;
        }

    }


    internal class Day14
    {
        internal long Execute1(string fileName)
        {
            List<string> allLines = StringLibraries.GetAllLines(fileName);

            AdventClass calculator = new AdventClass(allLines, false);

            List<string> rotatedLines = calculator.RotateLines(allLines);
            List<string> newLines = calculator.MoveRocks(rotatedLines);

            long total = calculator.CalculateResult(newLines);

            return total;
        }

        public (long start, long length) FindCycle(List<long> values)
        {
            for (long i = 0; i < values.Count; i++)
            {
                long testStart = values[(int)i];

                // Find another of this value
                long nextValue = values.IndexOf(testStart, (int)(i + 1));
                while (nextValue > 0)
                {
                    long diff = nextValue - i;

                    if (diff < 2)
                    {
                        nextValue = values.IndexOf(testStart, (int)(nextValue + 1));
                        continue;
                    }

                    bool foundCycle = true;
                    for (long j = 0; j < diff; j++)
                    {
                        if ((nextValue + j) > values.Count-1)
                        {
                            foundCycle = false;
                            break;
                        }
                        if (values[(int)(i+j)] != values[(int)(nextValue+j)])
                        {
                            foundCycle = false;
                            break;
                        }
                    }

                    if (foundCycle)
                    {
                        return (i, diff);
                    }

                    nextValue = values.IndexOf(testStart, (int)(nextValue + 1));
                }
            }

            return (-1, -1);
        }

        internal long Execute2(string fileName)
        {
            List<string> allLines = StringLibraries.GetAllLines(fileName);

            AdventClass calculator = new AdventClass(allLines, false);

            List<string> lines = calculator.RotateLines(allLines);

            long total = 0;
            List<string> rotatedLines = null;
            List<long> results = new List<long>();
            for (int i = 0; i < 1000000000; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    rotatedLines = calculator.MoveRocks(lines);
                    lines = calculator.RotateLinesOtherWay(rotatedLines);
                }

                long test = calculator.CalculateResult(lines);
                results.Add(test);

                if ((i % 100) == 0)
                {
                    var cycle = FindCycle(results);

                    if (cycle.start >= 0)
                    {
                        long iteration = (1000000000 - cycle.start);
                        long index = iteration % cycle.length;

                        total = results[(int)(cycle.start + index - 1)];
                        break;
                    }
                }

            }

            return total;
        }

        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            long total;
            if (!part2)
            {
                total = Execute1(fileName);
            }
            else
            {
                total = Execute2(fileName);
            }

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(counter + ") " + "(" + millis + ") Result is: " + total);

        }

    }
}
