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
        public AOCGrid Weights = null;
        public Queue<T> NodeQueue = new Queue<T>();
        public Dictionary<T, long> VisitedCache = new Dictionary<T, long>();

        public DjikstraAlgorithm(string fileName)
        {
            Weights = new AOCGrid(fileName);
            Weights.ConvertToIntegers();
        }

        public long Calculate(T rootNode)
        {
            long total = 0;

            NodeQueue.Enqueue(rootNode);

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

                    if (!Weights.MoveNext(newNode.Coord, newNode.Direction))
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
                long value = Weights.Get(nextNode.Coord);
                nextNode.Distance = currentNode.Distance + value;

                NodeQueue.Enqueue(nextNode);
            }
        }

        public virtual bool IsValidNode(T currentNode, T nextNode)
        {
            return true;
        }

        public virtual long GetResult()
        {
            long total = long.MaxValue;
            foreach (var val in VisitedCache.Keys.Where(x => (x.Coord.X == (Weights.GridWidth - 1)) && (x.Coord.Y == (Weights.GridHeight - 1))))
            {
                total = Math.Min(VisitedCache[val], total);
            }

            return total;
        }
    }
}
