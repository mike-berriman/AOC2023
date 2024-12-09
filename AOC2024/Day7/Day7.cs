using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class Calculation
    {
        public long Result { get; set; } = 0;
        public List<long> Values { get; set; } = new List<long>();

        public long Process()
        {
            int operators = 0;

            long operatorCount = Values.Count - 1;
            long options = (long)Math.Pow(2, operatorCount);

            for (int i = 0; i < options; i++)
            {
                long thisTotal = Values[0];

                for (int j = 1; j < Values.Count; j++)
                {
                    if (((operators >> (j-1)) & 1) == 1)
                    {
                        thisTotal += Values[j];
                    }
                    else
                    {
                        thisTotal *= Values[j];
                    }
                }

                if (thisTotal == Result)
                {
                    return Result;
                }

                operators++;
            }

            return 0;
        }

        public void Increment(List<int> values, int index)
        {
            if (index < values.Count)
            {
                if (values[index] < 2)
                {
                    values[index]++;
                }
                else
                {
                    values[index] = 0;
                    Increment(values, index + 1);
                }
            }
        }

        public long Process2()
        {
            long operatorCount = Values.Count - 1;
            long options = (long)Math.Pow(3, operatorCount);

            List<int> operators = new List<int>();
            for(int i = 0; i < operatorCount; i++)
            {
                operators.Add(0);
            }

            for (int i = 0; i < options; i++)
            {
                long thisTotal = Values[0];


                for (int j = 1; j < Values.Count; j++)
                {
                    if (operators[j - 1] == 0)
                    {
                        thisTotal += Values[j];
                    }
                    else if (operators[j - 1] == 1)
                    {
                        thisTotal *= Values[j];
                    }
                    else
                    {
                        string val = thisTotal.ToString() + Values[j].ToString();
                        thisTotal = Convert.ToInt64(val);

                    }
                }

                if (thisTotal == Result)
                {
                    return Result;
                }

                Increment(operators, 0);
            }

            return 0;
        }

    }

    internal class Day7
    {
        private bool m_part2 = false;
        Calculation calcs = new Calculation();

        public Day7(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            return calcs.Process();
        }

        public long Calculate2()
        {
            return calcs.Process2();
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
            string[] splits = line.Split(':');
            calcs.Result = Convert.ToInt64(splits[0]);
            calcs.Values = StringLibraries.GetListOfInts(splits[1], ' ');

        }

    }
}
