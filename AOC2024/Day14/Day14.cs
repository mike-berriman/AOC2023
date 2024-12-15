using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class Robot
    {
        public Coordinate StartPos { get; set; } = null;
        public Coordinate Velocity { get; set; } = null;


    }

    internal class Day13
    {
        private bool m_part2 = false;
        List<Robot> robots = new List<Robot>();

        public Day13(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;
            long gridWidth = 101;
            long gridHeight = 103;
            long timeframe = 100;

            AOCGrid grid = new AOCGrid(gridWidth, gridHeight);
            grid.Clear('.');

            foreach (Robot r in robots)
            {
                Coordinate endPos = new Coordinate();
                endPos.X = (r.StartPos.X + (r.Velocity.X * timeframe)) % gridWidth;
                endPos.Y = (r.StartPos.Y + (r.Velocity.Y * timeframe)) % gridHeight;

                if (endPos.X < 0)
                {
                    endPos.X += gridWidth;
                }
                if (endPos.Y < 0)
                {
                    endPos.Y += gridHeight;
                }

                char val = grid.Get(endPos);
                if (val == '.')
                {
                    grid.Set(endPos, (char)1);
                }
                else
                {
                    grid.Set(endPos, (char)(val + 1));
                }
            }

            Coordinate[] starts = new Coordinate[4];

            starts[0] = new Coordinate(0, 0);
            starts[1] = new Coordinate(0, gridHeight / 2+1);
            starts[2] = new Coordinate(gridWidth/2+1, 0);
            starts[3] = new Coordinate(gridWidth/2+1, gridHeight / 2+1);

            total = 1;
            foreach (Coordinate start in starts)
            {
                long quadTotal = 0;

                for (long i = start.X; i < start.X + gridWidth / 2; i++)
                {
                    for (long j = start.Y; j < start.Y + gridHeight / 2; j++)
                    {
                        char val = grid.Get(new Coordinate(i, j));
                        if (val != '.')
                        {
                            quadTotal += (long)(val);
                        }

                    }
                }

                total *= quadTotal;
            }


            return total;
        }

        public long Calculate2()
        {
            long total = 0;
            long gridWidth = 101;
            long gridHeight = 103;
            //long timeframe = 100;

            StreamWriter writer = new StreamWriter("d:\\temp\\advent\\robots\\robots.txt");

            for (int i = 52; i < 1000000; i+=103)
            {

                AOCGrid grid = new AOCGrid(gridWidth, gridHeight);
                grid.Clear('.');


                foreach (Robot r in robots)
                {
                    Coordinate endPos = new Coordinate();
                    endPos.X = (r.StartPos.X + (r.Velocity.X * i)) % gridWidth;
                    endPos.Y = (r.StartPos.Y + (r.Velocity.Y * i)) % gridHeight;

                    if (endPos.X < 0)
                    {
                        endPos.X += gridWidth;
                    }
                    if (endPos.Y < 0)
                    {
                        endPos.Y += gridHeight;
                    }

                    grid.Set(endPos, '*');
                }

                bool print = false;
                for (int aa = 0; aa < gridHeight; aa++)
                {
                    var row = grid.GetRow(aa);
                    //int count = row.Count(x => x == '*');
                    //if (count > 20)
                    //{
                    //    print = true;
                    //    break;
                    //}
                    if (row.Contains("********"))
                    {
                        total = i;
                        break;
                    }
                }

                if (total > 0)
                {
                    break;
                }
                if (print)
                {
                    //writer.WriteLine("Time: " + i);
                    //grid.WriteFile(writer);
                }
            }

            writer.Close();

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Split(' ');
                    List<long> coord = StringLibraries.GetListOfInts(splits[0].Substring(2), ',');
                    List<long> velocity = StringLibraries.GetListOfInts(splits[1].Substring(2), ',');

                    Robot r = new Robot();
                    r.StartPos = new Coordinate(coord[0], coord[1]);
                    r.Velocity = new Coordinate(velocity[0], velocity[1]);

                    robots.Add(r);
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
