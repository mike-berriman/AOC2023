using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day8
    {
        private bool m_part2 = false;

        AOCGrid m_inputGrid = null;
        AOCGrid m_outputGrid = null;

        public Day8(bool part2)
        {
            m_part2 = part2;
        }

        private Dictionary<char, List<Coordinate>> pairLists = new Dictionary<char, List<Coordinate>>();

        public long Calculate1()
        {
            long total = 0;

            m_outputGrid = new AOCGrid(m_inputGrid, true);
            pairLists = m_inputGrid.GetCoordinatesOfDuplicateValues();

            foreach (var val in pairLists)
            {

                for (int i = 0; i < val.Value.Count; i++)
                {
                    for (int j = i + 1; j < val.Value.Count; j++)
                    {
                        long xDiff = val.Value[i].X - val.Value[j].X;
                        long yDiff = val.Value[i].Y - val.Value[j].Y;

                        Coordinate antinode = new Coordinate(val.Value[i].X - (2 * xDiff), val.Value[i].Y - (2 * yDiff));
                        if (!m_inputGrid.IsOutside(antinode))
                        {
                            m_outputGrid.Set(antinode, '#');
                        }

                        antinode = new Coordinate(val.Value[j].X + (2 * xDiff), val.Value[j].Y + (2 * yDiff));
                        if (!m_inputGrid.IsOutside(antinode))
                        {
                            m_outputGrid.Set(antinode, '#');
                        }
                    }

                }
            }

            return m_outputGrid.CountValue('#');
        }

        public long Calculate2()
        {
            long total = 0;

            m_outputGrid = new AOCGrid(m_inputGrid, true);
            pairLists = m_inputGrid.GetCoordinatesOfDuplicateValues();

            foreach (var val in pairLists)
            {

                for (int i = 0; i < val.Value.Count; i++)
                {
                    for (int j = i + 1; j < val.Value.Count; j++)
                    {
                        long xDiff = val.Value[i].X - val.Value[j].X;
                        long yDiff = val.Value[i].Y - val.Value[j].Y;

                        bool finished = false;
                        Coordinate startCoord = new Coordinate(val.Value[i].X, val.Value[i].Y);
                        while (!finished)
                        {
                            startCoord = new Coordinate(startCoord.X - (xDiff), startCoord.Y - (yDiff));
                            if (!m_inputGrid.IsOutside(startCoord))
                            {
                                m_outputGrid.Set(startCoord, '#');
                            }
                            else
                            {
                                finished = true;
                            }
                        }

                        finished = false;
                        startCoord = new Coordinate(val.Value[j].X, val.Value[j].Y);
                        while (!finished)
                        {
                            startCoord = new Coordinate(startCoord.X + (xDiff), startCoord.Y + (yDiff));
                            if (!m_inputGrid.IsOutside(startCoord))
                            {
                                m_outputGrid.Set(startCoord, '#');
                            }
                            else
                            {
                                finished = true;
                            }
                        }
                    }
                }
            }

            return m_outputGrid.CountValue('#');
        }


        internal void ProcessSingleInput(string fileName)
        {
            m_inputGrid = new AOCGrid(fileName);
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
