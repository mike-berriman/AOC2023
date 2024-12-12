using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace AOC2024
{
    public class Result
    {
        public List<GraphNode> Coords = new List<GraphNode>();
        public List<Direction> Perimiters { get; set; } = new List<Direction>();
        public long Segments { get; set; } = 0;
        public char Value { get; set; } = ' ';
    }

    internal class Day12
    {
        private bool m_part2 = false;
        AOCGrid inputGrid = null;

        public Day12(bool part2)
        {
            m_part2 = part2;
        }



        public void FindAllContiguous(Result res, UndirectedGraph graph, GraphNode startCoord)
        {
            res.Coords.Add(startCoord);

            res.Perimiters.Add(Direction.North);
            res.Perimiters.Add(Direction.South);
            res.Perimiters.Add(Direction.East);
            res.Perimiters.Add(Direction.West);

            foreach (var neighbour in graph[startCoord])
            {
                res.Perimiters.Remove(startCoord.Coord.DirectionTo(neighbour.Coord));

                if (res.Coords.Contains(neighbour))
                {
                    continue;
                }
                else
                {
                    FindAllContiguous(res, graph, neighbour);
                }
            }
        }


        public long Calculate1()
        {
            long total = 0;
            Dictionary<char, List<Coordinate>> nameValues = inputGrid.GetCoordinatesOfDuplicateValues();
            List<Result> results = new List<Result>();

            foreach (var val in nameValues)
            {
                // Break each set down into groups
                AOCGrid newgrid = new AOCGrid(inputGrid);
                newgrid.ClearExcept(val.Key);


                UndirectedGraph graph = UndirectedGraph.BuildSimplePathGraph(newgrid, '.');
                while (graph.Count > 0)
                {
                    Result res = new Result();

                    var start = graph.First();
                    FindAllContiguous(res, graph, start.Key);

                    foreach (var result in res.Coords)
                    {
                        newgrid.Set(result.Coord, '.');
                    }

                    graph = UndirectedGraph.BuildSimplePathGraph(newgrid, '.');

                    results.Add(res);
                }
            }

            foreach (var result in results)
            {
                total += result.Coords.Count * result.Perimiters.Count;
            }

            
            return total;
        }

        public TraversalPosition MoveAroundPerimiter(AOCGrid grid, TraversalPosition pos, char val)
        {
            TraversalPosition newPos = new TraversalPosition(pos);
            if (grid.PeekNext(pos.Coord, DirectionExtensions.TurnLeft(pos.Dir)) == val)
            {
                newPos.Dir = DirectionExtensions.TurnLeft(pos.Dir);
                grid.MoveNext(newPos.Coord, newPos.Dir);

                return newPos;
            }
            else if (grid.PeekNext(pos.Coord, pos.Dir) == val)
            {
                grid.MoveNext(newPos.Coord, pos.Dir);
                return newPos;
            }
            else if (grid.PeekNext(pos.Coord, DirectionExtensions.TurnRight(pos.Dir)) == val)
            {
                newPos.Dir = DirectionExtensions.TurnRight(pos.Dir);
                grid.MoveNext(newPos.Coord, newPos.Dir);
                return newPos;
            }
            else if (grid.PeekNext(pos.Coord, DirectionExtensions.Reverse(pos.Dir)) == val)
            {
                newPos.Dir = DirectionExtensions.Reverse(pos.Dir);
                grid.MoveNext(newPos.Coord, newPos.Dir);
                return newPos;
            }

            return newPos;
        }

        public void WalkPerimeter(AOCGrid grid, UndirectedGraph graph, Result result, char val)
        {

            GraphNode startNode = null;
            foreach (var res in result.Coords)
            {

                if ((grid.PeekNext(res.Coord, Direction.North) != val) && ((grid.PeekNext(res.Coord, Direction.West) != val)))
                {
                    startNode = res;
                    break;
                }
            }

            if (graph.Count == 1)
            {
                result.Segments = 4;
                return;
            }

            result.Segments = 1;
            if (grid.PeekNext(startNode.Coord, Direction.South) != val)
            {
                result.Segments++;
            }

            // Now walk the perimiter until back to startNode, adding one each time you change direction.
            TraversalPosition startPos = new TraversalPosition(startNode.Coord, Direction.East);

            TraversalPosition lastPos = new TraversalPosition(startPos);
            TraversalPosition nextPos = MoveAroundPerimiter(grid, lastPos, val);
            TraversalPosition firstPos = startPos;

            bool first = true;
            while (!firstPos.Equals(nextPos))
            {
                if (first)
                {
                    firstPos = new TraversalPosition(nextPos);
                    first = false;
                }

                if (lastPos.Dir == DirectionExtensions.TurnLeft(nextPos.Dir))
                {
                    result.Segments += 1;
                }
                else if (DirectionExtensions.IsOppositeDirection(lastPos.Dir, nextPos.Dir))
                {
                    result.Segments += 2;
                }
                else if (lastPos.Dir == DirectionExtensions.TurnRight(nextPos.Dir))
                {
                    result.Segments += 1;
                }

                lastPos = nextPos;
                nextPos = MoveAroundPerimiter(grid, lastPos, val);
            }

            //if (DirectionExtensions.IsOppositeDirection(lastPos.Dir, nextPos.Dir))
            //{
            //    result.Segments += 1;
            //}

            nextPos.ToString();
            //if (lastPos.Dir == DirectionExtensions.TurnLeft(nextPos.Dir))
            //{
            //    result.Segments += 1;
            //}
            //else if (DirectionExtensions.IsOppositeDirection(lastPos.Dir, nextPos.Dir))
            //{
            //    result.Segments += 2;
            //}
            //else if (lastPos.Dir == DirectionExtensions.TurnRight(nextPos.Dir))
            //{
            //    result.Segments += 1;
            //}

        }

        public void RemoveDuplicatePerimiters(UndirectedGraph graph, Result res, GraphNode startCoord)
        {
            foreach (var neighbour in graph[startCoord])
            {

                if (res.Coords.Contains(neighbour))
                {
                    continue;
                }
                else
                {
                    FindAllContiguous(res, graph, neighbour);
                }
            }
        }

        public long Calculate2()
        {
            long total = 0;
            Dictionary<char, List<Coordinate>> nameValues = inputGrid.GetCoordinatesOfDuplicateValues();
            List<Result> results = new List<Result>();

            foreach (var val in nameValues)
            {
                // Break each set down into groups
                AOCGrid newgrid = new AOCGrid(inputGrid);
                newgrid.ClearExcept(val.Key);


                UndirectedGraph graph = UndirectedGraph.BuildSimplePathGraph(newgrid, '.');
                while (graph.Count > 0)
                {
                    Result res = new Result();
                    res.Value = val.Key;

                    var start = graph.First();
                    FindAllContiguous(res, graph, start.Key);

                    RemoveDuplicatePerimiters(graph, res, start.Key);

                    foreach (var result in res.Coords)
                    {
                        newgrid.Set(result.Coord, '.');
                    }

                    graph = UndirectedGraph.BuildSimplePathGraph(newgrid, '.');

                    results.Add(res);
                }
            }

            foreach (var result in results)
            {
                total += result.Coords.Count * result.Segments;
            }


            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            inputGrid = new AOCGrid(fileName);
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
