using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day25
{
    internal class Day25
    {
        internal void ProcessInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            StreamWriter writer = new StreamWriter("D:\\temp\\wires.dot");
            writer.WriteLine("graph {");
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    List<string> parts = StringLibraries.GetListOfStrings(line, ':');

                    List<string> connectedParts = StringLibraries.GetListOfStrings(parts[1], ' ');
                    foreach (string part in connectedParts)
                    {
                        writer.WriteLine($"    {parts[0]} -- {part}");
                    }
                }
            }
            writer.WriteLine("}");
            writer.Close();

            // Now used Gephi to find connections to break to form two parts
            // and count each part.
        }

        internal long Execute1(string fileName)
        {
            long total = 1;

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

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
