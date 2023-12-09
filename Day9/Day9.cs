using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day9
{
    internal class Day9
    {
        internal List<long> GetValueList(List<long> inputValues, bool last)
        {
            bool finished = false;
            List<long> currentValues = inputValues;
            List<long> lastValues = new List<long>();
            while (!finished)
            {
                List<long> diffValues = new List<long>();

                if (last)
                {
                    lastValues.Add(currentValues.Last());

                }
                else
                {
                    lastValues.Add(currentValues.First());
                }
                for (int i = 0; i < currentValues.Count - 1; i++)
                {
                    diffValues.Add(currentValues[i + 1] - currentValues[i]);
                }

                if (diffValues.All(x => x == diffValues[0]))
                {
                    lastValues.Add(diffValues[0]);
                    finished = true;
                }
                else
                {
                    currentValues = diffValues;
                }
            }

            return lastValues;
        }

        internal long ExtrapolateValues(List<long> inputValues)
        {
            long total = 0;

            List<long> lastValues = GetValueList(inputValues, true);

            total = lastValues.Sum();
            return total;
        }


        internal long ExtrapolateBackwards(List<long> inputValues)
        {
            long total = 0;

            List<long> lastValues = GetValueList(inputValues, false);

            total = lastValues.Last(); ;
            for (int i = lastValues.Count -2; i >= 0; i--)
            {
                total = lastValues[i] - total;
            }

            return total;
        }

        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    List<long> values = StringLibraries.GetListOfInts(line, ' ');
                    total += ExtrapolateValues(values);
                }
            }

            Console.WriteLine("1) Result is: " + total);
        }

        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    List<long> values = StringLibraries.GetListOfInts(line, ' ');
                    total += ExtrapolateBackwards(values);
                }
            }

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
