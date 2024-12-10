using AOCShared;

namespace AOCSharedTest
{
    public class Tests
    {
        private const string DataPath = @"D:\temp\advent\AOCSharedTest\Data";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GraphLongestUnweighted()
        {
            AOCGrid grid = new AOCGrid(Path.Combine(DataPath, "GraphLongestUnweighted.txt"));

            UndirectedGraph graph = UndirectedGraph.BuildSimplePathGraph(grid, '#');

            Coordinate start = new Coordinate(1, 0);
            Coordinate end = new Coordinate(grid.GridWidth-2, grid.GridHeight-1);

            Assert.AreEqual(154, graph.FindLongestPath(start, end));
        }

        [Test]
        public void GraphShortestUnweighted()
        {
            AOCGrid grid = new AOCGrid(Path.Combine(DataPath, "GraphLongestUnweighted.txt"));

            UndirectedGraph graph = UndirectedGraph.BuildSimplePathGraph(grid, '#');

            Coordinate start = new Coordinate(1, 0);
            Coordinate end = new Coordinate(grid.GridWidth - 2, grid.GridHeight - 1);

            Assert.AreEqual(154, graph.FindShortestPath(start, end));
        }

        [Test]
        public void GraphShortestWeighted()
        {
            AOCGrid grid = new AOCGrid(Path.Combine(DataPath, "WeightedGraph.txt"));

            UndirectedGraph graph = UndirectedGraph.BuildWeightedGraph(grid);

            Coordinate start = new Coordinate(1, 0);
            Coordinate end = new Coordinate(grid.GridWidth - 2, grid.GridHeight - 1);

            Assert.AreEqual(154, graph.FindShortestPath(start, end));
        }

        [Test]
        public void GraphLongestTest()
        {
            AOCGrid grid = new AOCGrid(Path.Combine(DataPath, "SimplePath.txt"));

            UndirectedGraph graph = UndirectedGraph.BuildSimplePathGraph(grid, '#');

            Coordinate start = new Coordinate(1, 0);
            Coordinate end = new Coordinate(1, grid.GridHeight - 1);

            Assert.AreEqual(154, graph.FindShortestPath(start, end));
        }
        [Test]

        public void GraphShortestTest()
        {
            AOCGrid grid = new AOCGrid(Path.Combine(DataPath, "SimplePath.txt"));

            UndirectedGraph graph = UndirectedGraph.BuildSimplePathGraph(grid, '#');

            Coordinate start = new Coordinate(1, 0);
            Coordinate end = new Coordinate(1, grid.GridHeight - 1);

            Assert.AreEqual(154, graph.FindShortestPath(start, end));
        }

    }
}