using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day2
    {
        private bool m_part2 = false;
        List<long> rawData = new List<long>();

        public Day2(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 1;

            if (isSafe(rawData).Count > 0)
            {
                total = 0;
            }

            return total;
        }

        public List<int> isSafe(List<long> thisData)
        {
            List<int> errorIndexes = new List<int>();
            bool isIncreasing = false;
            bool isDecreasing = false;

            for (int i = 1; i < thisData.Count; i++)
            {
                if ((thisData[i - 1] == thisData[i]) || (Math.Abs(thisData[i - 1] - thisData[i]) > 3))
                {
                    errorIndexes.Add(i);
                    continue;
                }

                if (thisData[i - 1] < thisData[i])
                {
                    if (isDecreasing)
                    {
                        errorIndexes.Add(i);
                        continue;
                    }
                    isIncreasing = true;
                }

                if (thisData[i - 1] > thisData[i])
                {
                    if (isIncreasing)
                    {
                        errorIndexes.Add(i);
                        continue;
                    }
                    isDecreasing = true;
                }
            }

            return errorIndexes;
        }

        public long Calculate2()
        {
            int total = 0;
            List<int> errorIndexes = isSafe(rawData);

            if (errorIndexes.Count == 0)
            {
                total = 1;
            }
            else
            {
                for (int idx = 0; idx < rawData.Count; idx++)
                {
                    List<long> newData = new List<long>(rawData);
                    newData.RemoveAt(idx);

                    if (isSafe(newData).Count == 0)
                    {
                        total = 1;
                        break;
                    }
                }
            }

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {
            rawData = StringLibraries.GetListOfInts(line, ' ');
        }

    }
}
