using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day16
{

    internal class AdventClass
    {
        private bool m_part2 = false;

        public AdventClass(string line, bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            return total;
        }


        public long Calculate()
        {
            long total = 0;

            if (!m_part2)
            {
                total = Calculate1();
            }
            else
            {
                total = Calculate2();
            }

            return total;
        }
    }


    internal class Day16
    {
        List<AdventClass> inputObjects = new List<AdventClass>();

        internal void ProcessInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // READER code here
                    AdventClass av = new AdventClass(line, part2);
                    inputObjects.Add(av);

                }
            }

        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            foreach (var obj in inputObjects)
            {
                total += obj.Calculate();
            }

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            foreach (var obj in inputObjects)
            {
                total += obj.Calculate();
            }

            return total;
        }


        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            ProcessInput(fileName, part2);

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
