using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day17
{
    public class RecursionSettings
    {
        public List<Coordinate> Path = new List<Coordinate>();
        public Coordinate CurrentPos { get; set; }
        public Direction CurrentDirection { get; set; }
        public long CurrentValue { get; set; } = 0;

        public RecursionSettings()
        {

        }

        public override int GetHashCode()
        {
            return (CurrentPos.X * 1000000) + (CurrentPos.Y * 1000) + Convert.ToInt32(CurrentDirection);
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as RecursionSettings);
        }

        public bool Equals(RecursionSettings obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }

        public RecursionSettings(RecursionSettings rhs)
        {
            CurrentPos = new Coordinate(rhs.CurrentPos);
            CurrentDirection = rhs.CurrentDirection;
            CurrentValue = rhs.CurrentValue;

            foreach (Coordinate coord in rhs.Path)
            {
                Path.Add(new Coordinate(coord));
            }
        }

        public long CalculateHeatLoss(AOCGrid grid)
        {
            long total = 0;
            foreach (Coordinate val in Path)
            {
                total += grid.Get(val);
            }

            total -= grid.GetRow(0)[0];

            return total;
        }

        public bool Visited(Coordinate coord)
        {
            foreach (Coordinate val in Path)
            {
                if (val.Equals(coord))
                {
                    return true;
                }
            }

            return false;
        }

        public bool Last3Dir(Direction dirin)
        {
            bool returnVal = false;

            if (Path.Count > 3)
            {
                for (int i = Path.Count-1; i > Path.Count - 4; i--)
                {
                    Direction dir = Path[i-1].DirectionTo(Path[i]);
                    if (dir != dirin)
                    {
                        returnVal = true;
                        break;
                    }
                }
            }
            else
            {
                returnVal = true;
            }


            return returnVal;
        }
    }


    internal class AdventClass
    {
        const long maxValue = 9999999999999;
        private bool m_part2 = false;
        private AOCGrid Grid = null;

        public Coordinate EndPos;
        public long MinHeat = maxValue;

        public Dictionary<RecursionSettings, long> CachedValues = new Dictionary<RecursionSettings, long>();

        public AdventClass(string fileName, bool part2)
        {
            m_part2 = part2;
            Grid = new AOCGrid(fileName);
            Grid.ConvertToIntegers();

            EndPos = new Coordinate()
            {
                X = Grid.GridWidth - 1,
                Y = Grid.GridHeight - 1
            };
        }

        public long Recurse(RecursionSettings settings)
        {
            if (CachedValues.ContainsKey(settings))
            {
                long returnVal = CachedValues[settings] + Grid.Get(settings.CurrentPos) + settings.CurrentValue;
                return returnVal;
            }

            if (settings.CurrentPos.Equals(EndPos))
            {
                long returnVal = settings.CurrentValue + Grid.Get(EndPos);
                return returnVal;

            } 
            else
            {
                settings.Path.Add(settings.CurrentPos);
                settings.CurrentValue += Grid.Get(settings.CurrentPos);

                long minValue = maxValue;

                Coordinate straight = new Coordinate(settings.CurrentPos);
                if (!Grid.MoveNext(straight, settings.CurrentDirection))
                {
                    // Not already visited
                    if (!settings.Visited(straight))
                    {
                        // Not three in a row
                        if (settings.Last3Dir(settings.CurrentDirection))
                        {
                            RecursionSettings newSettings = new RecursionSettings(settings);
                            newSettings.CurrentPos = straight;
                            minValue = Math.Min(Recurse(newSettings), minValue);
                        }
                    }
                }

                Coordinate left = new Coordinate(settings.CurrentPos);
                Direction newDirection = Grid.TurnLeft(settings.CurrentDirection);
                if (!Grid.MoveNext(left, newDirection))
                {
                    // Not already visited
                    if (!settings.Visited(left))
                    {
                        RecursionSettings newSettings = new RecursionSettings(settings);
                        newSettings.CurrentPos = left;
                        newSettings.CurrentDirection = newDirection;
                        minValue = Math.Min(Recurse(newSettings), minValue);
                    }
                }

                Coordinate right = new Coordinate(settings.CurrentPos);
                newDirection = Grid.TurnRight(settings.CurrentDirection);
                if (!Grid.MoveNext(right, newDirection))
                {
                    // Not already visited
                    if (!settings.Visited(right))
                    {
                        RecursionSettings newSettings = new RecursionSettings(settings);
                        newSettings.CurrentPos = right;
                        newSettings.CurrentDirection = newDirection;
                        minValue = Math.Min(Recurse(newSettings), minValue);
                    }
                }

                CachedValues.Add(settings, minValue - settings.CurrentValue);
                return minValue;
            }

            return long.MaxValue;
        }

        public long Calculate1()
        {
            long total = 0;

            RecursionSettings settings = new RecursionSettings();
            settings.CurrentPos = new Coordinate()
            {
                X = 0,
                Y = 0
            };
            settings.CurrentDirection = Direction.East;
            settings.CurrentValue = 0 - Grid.Get(settings.CurrentPos);

            RecursionSettings settings2 = new RecursionSettings();
            settings2.CurrentPos = new Coordinate()
            {
                X = 0,
                Y = 0
            };
            settings2.CurrentDirection = Direction.South;
            settings2.CurrentValue = 0 - Grid.Get(settings2.CurrentPos);

            long east = Recurse(settings);

            CachedValues.Clear();
            long south = Recurse(settings2);
            total = Math.Min(east, south);

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


    internal class Day17
    {
        List<AdventClass> inputObjects = new List<AdventClass>();

        internal void ProcessInput(string fileName, bool part2)
        {
            // READER code here
            AdventClass av = new AdventClass(fileName, part2);
            inputObjects.Add(av);
        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            foreach (var obj in inputObjects)
            {
                total += obj.Calculate();
            }

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            foreach (var obj in inputObjects)
            {
                total += obj.Calculate();
            }

            return total;
        }


        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            ProcessInput(fileName, part2);

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
