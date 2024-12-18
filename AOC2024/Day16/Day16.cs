using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class ReindeerNode
    {
        public Coordinate Coord { get; set; } = new Coordinate();
        public long Cost { get; set; } = 0;
        public Direction Direction { get; set; } = Direction.Unknown;

        public ReindeerNode()
        {
            Coord = new Coordinate() { X = 0, Y = 0 };
            Cost = 0;
            Direction = Direction.Unknown;
        }

        public void SetFrom(ReindeerNode rhs)
        {
            Coord = new Coordinate(rhs.Coord);
            Cost = rhs.Cost;
            Direction = rhs.Direction;
        }

        public override int GetHashCode()
        {
            //return (Coord.X * 1000000) + (Coord.Y * 1000);
            return (int)((Coord.X * 10000000) + (Coord.Y * 10000) + (Convert.ToInt32(Direction) * 1000));
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as ReindeerNode);
        }

        public bool Equals(ReindeerNode obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }

    }

    public class ReindeerAlgorithm
    {
        AOCGrid m_grid = null;
        public Queue<ReindeerNode> NodeQueue = new Queue<ReindeerNode>();
        public Dictionary<ReindeerNode, long> VisitedCache = new Dictionary<ReindeerNode, long>();

        public ReindeerAlgorithm(AOCGrid grid)
        {
            m_grid = grid;
        }

        public long Calculate(ReindeerNode rootNode, Coordinate endNode)
        {
            long total = 0;

            NodeQueue.Enqueue(rootNode);

            while (NodeQueue.Count > 0)
            {
                ReindeerNode thisNode = NodeQueue.Dequeue();

                if (VisitedCache.ContainsKey(thisNode))
                {
                    if (VisitedCache[thisNode] > thisNode.Cost)
                    {
                        // Replace the distance measure with the new distance measure
                        VisitedCache.Remove(thisNode);
                        VisitedCache.Add(thisNode, thisNode.Cost);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    VisitedCache.Add(thisNode, thisNode.Cost);
                }

                for (int i = 0; i < 4; i++)
                {
                    ReindeerNode newNode = new ReindeerNode();
                    newNode.SetFrom(thisNode);
                    newNode.Direction = (Direction)i;

                    if (DirectionExtensions.IsOppositeDirection(thisNode.Direction, newNode.Direction))
                    {
                        // Don't look backwards
                        continue;
                    }

                    //if (newNode.Direction != thisNode.Direction)
                    //{
                    //    newNode.NumTurns++;
                    //}

                    if (!m_grid.MoveNext(newNode.Coord, newNode.Direction))
                    {
                        if (m_grid.Get(newNode.Coord) != '#')
                        {
                            DoProcessing(thisNode, newNode);
                        }
                    }
                }
            }

            return GetResult(endNode);
        }

        public virtual void DoProcessing(ReindeerNode currentNode, ReindeerNode nextNode)
        {
            long thisVal = currentNode.Cost + 1;
            if (currentNode.Direction != nextNode.Direction)
            {
                thisVal += 1000;
            }

            nextNode.Cost = thisVal;

            NodeQueue.Enqueue(nextNode);
        }

        public List<Direction> EndDirections { get; set; } = new List<Direction>();
        public long GetResult(Coordinate endNode)
        {
            long total = long.MaxValue;

            foreach (var val in VisitedCache.Keys.Where(x => (x.Coord.Equals(endNode))))
            {
                total = Math.Min(VisitedCache[val], total);
                EndDirections.Add(val.Direction);
            }

            return total;
        }

    }

    internal class Day16
    {
        private bool m_part2 = false;
        private AOCGrid m_grid = null;

        public Day16(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            ReindeerNode startNode = new ReindeerNode();
            startNode.Coord = m_grid.FindAll('S').First();
            startNode.Direction = Direction.East;

            ReindeerAlgorithm alg = new ReindeerAlgorithm(m_grid);
            return alg.Calculate(startNode, m_grid.FindAll('E').First());
        }

        public bool Calculate(ReindeerNode startNode, Coordinate endCoord, Coordinate testCoord, long expectedVal)
        {
            bool onPath = false;
            ReindeerNode startNew = new ReindeerNode();

            ReindeerAlgorithm alg = new ReindeerAlgorithm(m_grid);
            long calc = alg.Calculate(startNode, testCoord);

            foreach (Direction dir in alg.EndDirections)
            {
                ReindeerAlgorithm alg2 = new ReindeerAlgorithm(m_grid);
                ReindeerNode endNode = new ReindeerNode();
                endNode.Coord = testCoord;
                endNode.Direction = dir;

                long endVal = alg2.Calculate(endNode, endCoord);

                if (calc + endVal == expectedVal)
                {
                    onPath = true;
                    break;
                }
            }

            return onPath;
        }

        public long Calculate2()
        {
            long expectedVal = Calculate1();

            long total = 0;
            ReindeerNode startNode = new ReindeerNode();
            startNode.Coord = m_grid.FindAll('S').First();
            startNode.Direction = Direction.East;

            Coordinate end = m_grid.FindAll('E').First();

            //Coordinate testCoord = new Coordinate(13, 1);
            //char testVal = m_grid.Get(testCoord);
            //Calculate(startNode, end, testCoord, expectedVal);

            AOCGrid result = new AOCGrid(m_grid);

            int count = 0;
            Parallel.For(0, m_grid.GridWidth,
            x =>
            {
                Console.WriteLine("Row : " + count + " (Of " + m_grid.GridWidth + ")");
                Interlocked.Add(ref count, 1);
                for (int y = 0; y < m_grid.GridHeight; y++)
                {
                    if (m_grid.Get((int)x, y) != '#')
                    {
                        Coordinate coord = new Coordinate(x, y);
                        if (Calculate(startNode, end, new Coordinate(x, y), expectedVal))
                        {
                            result.Set(coord, 'O');
                            Interlocked.Add(ref total, 1);
                        }

                    }
                }
            });

            Console.Clear();
            result.PrintToConsole("Final result", false);
            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            m_grid = new AOCGrid(fileName);
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
