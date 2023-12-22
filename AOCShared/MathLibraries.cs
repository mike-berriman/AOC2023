using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class Coordinate3
    {
        public long X;
        public long Y;
        public long Z;

        public Coordinate3()
        {

        }

        public Coordinate3(Coordinate3 rhs)
        {
            X = rhs.X;
            Y = rhs.Y;
            Z = rhs.Z;
        }

        public Coordinate3(string input)
        {
            List<long> coords = StringLibraries.GetListOfInts(input, ',');
            X = coords[0];
            Y = coords[1];
            Z = coords[2];
        }
    }


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

        public static long PolylineSquarePerimeter(List<Coordinate> coords)
        {
            double length = 0;
            for (int i = 1; i < coords.Count; i++)
            {
                length += Math.Abs(coords[i].X - coords[i - 1].X);
                length += Math.Abs(coords[i].Y - coords[i - 1].Y);
            }

            return (long)length;
        }

        public static long PolylineArea(List<Coordinate> coords, bool includePerimeter)
        {
            long num4 = 0;
            long num5 = 0;
            foreach (Coordinate item in coords)
            {
                num4 += item.X;
                num5 += item.Y;
            }

            num4 /= (long)coords.Count;
            num5 /= (long)coords.Count;
            long num6 = 0;
            long num7 = 0;
            for (int j = 0; j < coords.Count - 1; j++)
            {
                num6 += (coords[j].X - num4) * (coords[j + 1].Y - num5);
                num7 += (coords[j].Y - num5) * (coords[j + 1].X - num4);
            }

            num6 += (coords[coords.Count - 1].X - num4) * (coords[0].Y - num5);
            num7 += (coords[coords.Count - 1].Y - num5) * (coords[0].X - num4);
            long total = (long)((num7 - num6) / 2.0);

            if (includePerimeter)
            {
                total += (PolylineSquarePerimeter(coords)/2) + 1;
            }

            return total;
        }
    }
}
