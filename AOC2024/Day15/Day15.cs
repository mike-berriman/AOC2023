using AOCShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day15
    {
        private bool m_part2 = false;
        AOCGrid m_grid = null;
        string m_instructions = string.Empty;

        public Day15(bool part2)
        {
            m_part2 = part2;
        }

        public void MoveNext(Coordinate position, char direction)
        {
            Direction dir = DirectionExtensions.FromChar(direction);

            char next = m_grid.PeekNext(position, dir);
            if (next == '#')
            {
                return;
            }

            if (next == '.')
            {
                position.Move(dir, 1);
                return;
            }

            if (next == 'O')
            {
                // Keep moving in the same direction, until we hit a '.' or a '#'
                Coordinate testPosition = new Coordinate(position);
                testPosition.Move(dir);

                char nextVal = m_grid.PeekNext(testPosition, dir);
                while ((nextVal != '#') && (nextVal != '.'))
                {
                    testPosition.Move(dir);
                    nextVal = m_grid.PeekNext(testPosition, dir);
                }

                if (nextVal == '.')
                {
                    position.Move(dir);

                    testPosition.Move(dir);
                    m_grid.Set(testPosition, 'O');

                    Direction reverse = DirectionExtensions.Reverse(dir);

                    while (!testPosition.Equals(position))
                    {
                        testPosition.Move(reverse);

                        if (!testPosition.Equals(position))
                        {
                            m_grid.Set(testPosition, 'O');
                        }
                        else
                        {
                            m_grid.Set(testPosition, '.');
                        }
                    }

                    return;
                }

                if (nextVal == '#')
                {
                    return;
                }
            }
        }

        public long Calculate1()
        {
            long total = 0;

            Coordinate position = m_grid.FindAll('@').First();
            m_grid.Set(position, '.');

            foreach (char instruction in m_instructions)
            {
                MoveNext(position, instruction);

                //AOCGrid temp = new AOCGrid(m_grid);
                //temp.Set(position, '@');
                //temp.PrintToClipboard();
                //temp = null;
            }

            List<Coordinate> allBoxes = m_grid.FindAll('O');
            foreach (Coordinate coord in allBoxes)
            {
                total += (coord.Y * 100) + coord.X;
            }

            return total;
        }

        public void ScaleGrid()
        {
            AOCGrid newGrid = new AOCGrid(m_grid.GridWidth * 2, m_grid.GridHeight);

            for (int y = 0; y < m_grid.GridHeight; y++)
            {
                string row = m_grid.GetRow(y);

                int x = 0;
                foreach (char val in row)
                {
                    if (val == 'O')
                    {
                        newGrid.Set(new Coordinate(x++, y), '[');
                        newGrid.Set(new Coordinate(x++, y), ']');
                    }
                    else if (val == '@')
                    {
                        newGrid.Set(new Coordinate(x++, y), '@');
                        newGrid.Set(new Coordinate(x++, y), '.');
                    }
                    else
                    {
                        newGrid.Set(new Coordinate(x++, y), val);
                        newGrid.Set(new Coordinate(x++, y), val);
                    }
                }
            }

            m_grid = newGrid;
        }

        public bool MoveNext2(Coordinate position, char direction)
        {
            Direction dir = DirectionExtensions.FromChar(direction);

            char next = m_grid.PeekNext(position, dir);
            if (next == '#')
            {
                return false;
            }

            if (next == '.')
            {
                position.Move(dir, 1);
                return false;
            }

            if ((next == '[') || (next == ']'))
            {

                if ((dir == Direction.East) || (dir == Direction.West))
                {
                    // Keep moving in the same direction, until we hit a '.' or a '#'
                    Coordinate testPosition = new Coordinate(position);
                    testPosition.Move(dir);

                    char nextVal = m_grid.PeekNext(testPosition, dir);
                    while ((nextVal != '#') && (nextVal != '.'))
                    {
                        testPosition.Move(dir);
                        nextVal = m_grid.PeekNext(testPosition, dir);
                    }

                    if (nextVal == '.')
                    {
                        position.Move(dir);

                        char lastVal = m_grid.Get(testPosition);
                        testPosition.Move(dir);
                        m_grid.Set(testPosition, lastVal);

                        Direction reverse = DirectionExtensions.Reverse(dir);

                        while (!testPosition.Equals(position))
                        {
                            testPosition.Move(reverse);
                            lastVal = m_grid.PeekNext(testPosition, reverse);

                            if (!testPosition.Equals(position))
                            {
                                m_grid.Set(testPosition, lastVal);
                            }
                            else
                            {
                                m_grid.Set(testPosition, '.');
                            }
                        }

                        return true;
                    }

                    if (nextVal == '#')
                    {
                        return false;
                    }
                }
                else
                {
                    // North/South case is the tricky one
                    MoveNorthSouth(position, dir);
                    return true;
                }
            }

            return false;
        }

        public char FindUltimateMove(List<Coordinate> recursionStack, Direction dir)
        {
            char returnVal = '.';
            List<Coordinate> newRecursionStack = new List<Coordinate>();

            bool foundMoreBoxes = false;
            foreach (Coordinate coord in recursionStack)
            {
                char nextVal = m_grid.PeekNext(coord, dir);
                if (nextVal == '#')
                {
                    return '#';
                }
                if (nextVal == '.')
                {
                    //Nothing here
                    //newRecursionStack.Add(coord.MoveCopy(dir));
                }
                else
                {
                    Coordinate next = coord.MoveCopy(dir);
                    foundMoreBoxes = true;

                    if (!newRecursionStack.Contains(next))
                    {
                        newRecursionStack.Add(next);
                    }
                    if (nextVal == ']')
                    {
                        Coordinate copy = next.MoveCopy(Direction.West);
                        if (!newRecursionStack.Contains(copy))
                        {
                            newRecursionStack.Add(copy);
                        }
                    }
                    else
                    {
                        Coordinate copy = next.MoveCopy(Direction.East);
                        if (!newRecursionStack.Contains(copy))
                        {
                            newRecursionStack.Add(copy);
                        }
                    }
                }
            }

            if (foundMoreBoxes)
            {
                returnVal = FindUltimateMove(newRecursionStack, dir);
            }

            if (returnVal == '.')
            {
                Direction reverse = DirectionExtensions.Reverse(dir);

                // Move all the boxes up
                foreach (Coordinate coord in recursionStack)
                {
                    Coordinate nextCoord = coord.MoveCopy(dir);
                    m_grid.Set(nextCoord, m_grid.Get(coord));
                    m_grid.Set(coord, '.');
                }
            }

            return returnVal;
        }


        public void MoveNorthSouth(Coordinate position, Direction dir)
        {
            List<Coordinate> recursionStack = new List<Coordinate>();

            char nextVal = m_grid.PeekNext(position, dir);

            Coordinate next = position.MoveCopy(dir);
            recursionStack.Add(next);

            if (nextVal == ']')
            {
                next = next.MoveCopy(Direction.West);
                recursionStack.Add(next);
            }
            else
            {
                next = next.MoveCopy(Direction.East);
                recursionStack.Add(next);
            }

            char value = FindUltimateMove(recursionStack, dir);
            if (value == '.')
            {
                m_grid.Set(position, '.');
                position.Move(dir);
            }
        }




        public long Calculate2()
        {
            long total = 0;

            ScaleGrid();

            Coordinate position = m_grid.FindAll('@').First();
            m_grid.Set(position, '.');

            int count = 1;
            foreach (char instruction in m_instructions)
            {
                bool important = MoveNext2(position, instruction);

                AOCGrid temp2 = new AOCGrid(m_grid);
                temp2.Set(position, '@');

                temp2.PrintToConsole("(" + count + ") Instruction : " + instruction);

                count++;
            }

            List<Coordinate> allBoxes = m_grid.FindAll('[');
            foreach (Coordinate coord in allBoxes)
            {
                total += (coord.Y * 100) + coord.X;
            }

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            m_grid = new AOCGrid(fileName);

            fileName = Path.ChangeExtension(fileName, "Instructions.txt");
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    m_instructions += line.Trim();
                }
                else
                {
                    break;
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
