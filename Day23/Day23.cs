using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Day23
{
    internal class DjikstraNode
    {
        public Coordinate Coord { get; set; } = new Coordinate();
        public long Distance { get; set; } = 0;
        public bool EnforceDirection { get; set; } = false;
        public Direction lastDirection = Direction.Unknown;
        public int Path { get; set; } = 0;


        public DjikstraNode()
        {

        }

        public DjikstraNode(DjikstraNode rhs)
        {
            Coord = new Coordinate(rhs.Coord);
            Distance = 0;
            lastDirection = rhs.lastDirection;
            Path = rhs.Path;
            EnforceDirection = rhs.EnforceDirection;
        }

        public override int GetHashCode()
        {
            return (int)((Coord.X * 10000000000) + (Coord.Y * 10000000) + Path);
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

    internal class UndirectedGraph : Dictionary<Coordinate, HashSet<Coordinate>>
    {
        public Coordinate End;
        HashSet<Coordinate> visited = new HashSet<Coordinate>();

        public void AddNode(Coordinate parent, Coordinate child)
        {
            if (!ContainsKey(parent))
            {
                this[parent] = new HashSet<Coordinate>();
            }
            this[parent].Add(child);
        }


        private long IntFindLongestPath(Coordinate currentCoord, Coordinate end)
        {
            long length = 0;

            if (currentCoord.Equals(end))
            {
                return visited.Count-1;
            }

            foreach (var neighbour in this[currentCoord])
            {
                if (visited.Contains(neighbour))
                {
                    continue;
                }
                else
                {
                    visited.Add(neighbour);

                    long neighbourLength = IntFindLongestPath(neighbour, end);
                    length = Math.Max(length, neighbourLength);

                    visited.Remove(neighbour);


                }
            }

            return length;
        }

        public long FindLongestPath(Coordinate start, Coordinate end)
        {
            visited.Add(start);
            return IntFindLongestPath(start, end);
        }
    }

    internal class AdventObject
    {
        private bool m_part2 = false;
        public AOCGrid Weights = null;
        public Queue<DjikstraNode> NodeQueue = new Queue<DjikstraNode>();
        public Dictionary<DjikstraNode, long> VisitedCache = new Dictionary<DjikstraNode, long>();

        public AdventObject(string fileName, bool part2)
        {
            Weights = new AOCGrid(fileName);

            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;
            int maxPath = 0;

            DjikstraNode rootNode = new DjikstraNode()
            {
                Coord = new Coordinate() { X = 1, Y = 0 },
                Distance = 0,
            };
            NodeQueue.Enqueue(rootNode);

            while (NodeQueue.Count > 0)
            {
                DjikstraNode thisNode = NodeQueue.Dequeue();

                if (VisitedCache.ContainsKey(thisNode))
                {
                    continue;

                }
                else
                {
                    VisitedCache.Add(thisNode, thisNode.Distance);
                }

                int pathCount = 0;
                for (int i = 0; i < 4; i++)
                {
                    DjikstraNode newNode = new DjikstraNode(thisNode);
                    newNode.EnforceDirection = false;
                    newNode.lastDirection = (Direction)i;

                    if (!Weights.MoveNext(newNode.Coord, newNode.lastDirection))
                    {
                        newNode.Distance = thisNode.Distance + 1;

                        if (DirectionExtensions.IsOppositeDirection(thisNode.lastDirection, newNode.lastDirection))
                        {
                            continue;
                        }

                        if ((thisNode.EnforceDirection) && (newNode.lastDirection != thisNode.lastDirection))
                        {
                            continue;
                        }

                        char val = Weights.Get(newNode.Coord);
                        if (val == '#')
                        {
                            continue;
                        }

                        if (pathCount == 0)
                        {
                            newNode.Path = thisNode.Path;
                        }
                        else
                        {
                            maxPath++;
                            // Create a new path

                            var oldPath = VisitedCache.Where(x => x.Key.Path == thisNode.Path).ToList();
                            foreach (var cacheValue in oldPath)
                            {
                                DjikstraNode newCacheNode = new DjikstraNode(cacheValue.Key);
                                newCacheNode.Path = maxPath;
                                VisitedCache.Add(newCacheNode, VisitedCache[cacheValue.Key]);
                            }
                            newNode.Path = maxPath;
                        }
                        pathCount++;

                        if (val != '.')
                        {
                            if (!m_part2)
                            {
                                newNode.EnforceDirection = true;

                                switch (val)
                                {
                                    case 'v':
                                        newNode.lastDirection = Direction.South;
                                        break;
                                    case '>':
                                        newNode.lastDirection = Direction.East;
                                        break;
                                    case '<':
                                        newNode.lastDirection = Direction.West;
                                        break;
                                    case '^':
                                        newNode.lastDirection = Direction.North;
                                        break;

                                }
                            }


                        }

                        if (!m_part2)
                        {
                        }
                        else
                        {
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

            total = 0;
            foreach (var val in VisitedCache.Keys.Where(x => (x.Coord.X == (Weights.GridWidth - 2)) && (x.Coord.Y == (Weights.GridHeight - 1))))
            {
                total = Math.Max(VisitedCache[val], total);
            }

            return total;
        }

        public UndirectedGraph BuildGraph()
        {
            UndirectedGraph graph = new UndirectedGraph();

            for (int x = 0; x < Weights.GridWidth; x++)
            {

                for (int y = 0; y < Weights.GridHeight; y++)
                {
                    Coordinate coord = new Coordinate(x, y);
                    char val = Weights.Get(coord);

                    if (val == '#')
                    {
                        continue;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        Coordinate newNode = new Coordinate(coord);
                        Direction dir = (Direction)i;

                        if (!Weights.MoveNext(newNode, dir))
                        {
                            if ((newNode != coord) &&(Weights.Get(newNode) != '#'))
                            {
                                graph.AddNode(coord, newNode);
                            }
                        }
                    }
                }
            }

            return graph;
        }


        public long Calculate2()
        {
            long total = 0;

            UndirectedGraph graph = BuildGraph();

            Coordinate start = new Coordinate(1, 0);
            Coordinate end = new Coordinate(Weights.GridWidth-2, Weights.GridHeight-1);

            total = graph.FindLongestPath(start, end);

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


    internal class Day23
    {
        AdventObject inputObjects = null;
        public string FileName;
        public bool Part2;
        public int Counter;

        internal void ProcessInput(string fileName, bool part2)
        {
            // READER code here
            inputObjects = new AdventObject(fileName, part2);

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

            total = inputObjects.Calculate2();

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

        public void Execute()
        {
            DateTime startTime = DateTime.Now;

            ProcessInput(FileName, Part2);

            long total;
            if (!Part2)
            {
                total = Execute1(FileName);
            }
            else
            {
                total = Execute2(FileName);
            }

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(Counter + ") " + "(" + millis + ") Result is: " + total);

        }

    }
}
