using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class MathLibraries
    {

        public static long LowestCommonMultiple(List<long> lengths)
        {
            if (lengths.Count == 0)
            {
                return 0;
            }

            long lcm = lengths[0];

            for (int i = 0; i < lengths.Count; i++)
            {
                lcm = LowestCommonMultiple(lcm, lengths[i]);
            }

            return lcm;
        }

        public static long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        public static long LowestCommonMultiple(long a, long b)
        {
            return a * b / GreatestCommonDivisor(a, b);
        }

    }
}
