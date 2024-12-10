using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOCShared
{
    public class WordSearch
    {
        AOCGrid m_grid = null;
        private Regex regex = null;
        private string m_searchString = string.Empty;

        public WordSearch(AOCGrid grid, string searchString)
        {
            m_grid = grid;
            regex = new Regex(searchString);
            m_searchString = searchString;
        }

        public int CountMatches(string line)
        {
            return regex.Matches(line).Count;
        }

        public int TestLine(string testLine, bool reverse)
        {
            int count = 0;

            count += CountMatches(testLine);

            if (reverse)
            {
                count += CountMatches(new string(testLine.Reverse().ToArray()));
            }

            return count;
        }

        private int TestLines(AOCGrid grid, bool reverse)
        {
            int count = 0;

            for (int i = 0; i < grid.GridHeight; i++)
            {
                count += TestLine(grid.GetRow(i), reverse);
            }

            return count;
        }

        public int TestHorizontal(bool reverse)
        {
            return TestLines(m_grid, reverse);
        }

        private int TestVertical(AOCGrid grid, bool reverse)
        {
            int count = 0;

            for (int i = 0; i < grid.GridWidth; i++)
            {
                count += TestLine(grid.GetColumn(i), reverse);
            }

            return count;

        }

        public int TestVertical(bool reverse)
        {
            return TestVertical(m_grid, reverse);
        }

        private string GetDiagonal(AOCGrid grid, int diagonalIndex)
        {
            int numLetters = diagonalIndex;
            int startInc = 0;
            string line = string.Empty;

            if (diagonalIndex > grid.GridHeight)
            {
                numLetters = (grid.GridHeight * 2) - diagonalIndex;
                startInc = (diagonalIndex % grid.GridHeight);
            }

            for (int j = numLetters - 1; j >= 0; j--)
            {
                line += grid.Get(diagonalIndex - j - 1 - startInc, j + startInc);
            }

            return line;
        }

        private int TestDiagonalDown(AOCGrid grid, bool reverse)
        {
            int count = 0;
            int startOffset = m_searchString.Length;

            int totalCount = (grid.GridHeight - startOffset) * 2;

            for (int i = startOffset; i < totalCount + startOffset; i++)
            {
                string line = GetDiagonal(grid, i);

                count += TestLine(line, reverse);
            }

            return count;

        }
        private int TestDiagonalUp(AOCGrid grid, bool reverse)
        {
            AOCGrid newGrid = new AOCGrid(grid);
            newGrid.RotateAntiClockwise();

            return TestDiagonalDown(newGrid, reverse);
        }

        public int TestDiagonals(bool reverse)
        {
            int count = TestDiagonalDown(m_grid, reverse);
            count += TestDiagonalUp(m_grid, reverse);

            return count;
        }
    }
}
