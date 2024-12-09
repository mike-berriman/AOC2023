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

            TraversalPosition pos = m_grid.FindStart();

            bool finished = false;
            while (!finished)
            {
                char nextVal = m_grid.PeekNext(pos);

                if (nextVal == (char)0)
                {
                    break;
                }
                else if (nextVal.Equals('#'))
                {
                    pos.Dir = DirectionExtensions.TurnRight(pos.Dir);
                }
                else
                {
                    if (nextVal.Equals('.'))
                    {
                        total++;
                    }

                    pos.MoveNext();
                    m_grid.Set(pos.Coord, 'X');
                }
            }

            return total;
        }

        private List<TraversalPosition> GetPath()
        {
            List<TraversalPosition> returnList = new List<TraversalPosition>();

            AOCGrid tempGrid = new AOCGrid(m_grid);

            TraversalPosition pos = tempGrid.FindStart();

            bool finished = false;
            while (!finished)
            {
                char nextVal = m_grid.PeekNext(pos);

                if (nextVal == (char)0)
                {
                    break;
                }
                else if (nextVal.Equals('#'))
                {
                    pos.Dir = DirectionExtensions.TurnRight(pos.Dir);
                }
                else
                {
                    returnList.Add(new TraversalPosition(pos));

                    pos.MoveNext();
                    tempGrid.Set(pos.Coord, 'X');
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

            TraversalPosition startPos = m_grid.FindStart();
            Coordinate startCoord = startPos.Coord;
            Direction startDir = startPos.Dir;
            Coordinate firstCoord = startCoord.MoveCopy(startDir, 1);

            var fullPath = GetPath();

            List<Coordinate> stopCoords = new List<Coordinate>();

            int count = 0;
            foreach (var path in fullPath)
            {
                count++;
                AOCGrid tempGrid = new AOCGrid(m_grid);
                Coordinate coord = new Coordinate(startCoord);
                Direction dir = startDir;

                Coordinate testCoord = path.Coord.MoveCopy(path.Dir, 1);

                if (testCoord.Equals(firstCoord))
                {
                    continue;
                }

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
                        finished = true;
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
                            break;
                        }

                        tempGrid.Set(newCoord, 'X');
                        coord = newCoord;
                        traverseList.Add(new TraversalPosition(coord, dir));
                    }
                }

                if (!finished)
                {
                    if (!stopCoords.Contains(testCoord))
                    {
                        stopCoords.Add(new Coordinate(testCoord));
                        total++;
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
