using AOCShared;

namespace AOCSharedTest
{
    public class DjikstraTests
    {
        private const string DataPath = @"D:\temp\advent\AOCSharedTest\Data";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ShortestWeightedPath()
        {
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "DjikstraBasic.txt"));

            DjikstraNode rootNode = new DjikstraNode()
            {
                Coord = new Coordinate() { X = 0, Y = 0 },
                Distance = 0,
                Direction = Direction.Unknown,
            };

            long total = alg.Calculate(rootNode);

            Assert.AreEqual(78, total);
        }

        [Test]
        public void Test2023Day17()
        {
            Day17Algorithm alg = new Day17Algorithm(Path.Combine(DataPath, "DjikstraBasic.txt"));

            Day17Node rootNode = new Day17Node()
            {
                Coord = new Coordinate() { X = 0, Y = 0 },
                Distance = 0,
                Direction = Direction.Unknown,
                StepsInDirection = 0
            };

            long total = alg.Calculate(rootNode);

            Assert.AreEqual(102, total);
        }

        public class Day17Node : DjikstraNode
        {
            public int StepsInDirection { get; set; }

            public override void SetFrom(DjikstraNode rhs)
            {
                Coord = new Coordinate(rhs.Coord);
                Distance = long.MaxValue;
                Direction = rhs.Direction;

                StepsInDirection = (rhs as Day17Node).StepsInDirection;
            }

            public override int GetHashCode()
            {
                //return (Coord.X * 1000000) + (Coord.Y * 1000);
                return (int)((Coord.X * 1000000) + (Coord.Y * 1000) + (Convert.ToInt32(Direction) * 100) + StepsInDirection);
            }
        }

        public class Day17Algorithm : DjikstraAlgorithm<Day17Node>
        {
            public Day17Algorithm(string fileName) : base(fileName)
            {

            }

            public override void DoProcessing(Day17Node thisNode, Day17Node newNode)
            {
                long value = Weights.Get(newNode.Coord);
                newNode.Distance = thisNode.Distance + value;

                if (newNode.Direction == thisNode.Direction)
                {
                    newNode.StepsInDirection++;
                }
                else
                {
                    newNode.StepsInDirection = 1;
                }

                if (newNode.StepsInDirection > 3)
                {
                    return;
                }

                NodeQueue.Enqueue(newNode);
            }

        }


    }
}