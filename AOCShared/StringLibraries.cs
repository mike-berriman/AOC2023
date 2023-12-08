using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class StringLibraries
    {
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