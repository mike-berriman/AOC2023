using AOCShared;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class Day20Node : IEqualityComparer<Day20Node>
    {
        public Coordinate Coord { get; set; } = new Coordinate();
        public long Distance { get; set; } = long.MaxValue;
        public Direction Direction { get; set; } = Direction.Unknown;

        public Coordinate StartCheat { get; set; } = null;
        public Coordinate EndCheat { get; set; } = null;
        public long CheatLength { get; set; } = 0;


        public void SetFrom(Day20Node rhs)
        {
            Coord = new Coordinate(rhs.Coord);
            Distance = long.MaxValue;
            Direction = rhs.Direction;

            StartCheat = rhs.StartCheat;
            EndCheat = rhs.EndCheat;
            CheatLength = rhs.CheatLength;
        }

        public override int GetHashCode()
        {
            //return (Coord.X * 1000000) + (Coord.Y * 1000);
            return (int)((Coord.X * 1000000) + (Coord.Y * 1000) + (Convert.ToInt32(Direction) * 10) + CheatLength);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Day20Node);
        }

        public long GetTestCode()
        {
            return (long)((Coord.X * 1000000) + (Coord.Y * 1000) + (Convert.ToInt32(Direction) * 10) + CheatLength);
        }

        public bool Equals(Day20Node obj)
        {
            bool result = (obj != null && obj.GetTestCode() == this.GetTestCode());
            if (result)
            {
                bool startEquals = false;
                bool endEquals = false;

                if ((obj.StartCheat == null) && (StartCheat == null))
                {
                    startEquals = true;
                } 
                else if ((obj.StartCheat != null) != (StartCheat != null))
                {
                    startEquals = false;
                }
                else if (StartCheat.Equals(obj.StartCheat))
                {
                    startEquals = true;
                }

                if ((obj.EndCheat == null) && (EndCheat == null))
                {
                    endEquals = true;
                }
                else if ((obj.EndCheat != null) != (EndCheat != null))
                {
                    endEquals = false;
                }
                else if (EndCheat.Equals(obj.EndCheat))
                {
                    endEquals = true;
                }

                result = startEquals && endEquals;

            }

            return result;
        }

        public bool Equals(Day20Node? x, Day20Node? y)
        {
            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] Day20Node obj)
        {
            throw new NotImplementedException();
        }
    }

    public class Day20Djikstra 
    {
        public AOCGrid m_Grid = null;
        public Queue<Day20Node> NodeQueue = new Queue<Day20Node>();

        public Dictionary<Day20Node, long> VisitedCache = new Dictionary<Day20Node, long>();

        private bool m_numericWeighted = false;
        protected Day20Node m_startPosition { get; set; } = null;
        protected Day20Node m_endPosition { get; set; } = null;

        public char WallCharacter { get; set; } = '#';

        public int TimeLimit { get; set; } = 20;
        public long UncheatingTime { get; set; } = 0;

        public Day20Djikstra(AOCGrid grid, bool numericWeighted) 
        {
            m_Grid = grid;

            if (numericWeighted)
            {
                m_numericWeighted = true;
                m_Grid.ConvertToIntegers();
            }
        }

        public virtual long CalculateFromCoords(Coordinate startPosition = null, Coordinate endPosition = null)
        {
            Day20Node startNode = null;
            if (startPosition != null)
            {
                startNode = new Day20Node();
                startNode.Coord = startPosition;
                startNode.Direction = Direction.Unknown;
                startNode.Distance = 0;
            }

            Day20Node endNode = null;
            if (endPosition != null)
            {
                endNode = new Day20Node();
                endNode.Coord = endPosition;
                endNode.Direction = Direction.Unknown;
                endNode.Distance = 0;
            }

            return Calculate(startNode, endNode);
        }

        public virtual long Calculate(Day20Node startPosition = null, Day20Node endPosition = null)
        {
            long total = 0;

            if (startPosition == null)
            {
                startPosition = new Day20Node();
                startPosition.Coord = new Coordinate(0, 0);
                startPosition.Direction = Direction.Unknown;
                startPosition.Distance = 0;
            }
            m_startPosition = startPosition;


            if (endPosition == null)
            {
                endPosition = new Day20Node();
                endPosition.Coord = new Coordinate(m_Grid.GridWidth - 1, m_Grid.GridHeight - 1);
                endPosition.Direction = Direction.Unknown;
            }
            m_endPosition = endPosition;


            NodeQueue.Enqueue(startPosition);

            while (NodeQueue.Count > 0)
            {
                Day20Node thisNode = NodeQueue.Dequeue();

                if (VisitedCache.ContainsKey(thisNode))
                {
                    if (VisitedCache[thisNode] > thisNode.Distance)
                    {
                        // Replace the distance measure with the new distance measure
                        VisitedCache.Remove(thisNode);
                        VisitedCache.Add(thisNode, thisNode.Distance);
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    VisitedCache.Add(thisNode, thisNode.Distance);
                }

                if (thisNode.Coord.Equals(endPosition.Coord))
                {
                    continue;
                }

                for (int i = 0; i < 4; i++)
                {
                    Day20Node newNode = new Day20Node();
                    newNode.SetFrom(thisNode);
                    newNode.Distance = long.MaxValue;
                    newNode.Direction = (Direction)i;

                    if (DirectionExtensions.IsOppositeDirection(thisNode.Direction, newNode.Direction))
                    {
                        // Don't look backwards
                        continue;
                    }

                    if (!m_Grid.MoveNext(newNode.Coord, newNode.Direction))
                    {
                        DoProcessing(thisNode, newNode);
                    }
                }
            }

            return GetResult();
        }


        public void DoProcessing(Day20Node currentNode, Day20Node nextNode)
        {
            if (IsValidNode(currentNode, nextNode))
            {
                long value = 1;

                nextNode.Distance = currentNode.Distance + value;

                if (m_Grid.Get(nextNode.Coord) == WallCharacter)
                {
                    if (nextNode.StartCheat != null)
                    {
                        nextNode.CheatLength++;
                    }
                    else
                    {
                        nextNode.StartCheat = nextNode.Coord;
                        nextNode.CheatLength++;
                    }
                }
                else
                {
                    if ((nextNode.StartCheat != null) && (nextNode.EndCheat == null))
                    {
                        nextNode.EndCheat = nextNode.Coord;
                    }
                }

                NodeQueue.Enqueue(nextNode);
            }
        }

        public bool IsValidNode(Day20Node currentNode, Day20Node nextNode)
        {
            if (m_Grid.Get(nextNode.Coord) == WallCharacter)
            {
                if (currentNode.EndCheat != null)
                {
                    // Already used our cheat
                    return false;
                }
                if (currentNode.StartCheat != null)
                {
                    if (currentNode.CheatLength > 19)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        public long GetResult()
        {
            long total = 0;

            Dictionary<Coordinate, List<Coordinate>> cheatCache = new Dictionary<Coordinate, List<Coordinate>>();

            foreach (var val in VisitedCache.Keys.Where(x => x.Coord.Equals(m_endPosition.Coord)))
            {
                bool cahced = false;

                if (val.StartCheat != null)
                {
                    if (cheatCache.ContainsKey(val.StartCheat))
                    {
                        if (cheatCache[val.StartCheat].Contains(val.EndCheat))
                        {
                            cahced = true;
                        }
                    }
                }

                if (!cahced)
                {
                    if ((val.Distance - UncheatingTime) >= TimeLimit)
                    {
                        total += 1;
                        cheatCache.Add(val.StartCheat, new List<Coordinate>() { val.EndCheat });
                    }
                }
            }

            return total;
        }

    }

    internal class Day20
    {
        private bool m_part2 = false;
        AOCGrid m_grid = null;

        public Day20(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;
            
            List<Coordinate> coords = m_grid.FindAll('#');
            Coordinate startCoord = m_grid.FindAll('S').First();
            Coordinate endCoord = m_grid.FindAll('E').First();

            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(m_grid, false);
            long uncheating = alg.CalculateFromCoords(startCoord, endCoord);

            int count = coords.Count;
            Parallel.ForEach(coords, coord =>
            {
                AOCGrid newGrid = new AOCGrid(m_grid);
                newGrid.Set(coord, '.');

                DjikstraAlgorithm<DjikstraNode> newAlg = new DjikstraAlgorithm<DjikstraNode>(newGrid, false);
                long newVal = newAlg.CalculateFromCoords(startCoord, endCoord);

                if (uncheating - newVal >= 100)
                {
                    Interlocked.Add(ref total, 1);
                }

                Console.WriteLine(count--);
            });

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            Coordinate startCoord = m_grid.FindAll('S').First();
            Coordinate endCoord = m_grid.FindAll('E').First();

            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(m_grid, false);
            long uncheating = alg.CalculateFromCoords(startCoord, endCoord);


            Day20Djikstra algCheating = new Day20Djikstra(m_grid, false);
            algCheating.TimeLimit = 20;
            algCheating.UncheatingTime = uncheating;

            total = algCheating.CalculateFromCoords(startCoord, endCoord);


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

