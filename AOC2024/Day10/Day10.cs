using AOCShared;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class Day10Graph : UndirectedGraph
    {
        public List<Day10Graph> graphs = new List<Day10Graph>();
        public Coordinate Root = null;
        public List<Coordinate> heads = new List<Coordinate>();

        public void Populate(AOCGrid grid, Coordinate root)
        {
            int val = grid.GetInt(root);

            for (int i = 0; i < 4; i++)
            {
                Coordinate newNode = new Coordinate(root);
                Direction dir = (Direction)i;

                if (!grid.MoveNext(newNode, dir))
                {
                    if (grid.GetInt(newNode) == val+1)
                    {
                        AddNode(new GraphNode(root), new GraphNode(newNode));
                        Populate(grid, newNode);
                    }
                }
            }
        }

        public void PopulateGraph(AOCGrid grid)
        {
            List<Coordinate> roots = grid.FindAll('0');

            foreach (Coordinate root in roots)
            {
                Day10Graph graph = new Day10Graph();
                graph.Root = root;
                graph.Populate(grid, root);

                graphs.Add(graph);
            }
        }

        public long RecursiveCount(AOCGrid grid, GraphNode g)
        {
            long total = 0;
            if (this.ContainsKey(g))
            {
                foreach (var node in this[g])
                {
                    total += RecursiveCount(grid, node);
                }
            }
            else
            {
                if (grid.GetInt(g.Coord) == 9)
                {
                    total += 1;
                    heads.Add(g.Coord);
                }
            }

            return total;
        }

        public long CalculateSingle(AOCGrid grid, bool part1)
        {
            long total = 0;

            total = RecursiveCount(grid, new GraphNode(Root));

            if (part1)
            {
                total = heads.Distinct().Count();
            }

            return total;
        }

        public long Calculate(AOCGrid grid, bool part1)
        {
            long total = 0;
            foreach (var g in graphs)
            {
                total += g.CalculateSingle(grid, part1);
            }
            return total;
        }
    }

    internal class Day10
    {
        private bool m_part2 = false;
        private AOCGrid m_grid = null;

        public Day10(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            Day10Graph graph = new Day10Graph();
            graph.PopulateGraph(m_grid);

            total = graph.Calculate(m_grid, true);

            return total;
        }

        public long Calculate2()
        {
            long total = 0;
            Day10Graph graph = new Day10Graph();
            graph.PopulateGraph(m_grid);

            total = graph.Calculate(m_grid, false);

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
