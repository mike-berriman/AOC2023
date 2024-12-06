using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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

    // Define an extension method in a non-nested static class.
    public static class DirectionExtensions
    {
        public static Direction TurnLeft(Direction dir)
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

        public static Direction TurnRight(Direction dir)
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

        public static char ToChar(Direction dir)
        {
            switch (dir)
            {
                case Direction.East:
                    return '>';
                case Direction.West:
                    return '<';
                case Direction.North:
                    return '^';
                case Direction.South:
                    return 'v';
            }

            return ' ';

        }

        public static bool IsOppositeDirection(Direction dir1, Direction dir2)
        {
            bool reverse = false;
            switch (dir1)
            {
                case Direction.East:
                    if (dir2 == Direction.West)
                    {
                        reverse = true;
                    }
                    break;
                case Direction.West:
                    if (dir2 == Direction.East)
                    {
                        reverse = true;
                    }
                    break;
                case Direction.North:
                    if (dir2 == Direction.South)
                    {
                        reverse = true;
                    }
                    break;
                case Direction.South:
                    if (dir2 == Direction.North)
                    {
                        reverse = true;
                    }
                    break;
            }

            return reverse;
        }

    }

    public class Coordinate : IEquatable<Coordinate>
    {
        public long X { get; set; } = 0;
        public long Y { get; set; } = 0;

        public Coordinate()
        {

        }

        public Coordinate(long x, long y)
        {
            X = x;
            Y = y;
        }

        public Coordinate(Coordinate rhs)
        {
            X = rhs.X;
            Y = rhs.Y;
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

        public Coordinate MoveCopy(Direction CurrentDirection, long distance)
        {
            Coordinate c = new Coordinate(this);
            switch (CurrentDirection)
            {
                case Direction.East:
                    c.X += distance;
                    break;
                case Direction.North:
                    c.Y -= distance;
                    break;
                case Direction.West:
                    c.X -= distance;
                    break;
                case Direction.South:
                    c.Y += distance;
                    break;
            }

            return c;
        }

        public Coordinate Move(Direction CurrentDirection, long distance)
        {
            switch (CurrentDirection)
            {
                case Direction.East:
                    X += distance;
                    break;
                case Direction.North:
                    Y -= distance;
                    break;
                case Direction.West:
                    X -= distance;
                    break;
                case Direction.South:
                    Y += distance;
                    break;
            }

            return this;
        }

        public override int GetHashCode()
        {
            return (int)((X * 1000000) + (Y * 10));
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(Coordinate other)
        {
            if (GetHashCode() == other.GetHashCode())
            {
                return true;
            }

            return false;
        }
    }

    public class TraversalPosition : IEquatable<TraversalPosition>
    {
        public Coordinate Coord { get; set; } = null;
        public Direction Dir { get; set; } = Direction.Unknown;

        public TraversalPosition()
        {

        }

        public TraversalPosition(Coordinate coord, Direction dir)
        {
            Coord = coord;
            Dir = dir;
        }

        public TraversalPosition(long x, long y, Direction dir)
        {
            Coord = new Coordinate(x, y);
            Dir = dir;
        }

        public override int GetHashCode()
        {
            return (int)(Coord.GetHashCode() + (int)Dir);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Coordinate);
        }

        public bool Equals(TraversalPosition? other)
        {
            if (other != null)
            {
                if (GetHashCode() == other.GetHashCode())
                {
                    return true;
                }
            }

            return false;
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

        public void MultiplyGrid(int number)
        {
            List<char[]> newGrid = new List<char[]>();

            for (int i = 0; i < number; i++)
            {
                foreach (char[] line in Grid)
                {
                    char[] newLine = Enumerable.Repeat(line, number).SelectMany(arr => arr).ToArray();
                    newGrid.Add(newLine);
                }
            }

            Grid = newGrid;
            GridWidth = Grid[0].Length;
            GridHeight = Grid.Count;
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

        public (Direction, Coordinate) FindStart()
        {
            for (int i = 0; i < Grid.Count; i++)
            {
                char[] line = Grid[i];
                for (int j = 0; j < line.Length; j++)
                {
                    Coordinate coord = new Coordinate(j, i);
                    Direction dir = Direction.East;
                    if (line[j] == '^')
                    {
                        dir = Direction.North;
                        return (dir, coord);
                    }
                    if (line[j] == 'v')
                    {
                        dir = Direction.South;
                        return (dir, coord);
                    }
                    if (line[j] == '>')
                    {
                        dir = Direction.East;
                        return (dir, coord);
                    }
                    if (line[j] == '<')
                    {
                        dir = Direction.West;
                        return (dir, coord);
                    }

                }
            }

            return (Direction.East, null);

        }

        public bool IsOutside(Coordinate CurrentCoordinate)
        {
            bool finished = false;

            if (((CurrentCoordinate.X < 0) || (CurrentCoordinate.X >= GridWidth)) ||
                ((CurrentCoordinate.Y < 0) || (CurrentCoordinate.Y >= GridHeight)))
            {
                finished = true;
            }

            return finished;
        }

        public bool MoveNext(Coordinate CurrentCoordinate, Direction CurrentDirection, long distance = 1)
        {
            bool finished = false;

            CurrentCoordinate.Move(CurrentDirection, distance);

            if (((CurrentCoordinate.X < 0) || (CurrentCoordinate.X >= GridWidth)) ||
                ((CurrentCoordinate.Y < 0) || (CurrentCoordinate.Y >= GridHeight)))
            {
                finished = true;
            }

            return finished;
        }

        public void Set(Coordinate coord, char value)
        {
            Grid[(int)coord.Y][coord.X] = value;
        }

        public char Get(Coordinate coord)
        {
            return Grid[(int)coord.Y][coord.X];
        }

        public char Get(int x, int y)
        {
            return Grid[x][y];
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
