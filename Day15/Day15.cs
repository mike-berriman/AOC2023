using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day15
{

    internal class AdventClass
    {
        public AdventClass(string line, bool part2)
        {
        }

        public long Calculate()
        {
            long total = 0;

            return total;
        }
    }


    internal class Day15
    {
        List<AdventClass> inputObjects = new List<AdventClass>();

        internal void ProcessInput(string fileName)
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

        internal long Execute1(string fileName)
        {
            long total = 0;

            ProcessInput(fileName);

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            ProcessInput(fileName);

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
