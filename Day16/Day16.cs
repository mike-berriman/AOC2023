using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day16
{

    internal class Beam : IEquatable<Beam>
    {
        public Coordinate StartCoord { get; set; }
        public Direction StartDirection { get; set; }

        private Coordinate CurrentCoordinate;
        private Direction CurrentDirection;

        public List<Coordinate> SquaresCrossed { get; set; } = new List<Coordinate>();

        private AOCGrid Grid = null;

        public bool ProcessSquare(List<Beam> newBeams)
        {
            bool finished = false;

            switch (Grid.Get(CurrentCoordinate))
            {
                case '\\':
                    if (CurrentDirection == Direction.East)
                    {
                        CurrentDirection = Direction.South;
                    }
                    else if (CurrentDirection == Direction.West)
                    {
                        CurrentDirection = Direction.North;
                    }
                    else if (CurrentDirection == Direction.North)
                    {
                        CurrentDirection = Direction.West;
                    }
                    else if (CurrentDirection == Direction.South)
                    {
                        CurrentDirection = Direction.East;
                    }
                    break;
                case '/':
                    if (CurrentDirection == Direction.East)
                    {
                        CurrentDirection = Direction.North;
                    }
                    else if (CurrentDirection == Direction.West)
                    {
                        CurrentDirection = Direction.South;
                    }
                    else if (CurrentDirection == Direction.North)
                    {
                        CurrentDirection = Direction.East;
                    }
                    else if (CurrentDirection == Direction.South)
                    {
                        CurrentDirection = Direction.West;
                    }
                    break;
                case '|':
                    if ((CurrentDirection == Direction.East) ||
                        (CurrentDirection == Direction.West))
                    {
                        Beam newBeam = new Beam();
                        newBeam.StartDirection = Direction.North;
                        newBeam.StartCoord = new Coordinate(CurrentCoordinate);
                        newBeams.Add(newBeam);

                        newBeam = new Beam();
                        newBeam.StartDirection = Direction.South;
                        newBeam.StartCoord = new Coordinate(CurrentCoordinate);
                        newBeams.Add(newBeam);

                        finished = true;
                    }

                    break;
                case '-':
                    if ((CurrentDirection == Direction.North) ||
                        (CurrentDirection == Direction.South))
                    {
                        Beam newBeam = new Beam();
                        newBeam.StartDirection = Direction.East;
                        newBeam.StartCoord = new Coordinate(CurrentCoordinate);
                        newBeams.Add(newBeam);

                        newBeam = new Beam();
                        newBeam.StartDirection = Direction.West;
                        newBeam.StartCoord = new Coordinate(CurrentCoordinate);
                        newBeams.Add(newBeam);

                        finished = true;
                    }
                    break;
            }

            return finished;
        }

        public List<Beam> RunBeam(AOCGrid grid)
        {
            List<Beam> newBeams = new List<Beam>();
            CurrentCoordinate = new Coordinate(StartCoord);
            CurrentDirection = StartDirection;

            Grid = grid;

            ProcessSquare(newBeams);

            bool finished = false;
            while (!finished)
            {
                SquaresCrossed.Add(new Coordinate(CurrentCoordinate));

                finished = grid.MoveNext(CurrentCoordinate, CurrentDirection);
                if (!finished)
                {
                    finished = ProcessSquare(newBeams);
                }
            }

            return newBeams;
        }

        public bool Equals(Beam? other)
        {
            if (StartCoord.Equals(other.StartCoord) && (StartDirection == other.StartDirection))
            {
                return true;
            }

            return false;
        }
    }


    internal class AdventClass
    {
        public AOCGrid Grid = null;
        public List<Beam> Beams = new List<Beam>();

        private bool m_part2 = false;

        public AdventClass(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            for (int i = 0; i < Beams.Count; i++)
            {
                List<Beam> newBeams = Beams[i].RunBeam(Grid);

                foreach (Beam beam in newBeams)
                {
                    if (Beams.FirstOrDefault(x => x.Equals(beam)) == null)
                    {
                        Beams.Add(beam);
                    }
                }
            }

            AOCGrid ModifiedGrid = new AOCGrid(Grid);

            foreach (Beam beam in Beams)
            {
                foreach (Coordinate coord in beam.SquaresCrossed)
                {
                    ModifiedGrid.Set(coord, '#');
                }
            }

            foreach (char[] line in ModifiedGrid.Grid)
            {
                total += line.Count(x => x == '#');
            }

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            return total;
        }


        public long Calculate()
        {
            long total = 0;

            if (!m_part2)
            {
                total = Calculate1();
            }
            else
            {
                total = Calculate2();
            }

            return total;
        }
    }


    internal class Day16
    {
        AdventClass inputObjects = null;

        internal void ProcessInput(string fileName, bool part2)
        {
            inputObjects = new AdventClass(part2);
            inputObjects.Grid = new AOCGrid(fileName);
        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            Coordinate startCoord = new Coordinate()
            {
                X = 0,
                Y = 0
            };

            inputObjects.Beams.Add(new Beam()
            {
                StartCoord = startCoord,
                StartDirection = Direction.East
            }); ;

            total += inputObjects.Calculate();

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            List<Beam> starts = new List<Beam>();
            for (int i = 0; i < inputObjects.Grid.GridWidth; i++)
            {
                Coordinate startCoord = new Coordinate()
                {
                    X = i,
                    Y = 0
                };

                starts.Add(new Beam()
                {
                    StartCoord = startCoord,
                    StartDirection = Direction.South
                });

                startCoord = new Coordinate()
                {
                    X = i,
                    Y = inputObjects.Grid.GridHeight-1
                };

                starts.Add(new Beam()
                {
                    StartCoord = startCoord,
                    StartDirection = Direction.North
                });
            }

            for (int i = 0; i < inputObjects.Grid.GridHeight; i++)
            {
                Coordinate startCoord = new Coordinate()
                {
                    X = 0,
                    Y = i
                };

                starts.Add(new Beam()
                {
                    StartCoord = startCoord,
                    StartDirection = Direction.East
                });

                startCoord = new Coordinate()
                {
                    X = inputObjects.Grid.GridWidth-1,
                    Y = i
                };

                starts.Add(new Beam()
                {
                    StartCoord = startCoord,
                    StartDirection = Direction.West
                });
            }

            total = 0;

            foreach (Beam start in starts)
            {
                inputObjects.Beams.Clear();
                inputObjects.Beams.Add(start);

                long subtotal = inputObjects.Calculate();
                if (subtotal > total)
                {
                    total = subtotal;
                }

            }

            return total;
        }


        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            ProcessInput(fileName, false);

            long total;
            if (!part2)
            {
                total = Execute1(fileName);
            }
            else
            {
                total = Execute2(fileName);
            }

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(counter + ") " + "(" + millis + ") Result is: " + total);

        }

    }
}
