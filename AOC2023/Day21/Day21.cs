using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day21
{
    internal class DjikstraNode
    {
        public Coordinate Coord { get; set; } = new Coordinate();
        public long Distance { get; set; } = long.MaxValue;

        public DjikstraNode()
        {

        }

        public DjikstraNode(DjikstraNode rhs)
        {
            Coord = new Coordinate(rhs.Coord);
            Distance = long.MaxValue;
        }

        public override int GetHashCode()
        {
            return (int)((Coord.X * 1000000) + (Coord.Y * 1000));
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


    internal class AdventClass
    {
        private bool m_part2 = false;
        public AOCGrid Weights = null;
        public Queue<DjikstraNode> NodeQueue = new Queue<DjikstraNode>();
        public Dictionary<DjikstraNode, long> VisitedCache = new Dictionary<DjikstraNode, long>();

        public AdventClass(string fileName, bool part2)
        {
            m_part2 = part2;
            Weights = new AOCGrid(fileName);

        }

        public Coordinate GetStartCoordinate()
        {
            Coordinate c = new Coordinate();
            for (int i = 0; i < Weights.GridHeight; i++)
            {
                int index = Weights.GetRow(i).IndexOf('S');
                if (index > 0)
                {
                    c = new Coordinate() { X = index, Y = i };
                }
            }

            return c;
        }

        public long Calculate1()
        {
            long total = 0;
            DjikstraNode rootNode = new DjikstraNode()
            {
                Distance = 0
            };

            rootNode.Coord = GetStartCoordinate();

            if (m_part2)
            {
                long AdditonalGrids = 11;
                long gridOffset = AdditonalGrids / 2;
                Weights.MultiplyGrid((int)AdditonalGrids);

                rootNode.Coord.X += (AdditonalGrids / 2) * 131;
                rootNode.Coord.Y += (AdditonalGrids / 2) * 131;
            }

            int oldGridWidth = Weights.GridWidth;

            Weights.Set(rootNode.Coord, '.');

            List<long> requiredGrids = new List<long>();

            if (!m_part2)
            {
                requiredGrids.Add(64);
            }
            else
            {
                requiredGrids.Add(65);
                requiredGrids.Add(65 + 131);
                requiredGrids.Add(65 + (131*2));
            }

            long max = requiredGrids[requiredGrids.Count - 1]+1;

            SortedList<long, AOCGrid> allGrids = new SortedList<long, AOCGrid>();
            foreach (long val in requiredGrids)
            {
                allGrids.Add(val, new AOCGrid(Weights));
            }

            VisitedCache.Clear();
            total = 0;

            NodeQueue.Enqueue(rootNode);

            //for (int steps = 1; steps < max; steps++)
            {
                while (NodeQueue.Count > 0)
                {
                    DjikstraNode thisNode = NodeQueue.Dequeue();

                    if (VisitedCache.ContainsKey(thisNode))
                    {
                        if (VisitedCache[thisNode] != thisNode.Distance)
                        {
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
                        Direction dir = (Direction)i;

                        if (!Weights.MoveNext(newNode.Coord, dir))
                        {
                            newNode.Distance = thisNode.Distance + 1;

                            if (newNode.Distance > max)
                            {
                                continue;
                            }

                            char val = Weights.Get(newNode.Coord);
                            if (val != '#')
                            {
                                if (allGrids.ContainsKey(newNode.Distance))
                                {
                                    allGrids[newNode.Distance].Set(newNode.Coord, '0');
                                }

                                NodeQueue.Enqueue(newNode);
                            }
                        }
                    }
                }
            }

            //StreamWriter writer = new StreamWriter("d:\\temp\\results.csv");
            SortedList<long, long> values = new SortedList<long, long>();
            foreach (var grid in allGrids)
            {
                long count = 0;
                foreach (char[] line in grid.Value.Grid)
                {
                    count += line.Count(x => x == '0');
                }

                values[grid.Key] = count;
                //writer.WriteLine(aaa + "," + total);
                //Console.WriteLine(aaa + "," + total);
            }
            //writer.Close();

            if (!m_part2)
            {
                total = values[64];
            }
            else
            {
                long grids = 26501365 / 131;
                // Solve for the quadratic coefficients
                // ax^2 + bx + c
                long c = values[(int)requiredGrids[0]];
                long aPlusB = values[(int)requiredGrids[1]] - c;
                long fourAPlusTwoB = values[(int)requiredGrids[2]] - c;
                long twoA = fourAPlusTwoB - (2 * aPlusB);
                long a = twoA / 2;
                long b = aPlusB - a;

                total = a * (grids * grids) + b * grids + c;
            }


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


    internal class Day21
    {
        AdventClass inputObjects = null;

        internal void ProcessInput(string fileName, bool part2)
        {
            inputObjects = new AdventClass(fileName, part2);

        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            total += inputObjects.Calculate();

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            total += inputObjects.Calculate();

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
