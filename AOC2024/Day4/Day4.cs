using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day4
    {
        private bool m_part2 = false;
        private Regex regex = null;
        AOCGrid m_grid = null;

        public Day4(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            WordSearch search = new WordSearch(m_grid, "XMAS");

            total += search.TestHorizontal(true);
            total += search.TestVertical(true);
            total += search.TestDiagonals(true);

            return total;
        }

        public int CheckSurrounds(int line, int pos)
        {
            int count = 0;

            if ((line >= 1) && (pos >= 1) && (line < m_grid.GridHeight-1) && (pos < m_grid.GridWidth-1))
            {
                if ((m_grid.Get(line - 1, pos-1) == 'M') && (m_grid.Get(line + 1,pos + 1) == 'S'))
                {
                    count++;
                }
                if ((m_grid.Get(line - 1,pos - 1) == 'S') && (m_grid.Get(line + 1,pos + 1) == 'M'))
                {
                    count++;
                }
                if ((m_grid.Get(line - 1,pos + 1) == 'S') && (m_grid.Get(line + 1,pos - 1) == 'M'))
                {
                    count++;
                }
                if ((m_grid.Get(line - 1,pos + 1) == 'M') && (m_grid.Get(line + 1,pos - 1) == 'S'))
                {
                    count++;
                }
            }

            if (count == 2)
            {
                count = 1;
            }
            else
            {
                count = 0;
            }

            return count;
        }

        public long Calculate2()
        {
            long total = 0;

            for (int i = 1; i < m_grid.GridHeight; i++)
            {
                int start = 0;
                string row = m_grid.GetRow(i);

                start = row.IndexOf('A');
                while (start >= 0)
                {
                    total += CheckSurrounds(i, start);

                    start = row.IndexOf('A', start+1);
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
