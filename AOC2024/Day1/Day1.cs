using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day1
    {
        private bool m_part2 = false;
        List<long> list1 = new List<long>();
        List<long> list2 = new List<long>();

        public Day1(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;
            
            list1.Sort();
            list2.Sort();

            for (int i = 0; i < list1.Count; i++)
            {
                total += Math.Abs(list2[i] - list1[i]);
            }

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            Dictionary<long, long> list2Vals = new Dictionary<long, long>();
            foreach (long val in list2)
            {
                if (list2Vals.ContainsKey(val))
                {
                    list2Vals[val]++;
                }
                else
                {
                    list2Vals[val] = 1;
                }
            }

            foreach (long val in list1)
            {
                if (list2Vals.ContainsKey(val))
                {
                    total += val * list2Vals[val];
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
                    List<long> vals = AOCShared.StringLibraries.GetListOfInts(line, ' ');
                    list1.Add(vals[0]);
                    list2.Add(vals[1]);
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
