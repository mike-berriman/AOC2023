using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Day17
{
    internal class DjikstraNode
    {
        public Coordinate Coord { get; set; } = new Coordinate();
        public long Distance { get; set; } = long.MaxValue;
        public Direction Direction { get; set; }
        public int StepsInDirection { get; set; }

        public DjikstraNode()
        {

        }

        public DjikstraNode(DjikstraNode rhs)
        {
            Coord = new Coordinate(rhs.Coord);
            Distance = long.MaxValue;
            Direction = rhs.Direction;
            StepsInDirection = rhs.StepsInDirection;
        }

        public override int GetHashCode()
        {
            //return (Coord.X * 1000000) + (Coord.Y * 1000);
            return (Coord.X * 1000000) + (Coord.Y * 1000) + (Convert.ToInt32(Direction) * 100) + StepsInDirection;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as DjikstraNode);
        }

        public bool Equals(DjikstraNode obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }

    }


    internal class DjikstraClass
    {
        private bool m_part2 = false;
        public AOCGrid Weights = null;
        public Queue<DjikstraNode> NodeQueue = new Queue<DjikstraNode>();
        public Dictionary<DjikstraNode, long> VisitedCache = new Dictionary<DjikstraNode, long>();

        public DjikstraClass(string fileName, bool part2)
        {
            Weights = new AOCGrid(fileName);
            Weights.ConvertToIntegers();

            m_part2 = part2;
        }

        public bool IsOppositeDirection(Direction dir1, Direction dir2)
        {
            bool reverse = false;
            switch (dir1)
            {
                case Direction.East:
                    if (dir2 == Direction.West)
                    {
                        reverse = true;
                    }
                    break;
                case Direction.West:
                    if (dir2 == Direction.East)
                    {
                        reverse = true;
                    }
                    break;
                case Direction.North:
                    if (dir2 == Direction.South)
                    {
                        reverse = true;
                    }
                    break;
                case Direction.South:
                    if (dir2 == Direction.North)
                    {
                        reverse = true;
                    }
                    break;
            }

            return reverse;
        }

        public long Calculate1()
        {
            long total = 0;

            DjikstraNode rootNode = new DjikstraNode()
            {
                Coord = new Coordinate() { X = 0, Y = 0 },
                Distance = 0,
                Direction = Direction.Unknown,
                StepsInDirection = 0
            };
            NodeQueue.Enqueue(rootNode);

            while (NodeQueue.Count > 0)
            {
                DjikstraNode thisNode = NodeQueue.Dequeue();

                if (VisitedCache.ContainsKey(thisNode))
                {
                    if (VisitedCache[thisNode] > thisNode.Distance)
                    {
                        //VisitedCache[thisNode] = thisNode.Distance;
                        VisitedCache.Remove(thisNode);
                        VisitedCache.Add(thisNode, thisNode.Distance);
                    }
                    else
                    {
                        continue;
                    }

                }
                else
                {
                    VisitedCache.Add(thisNode, thisNode.Distance);
                }


                for (int i = 0; i < 4; i++)
                {
                    DjikstraNode newNode = new DjikstraNode(thisNode);
                    newNode.Direction = (Direction)i;

                    if (IsOppositeDirection(thisNode.Direction, newNode.Direction))
                    {
                        continue;
                    }

                    if (!Weights.MoveNext(newNode.Coord, newNode.Direction))
                    {
                        long value = Weights.Get(newNode.Coord);
                        newNode.Distance = thisNode.Distance + value;

                        if (newNode.Direction == thisNode.Direction)
                        {
                            newNode.StepsInDirection++;
                        }
                        else
                        {
                            newNode.StepsInDirection = 1;
                        }

                        if (!m_part2)
                        {
                            if (newNode.StepsInDirection > 3)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (thisNode.Direction != Direction.Unknown)
                            {
                                if (((thisNode.StepsInDirection < 4) && (thisNode.Direction != newNode.Direction)) ||
                                    (newNode.StepsInDirection > 10))
                                {
                                    continue;
                                }
                            }
                        }

                        NodeQueue.Enqueue(newNode);
                    }
                }
            }

            //StreamWriter writer = new StreamWriter("d:\\temp\\debugOutput.txt");
            //for (int i = 0; i < Weights.GridHeight; i++)
            //{
            //    for (int j = 0; j < Weights.GridWidth; j++)
            //    {
            //        long val = long.MaxValue;


            //        foreach (var thisval in VisitedCache.Keys.Where(x => (x.Coord.X == j) && (x.Coord.Y == i)))
            //        {
            //            val = Math.Min(VisitedCache[thisval], val);
            //        }

            //        Coordinate coord = new Coordinate()
            //        {
            //            X = j,
            //            Y = i
            //        };
            //        long inc = Weights.Get(coord);

            //        writer.Write(string.Format("{0,5}({1})", val, inc));

            //    }
            //    writer.WriteLine();
            //    writer.WriteLine();
            //}
            //writer.Close();

            total = long.MaxValue;
            foreach (var val in VisitedCache.Keys.Where(x => (x.Coord.X == (Weights.GridWidth - 1)) && (x.Coord.Y == (Weights.GridHeight - 1))))
            {
                total = Math.Min(VisitedCache[val], total);
            }

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
                total = Calculate1();
            }

            return total;
        }
    }


    internal class Day17_Djikstra
    {
        DjikstraClass inputObjects = null;

        internal void ProcessInput(string fileName, bool part2)
        {
            // READER code here
            inputObjects = new DjikstraClass(fileName, part2);

        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            total = inputObjects.Calculate1();

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            total = inputObjects.Calculate1();

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
