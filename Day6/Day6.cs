using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day6
{
    internal class Day6
    {
        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                }
            }

            Console.WriteLine("1) Result is: " + total);
        }






        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                }
            }

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
