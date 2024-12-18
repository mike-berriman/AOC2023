using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day18
    {
        private bool m_part2 = false;
        private AOCGrid m_grid = null;
        private List<Coordinate> coords = new List<Coordinate>();

        public Day18(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;
            int gridSize = 71;

            m_grid = new AOCGrid(gridSize, gridSize);
            m_grid.Clear('.');
            for (int i = 0; i < 1024; i++)
            {
                m_grid.Set(coords[i], '#');
            }

            DjikstraAlgorithm<DjikstraNode> alg = new AOCShared.DjikstraAlgorithm<DjikstraNode>(m_grid, false);
            long minDist = alg.Calculate();

            return minDist;
        }

        public long Calculate2()
        {
            long total = 0;
            int gridSize = 71;

            m_grid = new AOCGrid(gridSize, gridSize);
            m_grid.Clear('.');

            for (int i = 0; i < 1024; i++)
            {
                m_grid.Set(coords[i], '#');
            }

            Coordinate endCoord = null;
            for (int i = 1025; i < coords.Count; i++)
            {
                m_grid.Set(coords[i], '#');

                DjikstraAlgorithm<DjikstraNode> alg = new AOCShared.DjikstraAlgorithm<DjikstraNode>(m_grid, false);

                long minDist = alg.Calculate();
                if (minDist == long.MaxValue)
                {
                    endCoord = coords[i];
                    break;
                }
            }

            Console.WriteLine(endCoord.X + "," + endCoord.Y);


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
                    List<long> val = StringLibraries.GetListOfInts(line, ',');
                    Coordinate coord = new Coordinate(val[0], val[1]);
                    coords.Add(coord);
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
