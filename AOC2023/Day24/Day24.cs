using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using AOCShared;
using Microsoft.Z3;

namespace Day24
{

    internal class HailStone
    {
        private bool m_part2 = false;
        public DoubleCoordinate3 StartCoordinate = new DoubleCoordinate3();
        public DoubleCoordinate3 Velocity = new DoubleCoordinate3();
        public DoubleCoordinate3 EndCoordinate = new DoubleCoordinate3();

        public HailStone(string line, bool part2)
        {
            m_part2 = part2;
            List<string> parts = StringLibraries.GetListOfStrings(line, '@');

            StartCoordinate = new DoubleCoordinate3(parts[0]);
            Velocity = new DoubleCoordinate3(parts[1]);

            if (!part2)
            {
                StartCoordinate.Z = 0;
                Velocity.Z = 0;
            }
            //Velocity.Normalize();

            EndCoordinate.X = StartCoordinate.X + (1 * Velocity.X);
            EndCoordinate.Y = StartCoordinate.Y + (1 * Velocity.Y);
            EndCoordinate.Z = StartCoordinate.Z + (1 * Velocity.Z);
        }


        public DoubleCoordinate3 Intersects2D(HailStone other)
        {
            double y43 = other.Velocity.Y;
            double x21 = Velocity.X;
            double x43 = other.Velocity.X;
            double y21 = Velocity.Y;

            double denominator = y43 * x21 - x43 * y21;

            if (Math.Abs(denominator) < 0.00000001)
            {
                // Lines are parallels or coincident
                return null;
            }

            double y13 = StartCoordinate.Y - other.StartCoordinate.Y;
            double x13 = StartCoordinate.X - other.StartCoordinate.X;

            double numeratorA = x43 * y13 - y43 * x13;
            double numeratorB = x21 * y13 - y21 * x13;
            double ua = numeratorA / denominator;
            double ub = numeratorB / denominator;

            if (ua > -1e-9
                && ub > -1e-9)
            {
                // Intersection is greater than both start points
                DoubleCoordinate3 doubleRet = new DoubleCoordinate3(StartCoordinate.X + ua * x21, StartCoordinate.Y + ua * y21, 0);
                return doubleRet;
            }

            return null;
        }

    }

    internal class BoundingBox
    {
        public DoubleCoordinate3 Min = new DoubleCoordinate3();
        public DoubleCoordinate3 Max = new DoubleCoordinate3();

        public bool isInside(DoubleCoordinate3 coordinate)
        {
            if ((coordinate.X >= Min.X) && (coordinate.X <= Max.X) &&
                    (coordinate.Y >= Min.Y) && (coordinate.Y <= Max.Y) &&
                    (coordinate.Z >= Min.Z) && (coordinate.Z <= Max.Z))
            {
                return true;
            }

            return false;
        }

    }


    internal class Day24
    {
        List<HailStone> inputObjects = new List<HailStone>();

        internal void ProcessInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // READER code here
                    HailStone av = new HailStone(line, part2);
                    inputObjects.Add(av);

                }
            }

        }

        internal long Execute1(string fileName)
        {
            long total = 0;
            BoundingBox b = new BoundingBox();

            //b.Min.X = 7;
            //b.Min.Y = 7;
            //b.Max.X = 27;
            //b.Max.Y = 27;
            //b.Min.Z = double.MinValue;
            //b.Max.Z = double.MaxValue;

            b.Min.X = 200000000000000;
            b.Min.Y = 200000000000000;
            b.Max.X = 400000000000000;
            b.Max.Y = 400000000000000;
            b.Min.Z = double.MinValue;
            b.Max.Z = double.MaxValue;

            for (int i = 0; i < inputObjects.Count; i++)
            {

                for (int j = i + 1; j < inputObjects.Count; j++)
                {
                    DoubleCoordinate3 intersection = inputObjects[i].Intersects2D(inputObjects[j]);
                    if ((intersection != null) && b.isInside(intersection))
                    {
                        total++;
                    }
                }
            }


            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;
            Context ctx = new Context();
            Solver solver = ctx.MkSolver();

            // Coordinates of the stone
            var x = ctx.MkIntConst("x");
            var y = ctx.MkIntConst("y");
            var z = ctx.MkIntConst("z");

            // Velocity of the stone
            var vx = ctx.MkIntConst("vx");
            var vy = ctx.MkIntConst("vy");
            var vz = ctx.MkIntConst("vz");

            StreamWriter writer = new StreamWriter("d:\\temp\\simul_line.txt");
            for (int i = 0; i < 3/*inputObjects.Count*/; i++)
            {
                var t = ctx.MkIntConst($"t{i}"); // time for the stone to reach the hail
                var hail = inputObjects[i];

                var px = ctx.MkInt(Convert.ToInt64(hail.StartCoordinate.X));
                var py = ctx.MkInt(Convert.ToInt64(hail.StartCoordinate.Y));
                var pz = ctx.MkInt(Convert.ToInt64(hail.StartCoordinate.Z));

                var pvx = ctx.MkInt(Convert.ToInt64(hail.Velocity.X));
                var pvy = ctx.MkInt(Convert.ToInt64(hail.Velocity.Y));
                var pvz = ctx.MkInt(Convert.ToInt64(hail.Velocity.Z));

                var xLeft = ctx.MkAdd(x, ctx.MkMul(t, vx)); // x + t * vx
                var yLeft = ctx.MkAdd(y, ctx.MkMul(t, vy)); // y + t * vy
                var zLeft = ctx.MkAdd(z, ctx.MkMul(t, vz)); // z + t * vz

                var xRight = ctx.MkAdd(px, ctx.MkMul(t, pvx)); // px + t * pvx
                var yRight = ctx.MkAdd(py, ctx.MkMul(t, pvy)); // py + t * pvy
                var zRight = ctx.MkAdd(pz, ctx.MkMul(t, pvz)); // pz + t * pvz

                solver.Add(t >= 0); // time should always be positive - we don't want solutions for negative time
                solver.Add(ctx.MkEq(xLeft, xRight)); // x + t * vx = px + t * pvx
                solver.Add(ctx.MkEq(yLeft, yRight)); // y + t * vy = py + t * pvy
                solver.Add(ctx.MkEq(zLeft, zRight)); // z + t * vz = pz + t * pvz                
                
                writer.WriteLine("x + (T" + i + " * vx) = " + inputObjects[i].StartCoordinate.X + " - (T" + i + " * " + inputObjects[i].Velocity.X + ")");
                writer.WriteLine("y + (T" + i + " * vy) = " + inputObjects[i].StartCoordinate.Y + " - (T" + i + " * " + inputObjects[i].Velocity.Y + ")");
                writer.WriteLine("z + (T" + i + " * vz) = " + inputObjects[i].StartCoordinate.Z + " - (T" + i + " * " + inputObjects[i].Velocity.Z + ")");
            }
            writer.Close();

            solver.Check();
            var model = solver.Model;

            var rx = model.Eval(x);
            var ry = model.Eval(y);
            var rz = model.Eval(z);

            total = Convert.ToInt64(rx.ToString()) + Convert.ToInt64(ry.ToString()) + Convert.ToInt64(rz.ToString());

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
