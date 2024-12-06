using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day6
    {
        private bool m_part2 = false;
        private AOCGrid m_grid = null;

        public Day6(bool part2)
        {
            m_part2 = part2;
        }

        // Find number of visited nodes
        public long Calculate1()
        {
            long total = 1;

            (Direction dir, Coordinate coord) = m_grid.FindStart();

            bool finished = false;
            while (!finished)
            {
                Coordinate newCoord = coord.MoveCopy(dir, 1);

                if (m_grid.IsOutside(newCoord))
                {
                    break;
                }
                else if (m_grid.Get(newCoord).Equals('#'))
                {
                    dir = DirectionExtensions.TurnRight(dir);
                }
                else
                {
                    if (m_grid.Get(newCoord).Equals('.'))
                    {
                        total++;
                    }

                    m_grid.Set(newCoord, 'X');
                    coord = newCoord;

                }
            }

            return total;
        }

        private List<TraversalPosition> GetPath()
        {
            List<TraversalPosition> returnList = new List<TraversalPosition>();

            AOCGrid tempGrid = new AOCGrid(m_grid);

            (Direction dir, Coordinate coord) = tempGrid.FindStart();

            bool finished = false;
            while (!finished)
            {
                Coordinate newCoord = coord.MoveCopy(dir, 1);

                if (tempGrid.IsOutside(newCoord))
                {
                    break;
                }
                else if (tempGrid.Get(newCoord).Equals('#'))
                {
                    dir = DirectionExtensions.TurnRight(dir);
                }
                else
                {
                    returnList.Add(new TraversalPosition(newCoord, dir));

                    tempGrid.Set(newCoord, 'X');
                    coord = newCoord;

                }
            }

            return returnList;
        }

        // Calculate the number of cycles if we add extra blocks
        // Add a new block in front of each step in the path from part 1
        public long Calculate2()
        {
            long total = 0;
            List<Coordinate> extraObstacles = new List<Coordinate>();

            (Direction startDir, Coordinate startCoord) = m_grid.FindStart();

            var fullPath = GetPath();
            fullPath.RemoveAt(0);

            foreach (var path in fullPath)
            {
                AOCGrid tempGrid = new AOCGrid(m_grid);
                Coordinate coord = new Coordinate(startCoord);
                Direction dir = startDir;

                Coordinate testCoord = path.Coord.MoveCopy(path.Dir, 1);
                if (tempGrid.IsOutside(testCoord))
                {
                    continue;
                }

                if (tempGrid.Get(testCoord) == '.')
                {
                    tempGrid.Set(testCoord, '#');
                }
                else
                {
                    continue;
                }

                bool finished = false;
                List<TraversalPosition> traverseList = new List<TraversalPosition>();

                while (!finished)
                {
                    Coordinate newCoord = coord.MoveCopy(dir, 1);

                    if (tempGrid.IsOutside(newCoord))
                    {
                        break;
                    }
                    else if (tempGrid.Get(newCoord).Equals('#'))
                    {
                        dir = DirectionExtensions.TurnRight(dir);
                    }
                    else
                    {
                        if (traverseList.Contains(new TraversalPosition(newCoord, dir)))
                        {
                            total++;
                            break;
                        }

                        tempGrid.Set(newCoord, 'X');
                        coord = newCoord;
                        traverseList.Add(new TraversalPosition(coord, dir));
                    }
                }

            }

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
