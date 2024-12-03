using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day22
{

    internal class Brick
    {
        private bool m_part2 = false;
        public Coordinate3 StartCoord = new Coordinate3();
        public Coordinate3 EndCoord = new Coordinate3();
        public string Name;
        public List<Brick> IsSupporting = new List<Brick>();
        public List<Brick> SupportedBy = new List<Brick>();


        public Brick(string name, string line, bool part2)
        {
            m_part2 = part2;
            List<string> splits = StringLibraries.GetListOfStrings(line, '~');
            StartCoord = new Coordinate3(splits[0]);
            EndCoord = new Coordinate3(splits[1]);
            Name = name;
        }

        public Brick(Brick rhs)
        {
            Name = rhs.Name;
            StartCoord = new Coordinate3(rhs.StartCoord);
            EndCoord = new Coordinate3(rhs.EndCoord);
        }

        public bool CalculateSupporting(List<Brick> bricks)
        {
            bool returnCode = true;
            foreach (Brick b in bricks)
            {
                if (b.Name != Name)
                {
                    if (b.StartCoord.Z == (this.EndCoord.Z + 1))
                    {
                        if (IntersectsXY(b))
                        {
                            this.IsSupporting.Add(b);
                            b.SupportedBy.Add(this);

                            //Console.WriteLine(Name + " Is Supporting ", b.Name);
                        }
                    }
                }
            }

            return returnCode;
        }

        public bool CanSafelyDissolve(List<Brick> bricks)
        {
            if (IsSupporting.Count == 0)
            {
                return true;
            }

            bool supportingAll = true;
            foreach (var suppBrick in IsSupporting)
            {
                if (suppBrick.SupportedBy.Count <= 1)
                {
                    supportingAll = false;
                }
            }

            return supportingAll;
        }

        public bool Intersects1D(long min1, long min2, long max1, long max2)
        {
            return (max1 >= min2) && (max2 >= min1);
        }

        public bool IntersectsXY(Brick b)
        {
            bool intersects = false;
            if (Intersects1D(b.StartCoord.Y, this.StartCoord.Y, b.EndCoord.Y, this.EndCoord.Y) &&
                Intersects1D(b.StartCoord.X, this.StartCoord.X, b.EndCoord.X, this.EndCoord.X))
            {
                intersects = true;
            }

            return intersects;
        }

        public bool Intersects3d(Brick b)
        {
            bool intersects = false;
            if (Intersects1D(b.StartCoord.Z, this.StartCoord.Z, b.EndCoord.Z, this.EndCoord.Z) &&
                Intersects1D(b.StartCoord.Y, this.StartCoord.Y, b.EndCoord.Y, this.EndCoord.Y) &&
                Intersects1D(b.StartCoord.X, this.StartCoord.X, b.EndCoord.X, this.EndCoord.X))
            {
                intersects = true;
            }

            return intersects;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Name : " + Name);
            builder.AppendLine("X : (" + StartCoord.X + "," + EndCoord.X + ")");
            builder.AppendLine("Y : (" + StartCoord.Y + "," + EndCoord.Y + ")");
            builder.AppendLine("Z : (" + StartCoord.Z + "," + EndCoord.Z + ")");

            builder.AppendLine("Supports : " + string.Join(',', IsSupporting));
            builder.AppendLine("Supported By : " + string.Join(',', SupportedBy));

            builder.AppendLine("--------------------");


            return builder.ToString();
        }

    }


    internal class Day22
    {
        List<Brick> inputObjects = new List<Brick>();

        internal void ProcessInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int count = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // READER code here
                    Brick av = new Brick(count.ToString(), line, part2);
                    inputObjects.Add(av);
                    count++;
                }
            }
        }
        public void BricksFall2()
        {
            inputObjects = inputObjects.OrderBy(x => x.StartCoord.Z).ToList();

            for (int i = 0; i < inputObjects.Count; i++)
            {
                Brick brick = inputObjects[i];

                if (brick.StartCoord.Z == 1)
                {
                    continue;
                }

                List<Brick> supportingBricks = new List<Brick>();

                for (int j = 0; j < i; j++)
                {
                    if (brick.IntersectsXY(inputObjects[j]))
                    {
                        supportingBricks.Add(inputObjects[j]);
                    }
                }

                long maxHeight = 0;
                if (supportingBricks.Count > 0)
                {
                    maxHeight = supportingBricks.Max(x => x.EndCoord.Z);
                }

                foreach (Brick b in supportingBricks.Where(x => x.EndCoord.Z == maxHeight))
                {
                    brick.SupportedBy.Add(b);
                    b.IsSupporting.Add(brick);
                }

                long brickDiff = brick.EndCoord.Z - brick.StartCoord.Z;
                brick.StartCoord.Z = maxHeight + 1;
                brick.EndCoord.Z = maxHeight + 1 + brickDiff;
            }
        }


        internal long Execute1(string fileName)
        {
            long total = 0;

            BricksFall2();

            foreach (var obj in inputObjects)
            {
                if (obj.CanSafelyDissolve(inputObjects))
                {
                    total++;
                }
            }

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            BricksFall2();

            foreach (var brick in inputObjects)
            {
                var queue = new Queue<Brick>();
                queue.Enqueue(brick);
                var disintegrated = new HashSet<Brick>();

                while (queue.TryDequeue(out var currentBrick))
                {
                    disintegrated.Add(currentBrick);

                    foreach (var above in currentBrick.IsSupporting)
                    {
                        if (above.SupportedBy.All(x => disintegrated.Contains(x)))
                        {
                            total++;
                            queue.Enqueue(above);

                        }
                    }
                }
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
                total = Execute2(fileName);
            }

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(counter + ") " + "(" + millis + ") Result is: " + total);

        }

    }
}
