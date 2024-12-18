using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOCShared
{
    public class DjikstraNode
    {
        public Coordinate Coord { get; set; } = new Coordinate();
        public long Distance { get; set; } = long.MaxValue;
        public Direction Direction { get; set; } = Direction.Unknown;

        public DjikstraNode()
        {
            Coord = new Coordinate() { X = 0, Y = 0 };
            Distance = 0;
            Direction = Direction.Unknown;
        }

        public virtual void SetFrom(DjikstraNode rhs)
        {
            Coord = new Coordinate(rhs.Coord);
            Distance = long.MaxValue;
            Direction = rhs.Direction;
        }

        public override int GetHashCode()
        {
            //return (Coord.X * 1000000) + (Coord.Y * 1000);
            return (int)((Coord.X * 1000000) + (Coord.Y * 1000) + (Convert.ToInt32(Direction) * 100));
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as DjikstraNode);
        }

        public bool Equals(DjikstraNode obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }

    }

    public class DjikstraAlgorithm<T> where T : DjikstraNode, new()
    {
        public AOCGrid m_Grid = null;
        public Queue<T> NodeQueue = new Queue<T>();
        public Dictionary<T, long> VisitedCache = new Dictionary<T, long>();

        private bool m_numericWeighted = false;
        private T m_startPosition { get; set; } = null;
        private T m_endPosition { get; set; } = null;

        public char WallCharacter { get; set; } = '#';

        public DjikstraAlgorithm(AOCGrid grid, bool numericWeighted)
        {
            m_Grid = grid;

            if (numericWeighted)
            {
                m_numericWeighted = true;
                m_Grid.ConvertToIntegers();
            }
        }

        public DjikstraAlgorithm(string fileName, bool numericWeighted)
        {
            m_Grid = new AOCGrid(fileName);

            if (numericWeighted)
            {
                m_numericWeighted = true;
                m_Grid.ConvertToIntegers();
            }
        }

        public virtual long CalculateFromCoords(Coordinate startPosition = null, Coordinate endPosition = null)
        {
            T startNode = null;
            if (startPosition != null)
            {
                startNode = new T();
                startNode.Coord = startPosition;
                startNode.Direction = Direction.Unknown;
                startNode.Distance = 0;
            }

            T endNode = null;
            if (endPosition != null)
            {
                endNode = new T();
                endNode.Coord = endPosition;
                endNode.Direction = Direction.Unknown;
                endNode.Distance = 0;
            }

            return Calculate(startNode, endNode);
        }

        public virtual long Calculate(T startPosition = null, T endPosition = null)
        {
            long total = 0;

            if (startPosition == null)
            {
                startPosition = new T();
                startPosition.Coord = new Coordinate(0, 0);
                startPosition.Direction = Direction.Unknown;
                startPosition.Distance = 0;
            }
            m_startPosition = startPosition;


            if (endPosition == null)
            {
                endPosition = new T();
                endPosition.Coord = new Coordinate(m_Grid.GridWidth - 1, m_Grid.GridHeight - 1);
                endPosition.Direction = Direction.Unknown;
            }
            m_endPosition = endPosition;


            NodeQueue.Enqueue(startPosition);

            while (NodeQueue.Count > 0)
            {
                T thisNode = NodeQueue.Dequeue();

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
                    T newNode = new T();
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

        public virtual void DoProcessing(T currentNode, T nextNode)
        {
            if (IsValidNode(currentNode, nextNode))
            {
                long value = 1;
                
                if (m_numericWeighted)
                {
                    value = m_Grid.Get(nextNode.Coord);
                }
                nextNode.Distance = currentNode.Distance + value;

                NodeQueue.Enqueue(nextNode);
            }
        }

        public virtual bool IsValidNode(T currentNode, T nextNode)
        {
            if (m_Grid.Get(nextNode.Coord) == WallCharacter)
            {
                return false;
            }
            return true;
        }

        public virtual long GetResult()
        {
            long total = long.MaxValue;
            foreach (var val in VisitedCache.Keys.Where(x => x.Coord.Equals(m_endPosition.Coord)))
            {
                total = Math.Min(VisitedCache[val], total);
            }

            return total;
        }
    }
}
