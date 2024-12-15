using AOCShared;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Z3;

namespace AOC2024
{
    public class Machine
    {
        public Coordinate ButtonAIncrement { get; set; } = null;
        public Coordinate ButtonBIncrement { get; set; } = null;

        public Coordinate Prize { get; set; } = null;

        public List<Coordinate> FindWinners()
        {
            List<Coordinate> results = new List<Coordinate>();

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    long XResult = (ButtonAIncrement.X * i) + (ButtonBIncrement.X * j);
                    long YResult = (ButtonAIncrement.Y * i) + (ButtonBIncrement.Y * j);

                    if ((Prize.X == XResult) && (Prize.Y == YResult))
                    {
                        results.Add(new Coordinate(i, j));
                    }
                }
            }

            return results;
        }

        public long CalculateCost(Coordinate winner)
        {
            return (winner.X * 3) + winner.Y;
        }

        public List<Coordinate> FindWinnersPart2()
        {
            List<Coordinate> results = new List<Coordinate>();

            Context ctx = new Context();
            Solver solver = ctx.MkSolver();

            var A = ctx.MkIntConst($"A"); 
            var B = ctx.MkIntConst($"B");

            var pax = ctx.MkInt(Convert.ToInt64(ButtonAIncrement.X));
            var pay = ctx.MkInt(Convert.ToInt64(ButtonAIncrement.Y));

            var pbx = ctx.MkInt(Convert.ToInt64(ButtonBIncrement.X));
            var pby = ctx.MkInt(Convert.ToInt64(ButtonBIncrement.Y));

            var resx = ctx.MkInt(Convert.ToInt64(Prize.X));
            var resy = ctx.MkInt(Convert.ToInt64(Prize.Y));

            var X = ctx.MkAdd(ctx.MkMul(A, pax), ctx.MkMul(B, pbx));
            var Y = ctx.MkAdd(ctx.MkMul(A, pay), ctx.MkMul(B, pby));

            var prizeOffset = ctx.MkInt(3);

            solver.Add(A >= 0);
            solver.Add(B >= 0);
            solver.Add(ctx.MkEq(X, resx));
            solver.Add(ctx.MkEq(Y, resy));

            {
                Status statusVAl = solver.Check();

                if (statusVAl == Status.SATISFIABLE)
                {
                    var model = solver.Model;

                    var resA = Convert.ToInt64(model.Eval(A).ToString());
                    var resB = Convert.ToInt64(model.Eval(B).ToString());

                    Coordinate result = new Coordinate(resA, resB);
                    results.Add(result);
                }
            }


            return results;
        }


    }

    internal class Day13
    {
        private bool m_part2 = false;
        private List<Machine> machines = new List<Machine>();

        public Day13(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;
            
            foreach (Machine m in machines)
            {
                var winners = m.FindWinners();

                if (winners.Count > 0)
                {
                    long cheapest = long.MaxValue;

                    foreach (var winner in winners)
                    {
                        long cost = m.CalculateCost(winner);
                        if (cost < cheapest)
                        {
                            cheapest = cost;
                        }
                    }

                    total += cheapest;
                }
            }

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            foreach (Machine m in machines)
            {
                m.Prize.X += 10000000000000;
                m.Prize.Y += 10000000000000;
            }

            foreach (Machine m in machines)
            {
                var winners = m.FindWinnersPart2();

                if (winners.Count > 0)
                {
                    long cheapest = long.MaxValue;

                    foreach (var winner in winners)
                    {
                        long cost = m.CalculateCost(winner);
                        if (cost < cheapest)
                        {
                            cheapest = cost;
                        }
                    }

                    total += cheapest;
                }

            }


            return total;
        }

        private Coordinate GetIncrements(string line, char previous)
        {
            int x = line.IndexOf(previous);
            int comma = line.IndexOf(',');
            int y = line.IndexOf(previous, comma);

            long xVal = Convert.ToInt64(line.Substring(x + 1, comma - (x + 1)).Trim());
            long yVal = Convert.ToInt64(line.Substring(y + 1).Trim());
            return new Coordinate(xVal, yVal);
        }

        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;
            bool finished = false;
            while (!finished)
            {
                line = rdr.ReadLine();
                if (!string.IsNullOrEmpty(line))
                {
                    Machine m = new Machine();

                    m.ButtonAIncrement = GetIncrements(line, '+');

                    line = rdr.ReadLine();
                    m.ButtonBIncrement = GetIncrements(line, '+');

                    line = rdr.ReadLine();
                    m.Prize = GetIncrements(line, '=');

                    machines.Add(m);
                }

                if (rdr.EndOfStream)
                {
                    break;
                }
            }

            rdr.Close();
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
