using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class GraphNode : IEquatable<GraphNode>
    {
        public Coordinate Coord { get; set; } = null;
        public int Weight { get; set; } = 1;

        public GraphNode(Coordinate coord)
        {
            Coord = coord;
        }

        public GraphNode(Coordinate coord, int weight)
        {
            Coord = coord;
            Weight = weight;
        }

        public override int GetHashCode()
        {
            return (int)((Coord.X * 1000000) + (Coord.Y * 1000));
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as GraphNode);
        }

        public bool Equals(GraphNode? other)
        {
            return other != null && other.GetHashCode() == this.GetHashCode();
        }
    }

    public class UndirectedGraph : Dictionary<GraphNode, HashSet<GraphNode>>
    {
        public Coordinate End;
        HashSet<GraphNode> visited = new HashSet<GraphNode>();

        public void AddNode(GraphNode parent, GraphNode child)
        {
            if (!ContainsKey(parent))
            {
                this[parent] = new HashSet<GraphNode>();
            }
            this[parent].Add(child);
        }


        private long IntFindLongestPath(GraphNode currentCoord, GraphNode end)
        {
            long length = 0;

            if (currentCoord.Equals(end))
            {
                return visited.Count - 1;
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

        private long IntFindShortestPath(GraphNode currentCoord, GraphNode end)
        {
            long length = 9999999999;

            if (currentCoord.Equals(end))
            {
                return visited.Count - 1;
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

                    long neighbourLength = IntFindShortestPath(neighbour, end);
                    length = Math.Min(length, neighbourLength);

                    visited.Remove(neighbour);
                }
            }

            return length;
        }

        public long FindLongestPath(Coordinate start, Coordinate end)
        {
            GraphNode startNode = new GraphNode(start, 0);
            visited.Add(startNode);
            return IntFindLongestPath(startNode, new GraphNode(end));
        }

        public long FindShortestPath(Coordinate start, Coordinate end)
        {
            GraphNode startNode = new GraphNode(start, 0);
            visited.Add(startNode);
            return IntFindShortestPath(startNode, new GraphNode(end));
        }

        public static UndirectedGraph BuildSimplePathGraph(AOCGrid grid, char wallDelimiter)
        {
            UndirectedGraph graph = new UndirectedGraph();

            for (int x = 0; x < grid.GridWidth; x++)
            {
                for (int y = 0; y < grid.GridHeight; y++)
                {
                    Coordinate coord = new Coordinate(x, y);
                    GraphNode coordNode = new GraphNode(coord);
                    char val = grid.Get(coord);

                    if (val == wallDelimiter)
                    {
                        continue;
                    }

                    graph[coordNode] = new HashSet<GraphNode>();
                    for (int i = 0; i < 4; i++)
                    {
                        Coordinate newNode = new Coordinate(coord);
                        Direction dir = (Direction)i;

                        if (!grid.MoveNext(newNode, dir))
                        {
                            if (grid.Get(newNode) != wallDelimiter)
                            {
                                graph.AddNode(coordNode, new GraphNode(newNode));
                            }
                        }
                    }
                }
            }

            return graph;
        }

        public static UndirectedGraph BuildWeightedGraph(AOCGrid grid)
        {
            UndirectedGraph graph = new UndirectedGraph();

            for (int x = 0; x < grid.GridWidth; x++)
            {
                for (int y = 0; y < grid.GridHeight; y++)
                {
                    Coordinate coord = new Coordinate(x, y);
                    GraphNode coordNode = new GraphNode(coord);
                    char val = grid.Get(coord);

                    if (val == '#')
                    {
                        continue;
                    }

                    for (int i = 0; i < 4; i++)
                    {
                        Coordinate newNode = new Coordinate(coord);
                        Direction dir = (Direction)i;

                        if (!grid.MoveNext(newNode, dir))
                        {
                            if ((newNode != coord) && (grid.Get(newNode) != '#'))
                            {
                                graph.AddNode(coordNode, new GraphNode(newNode));
                            }
                        }
                    }
                }
            }

            return graph;
        }

    }
}
