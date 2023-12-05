using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Day3
{
    internal class Day3
    {
        //string fileName = @"D:\temp\advent\AdventOfCoding\Day3\TestData1.txt";
        string fileName = @"D:\temp\advent\AdventOfCoding\Day3\InputData.txt";

        List<string> lines = new List<string>();
        List<char> allowed = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };

        private MatchCollection GetIndexes(string line)
        {
            Dictionary<int, int> lineIndexes = new Dictionary<int, int>();
            Regex r = new Regex("[0-9]*");

            return r.Matches(line);
        }

        private bool IsAdjacent(int  lineIndex, int start, int length)
        {
            string line = lines[lineIndex];

            for (int i = start-1; i < start + length + 1; i++)
            {
                if ((i >= 0) && (i < line.Length))
                {
                    if (!allowed.Contains(line[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private int ProcessLine(int i)
        {
            int total = 0;
            string line = lines[i];
            MatchCollection matches = GetIndexes(line);

            foreach (Match m in matches)
            {
                if (m.Value.Length <= 0)
                {
                    continue;
                }
                bool include = false;
                if (IsAdjacent(i, m.Index, m.Length))
                {
                    include = true;
                }
                if ((i > 0) && IsAdjacent(i-1, m.Index, m.Length))
                {
                    include = true;
                }
                if ((i < lines.Count-1) && IsAdjacent(i + 1, m.Index, m.Length))
                {
                    include = true;
                }

                if (include)
                {
                    total += Convert.ToInt32(m.Value);
                }
            }

            return total;
        }

        private int ProcessLines()
        {
            int total = 0;

            for(int i = 0; i < lines.Count; i++)
            {
                total += ProcessLine(i);    
            }

            return total;
        }

        internal void Execute1()
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            total = ProcessLines();

            Console.WriteLine("1) Result is: " + total);
        }




        private bool IsAdjacent2(int lineIndex, int start, int length)
        {
            string line = lines[lineIndex];

            for (int i = start - 1; i < start + length + 1; i++)
            {
                if ((i >= 0) && (i < line.Length))
                {
                    if (!allowed.Contains(line[i]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private List<int> StarIndexes(string line)
        {
            List<int> starIndexes = new List<int>();
            int startIndex = 0;

            do
            {
                int index = line.IndexOf('*', startIndex);
                if (index >= 0)
                {
                    starIndexes.Add(index);
                    startIndex = index + 1;
                }
                else
                {
                    break;
                }
            }
            while (true);

            return starIndexes;
        }

        private bool CheckForAdjacent(Match m, int starIndex)
        {
            if ((starIndex >= (m.Index-1)) && (starIndex < m.Index + m.Length +1))
            {
                return true;
            }
            return false;
        }

        private void CheckForAdjacent(int starIndex, int lineIndex, List<int> adjacentMatches)
        {
            if ((lineIndex >= 0) && (lineIndex < lines.Count))
            {
                MatchCollection matches = GetIndexes(lines[lineIndex]);
                foreach (Match m in matches)
                {
                    if (m.Value.Length <= 0)
                    {
                        continue;
                    }

                    if (CheckForAdjacent(m, starIndex))
                    {
                        adjacentMatches.Add(Convert.ToInt32(m.Value));
                    }
                }
            }

        }

        private int ProcessLine2(int i)
        {
            int total = 0;
            string line = lines[i];

            List<int> starIndexes = StarIndexes(line);
            if (starIndexes.Count == 0)
            {
                return 0;
            }

            foreach (int starIndex in starIndexes)
            {
                List<int> adjacentMatches = new List<int>();

                CheckForAdjacent(starIndex, i, adjacentMatches);
                CheckForAdjacent(starIndex, i-1, adjacentMatches);
                CheckForAdjacent(starIndex, i+1, adjacentMatches);

                if (adjacentMatches.Count > 1)
                {
                    int subTotal = 1;
                    foreach (int val in adjacentMatches)
                    {
                        subTotal *= val;
                    }
                    total += subTotal;
                }
            }


            return total;
        }

        private int ProcessLines2()
        {
            int total = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                total += ProcessLine2(i);
            }

            return total;
        }



        internal void Execute2()
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            lines = new List<string>();
            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            total = ProcessLines2();

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
