using AOCShared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class MemoizeCache: IEquatable<MemoizeCache>
    {
        public long Value { get; set; } = 0;
        public int RecursionLevel { get; set; } = 0;

        public MemoizeCache(long value, int recursionLevel)
        {
            Value = value;
            RecursionLevel = recursionLevel;
        }

        public override int GetHashCode()
        {
            return (int)((Value * 100) + RecursionLevel );
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as MemoizeCache);
        }

        public bool Equals(MemoizeCache obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }
    }

    internal class Day11
    {
        private bool m_part2 = false;
        List<long> stones = new List<long>();
        List<long> newStones = new List<long>();

        Dictionary<MemoizeCache, long> resultCache = new Dictionary<MemoizeCache, long>();

        public Day11(bool part2)
        {
            m_part2 = part2;
        }

        private void changeStone(int index)
        {
            long stoneNum = stones[index];
            string stoneString = stoneNum.ToString();

            if (stoneNum == 0)
            {
                newStones.Add(1);
            }
            else if ((stoneString.Length % 2) == 0)
            {
                // Even
                newStones.Add(Convert.ToInt64(stoneString.Substring(0, stoneString.Length / 2)));
                newStones.Add(Convert.ToInt64(stoneString.Substring(stoneString.Length / 2)));
            }
            else
            {
                newStones.Add(stoneNum * 2024);
            }
        }

        private List<long> ChangeStoneRecursive(long stoneNum)
        {
            List<long> changedValues = new List<long>();
            string stoneString = stoneNum.ToString();

            if (stoneNum == 0)
            {
                changedValues.Add(1);
            }
            else if ((stoneString.Length % 2) == 0)
            {
                // Even
                changedValues.Add(Convert.ToInt64(stoneString.Substring(0, stoneString.Length / 2)));
                changedValues.Add(Convert.ToInt64(stoneString.Substring(stoneString.Length / 2)));
            }
            else
            {
                changedValues.Add(stoneNum * 2024);
            }

            return changedValues;
        }

        public long CalculateRecursive(long stoneValue, int recurseCount)
        {
            long total = 0;
            if (recurseCount == 75)
            {
                return 1;
            }
            else
            {
                MemoizeCache cacheVal = new MemoizeCache(stoneValue, recurseCount);
                if (resultCache.ContainsKey(cacheVal))
                {
                    return resultCache[cacheVal];
                }
                else
                {
                    List<long> changedValues = new List<long>();
                    string stoneString = stoneValue.ToString();

                    if (stoneValue == 0)
                    {
                        long value = CalculateRecursive(1, recurseCount + 1);
                        resultCache[cacheVal] = value;
                        return value;
                    }
                    else if ((stoneString.Length % 2) == 0)
                    {
                        // Even
                        long value1 = CalculateRecursive(Convert.ToInt64(stoneString.Substring(0, stoneString.Length / 2)), recurseCount + 1);
                        long value2 = CalculateRecursive(Convert.ToInt64(stoneString.Substring(stoneString.Length / 2)), recurseCount + 1);

                        resultCache[cacheVal] = value1 + value2;
                        return value1+value2;
                    }
                    else
                    {
                        long value = CalculateRecursive(stoneValue*2024, recurseCount + 1);
                        resultCache[cacheVal] = value;
                        return value;
                    }
                }
            }

            return total;
        }

        public long Calculate1()
        {
            long total = 0;

            int numBlinks = 25;
            for (int i = 0; i < numBlinks; i++)
            {
                newStones = new List<long>();

                for (int j = 0; j < stones.Count; j++)
                {
                    changeStone(j);
                }

                stones = newStones;
            }

            return stones.Count;
        }

        public long Calculate2()
        {
            long total = 0;

            for (int j = 0; j < stones.Count; j++)
            {
                total += CalculateRecursive(stones[j], 0);
            }

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    stones = StringLibraries.GetListOfInts(line, ' ');
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
