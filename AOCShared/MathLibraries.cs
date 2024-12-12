using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class BoundingBox3
    {
        public Coordinate3 Min = new Coordinate3();
        public Coordinate3 Max = new Coordinate3();

        public bool isInside(Coordinate3 coordinate)
        {
            if ((coordinate.X >= Min.X) && (coordinate.X <= Max.X) &&
                    (coordinate.Y >= Min.Y) && (coordinate.Y <= Max.Y) &&
                    (coordinate.Z >= Min.Z) && (coordinate.Z <= Max.Z))
            {
                return true;
            }

            return false;
        }

        public void Create(List<Coordinate3> coords)
        {
            foreach (Coordinate3 coordinate in coords)
            {
                if (coordinate.X < Min.X)
                {
                    Min.X = coordinate.X;
                }
                if (coordinate.Y < Min.Y)
                {
                    Min.Y = coordinate.Y;
                }
                if (coordinate.Z < Min.Z)
                {
                    Min.Z = coordinate.Z;
                }

                if (coordinate.X > Max.X)
                {
                    Max.X = coordinate.X;
                }
                if (coordinate.Y > Max.Y)
                {
                    Max.Y = coordinate.Y;
                }
                if (coordinate.Z > Max.Z)
                {
                    Max.Z = coordinate.Z;
                }
            }
        }
    }

    public class BoundingBox
    {
        public Coordinate Min = new Coordinate();
        public Coordinate Max = new Coordinate();

        public bool isInside(Coordinate3 coordinate)
        {
            if ((coordinate.X >= Min.X) && (coordinate.X <= Max.X) &&
                    (coordinate.Y >= Min.Y) && (coordinate.Y <= Max.Y))
            {
                return true;
            }

            return false;
        }

        public void Create(List<Coordinate> coords)
        {
            Min = new Coordinate(long.MaxValue, long.MaxValue);
            Max = new Coordinate(long.MinValue, long.MinValue);
            foreach (Coordinate coordinate in coords)
            {
                if (coordinate.X < Min.X)
                {
                    Min.X = coordinate.X;
                }
                if (coordinate.Y < Min.Y)
                {
                    Min.Y = coordinate.Y;
                }

                if (coordinate.X > Max.X)
                {
                    Max.X = coordinate.X;
                }
                if (coordinate.Y > Max.Y)
                {
                    Max.Y = coordinate.Y;
                }
            }
        }
    }


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

    public class DoubleCoordinate3
    {
        public double X;
        public double Y;
        public double Z;

        public DoubleCoordinate3()
        {

        }

        public DoubleCoordinate3(DoubleCoordinate3 rhs)
        {
            X = rhs.X;
            Y = rhs.Y;
            Z = rhs.Z;
        }

        public DoubleCoordinate3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public DoubleCoordinate3(string input)
        {
            List<double> coords = StringLibraries.GetListOfDoubles(input, ',');
            X = coords[0];
            Y = coords[1];
            Z = coords[2];
        }

        public double LengthSquared
        {
            get { return X * X + Y * Y + Z * Z; }
        }

        public double Length
        {
            get { return Math.Sqrt(LengthSquared); }
        }

        public double Normalize()
        {
            double length = Length;
            if (length > 0.000000001)
            {
                double invLength = 1.0 / length;
                X *= invLength;
                Y *= invLength;
                Z *= invLength;
            }
            else
            {
                length = 0;
                X = Y = Z = 0;
            }
            return length;
        }

        public void Subtract(DoubleCoordinate3 O)
        {
            X -= O.X;
            Y -= O.Y;
            Z -= O.Z;
        }

        public double Dot(DoubleCoordinate3 V2)
        {
            return X * V2.X + Y * V2.Y + Z * V2.Z;
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

            total = Math.Abs(total);
            if (includePerimeter)
            {
                total += Math.Abs((PolylineSquarePerimeter(coords) / 2) + 1);
            }

            return total;
        }
    }
}
