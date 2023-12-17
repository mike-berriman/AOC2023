using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public enum Direction
    {
        North,
        East,
        South,
        West,
        Unknown
    };

    public class Coordinate : IEquatable<Coordinate>
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;

        public Coordinate()
        {

        }

        public Coordinate(Coordinate rhs)
        {
            X = rhs.X;
            Y = rhs.Y;
        }

        public bool Equals(Coordinate? other)
        {
            if ((X == other.X) && (Y == other.Y))
            {
                return true;
            }

            return false;
        }

        public Direction DirectionTo(Coordinate rhs)
        {
            if (Y == rhs.Y)
            {
                if (X > rhs.X)
                {
                    return Direction.West;
                }
                else
                {
                    return Direction.East;
                }
            }
            else if (X == rhs.X)
            {
                if (Y > rhs.Y)
                {
                    return Direction.North;
                }
                else
                {
                    return Direction.South;
                }
            }

            return Direction.West;
        }
    }

    public class AOCGrid
    {
        public List<char[]> Grid { get; private set; } = new List<char[]>();
        public int GridWidth { get; private set; } = 0;
        public int GridHeight { get; private set; } = 0;

        public AOCGrid(string filename)
        {
            List<string> allData = StringLibraries.GetAllLines(filename);
            Init(allData);
        }

        public AOCGrid(List<string> allData)
        {
            Init(allData);
        }

        public AOCGrid(List<char[]> allData)
        {
            Init(allData);
        }

        public AOCGrid(AOCGrid allData)
        {
            Init(allData.Grid);
        }

        public void ConvertToIntegers()
        {
            foreach (char[] val in Grid)
            {
                for (int i = 0; i < val.Length; i++)
                {
                    val[i] -= '0';
                }
            }

        }

        private void Init(List<string> allData)
        {
            foreach (string lines in allData)
            {
                Grid.Add(lines.ToArray());
            }

            GridWidth = Grid[0].Length;
            GridHeight = Grid.Count;
        }

        private void Init(List<char[]> allData)
        {
            foreach (char[] lines in allData)
            {
                Grid.Add(lines.ToArray());
            }

            GridWidth = Grid[0].Length;
            GridHeight = Grid.Count;
        }

        public Direction TurnLeft(Direction dir)
        {
            switch (dir)
            {
                case Direction.East:
                    return Direction.North;
                case Direction.West:
                    return Direction.South;
                case Direction.North:
                    return Direction.West;
                case Direction.South:
                    return Direction.East;
            }

            return dir;
        }

        public Direction TurnRight(Direction dir)
        {
            switch (dir)
            {
                case Direction.East:
                    return Direction.South;
                case Direction.West:
                    return Direction.North;
                case Direction.North:
                    return Direction.East;
                case Direction.South:
                    return Direction.West;
            }

            return dir;
        }

        public bool MoveNext(Coordinate CurrentCoordinate, Direction CurrentDirection)
        {
            bool finished = false;
            switch (CurrentDirection)
            {
                case Direction.East:
                    if (CurrentCoordinate.X + 1 >= GridWidth)
                    {
                        finished = true;
                    }
                    else
                    {
                        CurrentCoordinate.X++;
                    }
                    break;
                case Direction.North:
                    if (CurrentCoordinate.Y - 1 < 0)
                    {
                        finished = true;
                    }
                    else
                    {
                        CurrentCoordinate.Y--;
                    }
                    break;
                    break;
                case Direction.West:
                    if (CurrentCoordinate.X - 1 < 0)
                    {
                        finished = true;
                    }
                    else
                    {
                        CurrentCoordinate.X--;
                    }
                    break;
                    break;
                case Direction.South:
                    if (CurrentCoordinate.Y + 1 >= GridHeight)
                    {
                        finished = true;
                    }
                    else
                    {
                        CurrentCoordinate.Y++;
                    }
                    break;
            }

            return finished;
        }

        public void Set(Coordinate coord, char value)
        {
            Grid[coord.Y][coord.X] = value;
        }

        public char Get(Coordinate coord)
        {
            return Grid[coord.Y][coord.X];
        }

        public string GetRow(int row)
        {
            return new string(Grid[row]);
        }

        public string GetColumn(int column)
        {
            return new string(Grid.Select(x => x[column]).ToArray());
        }

        public void TransposeXY()
        {
            List<char[]> rotatedLines = new List<char[]>();

            for (int i = 0; i < GridWidth; i++)
            {
                rotatedLines.Add(Grid.Select(x => x[i]).ToArray());
            }

            Grid = rotatedLines;
        }

        public void RotateAntiClockwise()
        {
            List<char[]> rotatedLines = new List<char[]>();

            for (int i = GridWidth - 1; i >= 0; i--)
            {
                rotatedLines.Add(Grid.Select(x => x[i]).ToArray());
            }

            Grid = rotatedLines;
        }

        public void RotateClockwise()
        {
            List<char[]> rotatedLines = new List<char[]>();

            for (int i = GridWidth - 1; i >= 0; i--)
            {
                rotatedLines.Add(Grid.Select(x => x[i]).Reverse().ToArray());
            }

            Grid = rotatedLines;
        }
    }
}
