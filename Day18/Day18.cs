using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;
using static AOCShared.MathLibraries;

namespace Day18
{

    internal class AdventClass
    {
        private bool m_part2 = false;
        public Direction dir { get; set; } = Direction.Unknown;
        public int distance { get; set; } = 0;
        public string colour { get; set; } = string.Empty;

        public AdventClass(string line, bool part2)
        {
            m_part2 = part2;
            string[] splits = line.Split(' ');

            if (!part2)
            {
                switch (splits[0][0])
                {
                    case 'R':
                        dir = Direction.East;
                        break;
                    case 'L':
                        dir = Direction.West;
                        break;
                    case 'U':
                        dir = Direction.North;
                        break;
                    case 'D':
                        dir = Direction.South;
                        break;
                }

                distance = Convert.ToInt32(splits[1]);
                colour = splits[2];
            }
            else
            {
                string val = splits[2];
                val = val.Replace("(", "");
                val = val.Replace(")", "");
                val = val.Replace("#", "");

                switch (val[5])
                {
                    case '0':
                        dir = Direction.East;
                        break;
                    case '2':
                        dir = Direction.West;
                        break;
                    case '3':
                        dir = Direction.North;
                        break;
                    case '1':
                        dir = Direction.South;
                        break;
                }

                val = val.Substring(0, 5);
                distance = int.Parse(val, System.Globalization.NumberStyles.HexNumber);

            }


        }

        public long Calculate1()
        {
            long total = 0;

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            return total;
        }


        public long Calculate()
        {
            long total = 0;

            if (!m_part2)
            {
                total = Calculate1();
            }
            else
            {
                total = Calculate2();
            }

            return total;
        }
    }


    internal class Day18
    {
        List<AdventClass> inputObjects = new List<AdventClass>();

        internal void ProcessInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // READER code here
                    AdventClass av = new AdventClass(line, part2);
                    inputObjects.Add(av);
                }
            }
        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            List<Coordinate> coords = new List<Coordinate>();

            Coordinate coord = new Coordinate()
            {
                X = 0,
                Y = 0
            };
            coords.Add(coord);

            long length = 0;
            Coordinate newCoord = new Coordinate(coord);
            foreach (var obj in inputObjects)
            {
                newCoord = new Coordinate(newCoord);

                switch (obj.dir)
                {
                    case Direction.East:
                        newCoord.X += (obj.distance);
                        break;
                    case Direction.West:
                        newCoord.X -= (obj.distance);
                        break;
                    case Direction.North:
                        newCoord.Y += (obj.distance);
                        break;
                    case Direction.South:
                        newCoord.Y -= (obj.distance);
                        break;
                }

                length += obj.distance;
                coords.Add(newCoord);
            }

            total = MathLibraries.PolylineArea(coords, true);

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            foreach (var obj in inputObjects)
            {
                total += obj.Calculate();
            }

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
                total = Execute1(fileName);
            }

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(counter + ") " + "(" + millis + ") Result is: " + total);

        }

    }
}
