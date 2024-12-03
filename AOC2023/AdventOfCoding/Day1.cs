using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    internal class Day1
    {
        string fileName = @"D:\temp\advent\AdventOfCoding\AdventOfCoding\InputData.txt";
        string fileName2 = @"D:\temp\advent\AdventOfCoding\AdventOfCoding\InputData2.txt";

        internal void Execute1()
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                char firstValue = line.FirstOrDefault(x => char.IsDigit(x));
                char lastValue = line.LastOrDefault(x => char.IsDigit(x));

                int num = ((firstValue-'0') * 10) + (lastValue-'0');
                total += num;
            }

            Console.WriteLine("1) Coordinate is: " + total);
        }

        internal void Execute2()
        {
            StreamReader rdr = new StreamReader(fileName2);
            string line = string.Empty;

            string[] searchStrings = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "zero" };
            int[] searchValues = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                int first = line.Length + 1;
                int firstIndex = -1;
                int last = -1;
                int lastIndex = -1;

                for(int i = 0; i < searchStrings.Length; i++)
                {
                    int index = line.IndexOf(searchStrings[i]);
                    if (index >= 0)
                    {
                        if (index < first)
                        {
                            first = index;
                            firstIndex = i;
                        }
                        if (index > last)
                        {
                            last = index;
                            lastIndex = i;
                        }
                    }

                    index = line.LastIndexOf(searchStrings[i]);
                    if (index >= 0)
                    {
                        if (index < first)
                        {
                            first = index;
                            firstIndex = i;
                        }
                        if (index > last)
                        {
                            last = index;
                            lastIndex = i;
                        }
                    }
                }

                int firstValue = searchValues[firstIndex];
                int lastValue = searchValues[lastIndex];

                int num = (firstValue * 10) + lastValue;
                total += num;
            }

            Console.WriteLine("2) Coordinate is: " + total);
        }


    }
}
