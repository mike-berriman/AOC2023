using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class StringLibraries
    {
        public static List<string> GetAllLines(string filename)
        {
            List<string> lines = new List<string>();
            using (StreamReader rdr = new StreamReader(filename))
            {
                string line = string.Empty;

                while ((line = rdr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }


        public static List<string> GetListOfStrings(string data, char delim)
        {
            return data.Split(delim, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).ToList();
        }

        public static List<long> GetListOfInts(string data, char delim)
        {
            return data.Split(delim, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToInt64(x)).ToList();
        }

        public static List<double> GetListOfDoubles(string data, char delim)
        {
            return data.Split(delim, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => Convert.ToDouble(x)).ToList();
        }
    }
}