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
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "DjikstraBasic.txt"), true);

            long total = alg.Calculate();

            Assert.AreEqual(78, total);
        }

        [Test]
        public void Test2023Day17()
        {
            Day17Algorithm alg = new Day17Algorithm(Path.Combine(DataPath, "DjikstraBasic.txt"));

            long total = alg.Calculate();

            Assert.AreEqual(102, total);
        }

        [Test]
        public void PathFinder()
        {
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "GraphLongestUnweighted.txt"), false);

            long total = alg.CalculateFromCoords(new Coordinate(1,0), new Coordinate(alg.m_Grid.GridWidth-2, alg.m_Grid.GridHeight-1));

            Assert.AreEqual(74, total);
        }

        [Test]
        public void LargePathFinder()
        {
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "LargerGridTest.txt"), false);

            long total = alg.CalculateFromCoords();

            Assert.AreEqual(272, total);
        }

        [Test]
        public void LargePathFinder2()
        {
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "StartEndTest.txt"), false);

            long total = alg.Calculate();

            Assert.AreEqual(576, total);
        }

        [Test]
        public void StartEndTest()
        {
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "StartEndTest.txt"), false);

            Coordinate start = alg.m_Grid.FindAll('S').First();
            Coordinate end = alg.m_Grid.FindAll('E').First();

            long total = alg.CalculateFromCoords(start, end);

            Assert.AreEqual(572, total);
        }

        [Test]
        public void UnsolveableTest()
        {
            DjikstraAlgorithm<DjikstraNode> alg = new DjikstraAlgorithm<DjikstraNode>(Path.Combine(DataPath, "UnsolvableTest.txt"), false);

            long total = alg.Calculate();

            Assert.AreEqual(long.MaxValue, total);
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
            public Day17Algorithm(string fileName) : base(fileName, true)
            {

            }

            public override void DoProcessing(Day17Node thisNode, Day17Node newNode)
            {
                long value = m_Grid.Get(newNode.Coord);
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