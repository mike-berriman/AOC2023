using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day11
{
    public class Galaxy
    {
        public int xPos { get; set; }
        public int yPos { get; set; }

        public long Distance(Galaxy other)
        {
            return Math.Abs(xPos - other.xPos) + Math.Abs(yPos - other.yPos);
        }


        const int extra = 1000000;
        public long Distance2(Galaxy other, List<int> emptyRows, List<int> emptyCols)
        {
            long total = 0;
            int inc = 1;
            if (other.xPos < xPos)
            {
                inc = -1;
            }
            for (int i = xPos; i != other.xPos; i += inc)
            {
                if (emptyCols.Contains(i))
                {
                    total += extra;
                }
                else
                {
                    total += 1;
                }
            }

            inc = 1;
            if (other.yPos < yPos)
            {
                inc = -1;
            }
            for (int i = yPos; i != other.yPos; i += inc)
            {
                if (emptyRows.Contains(i))
                {
                    total += extra;
                }
                else
                {
                    total += 1;
                }
            }

            return total;
        }
    }

    internal class Day11
    {
        internal void Execute1(string fileName)
        {
            List<string> lines = new List<string>();

            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            List<string> expandedLines = new List<string>();
            foreach (string thisline in lines)
            {
                if (thisline.All(x => x == '.'))
                {
                    expandedLines.Add(thisline);
                }

                expandedLines.Add(thisline);
            }

            List<int> emptyCols = new List<int>();
            for (int i = 0; i < expandedLines[i].Length; i++)
            {
                if (expandedLines.Select(x => x[i]).All(x => x == '.'))
                {
                    emptyCols.Add(i);
                }
            }

            lines = new List<string>();
            foreach(string thisline in expandedLines)
            {
                string newLine = thisline;

                for(int i = emptyCols.Count-1; i >= 0; i--)
                {
                    newLine = newLine.Insert(emptyCols[i], ".");
                }

                lines.Add(newLine);
            }


            List<Galaxy> galaxies = new List<Galaxy>();
            for (int i = 0; i < lines.Count; i++)
            {
                bool finished = false;
                int previousIndex = 0;
                while (!finished)
                {
                    int index = lines[i].IndexOf('#', previousIndex);
                    if (index < 0)
                    {
                        finished = true;
                    }
                    else
                    {
                        galaxies.Add(new Galaxy() { xPos = index, yPos = i });
                        previousIndex = index + 1;
                    }
                }
            }

            for (int i = 0; i < galaxies.Count; i++)
            {
                for ( int j = i; j < galaxies.Count; j++)
                {
                    if (i != j)
                    {
                        total += galaxies[i].Distance(galaxies[j]);
                    }
                }
            }



            Console.WriteLine("1) Result is: " + total);
        }






        internal void Execute2(string fileName)
        {
            List<string> lines = new List<string>();

            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            List<int> emptyLines = new List<int>();
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].All(x => x == '.'))
                {
                    emptyLines.Add(i);
                }
            }

            List<int> emptyCols = new List<int>();
            for (int i = 0; i < lines[0].Length; i++)
            {
                if (lines.Select(x => x[i]).All(x => x == '.'))
                {
                    emptyCols.Add(i);
                }
            }

            List<Galaxy> galaxies = new List<Galaxy>();
            for (int i = 0; i < lines.Count; i++)
            {
                bool finished = false;
                int previousIndex = 0;
                while (!finished)
                {
                    int index = lines[i].IndexOf('#', previousIndex);
                    if (index < 0)
                    {
                        finished = true;
                    }
                    else
                    {
                        galaxies.Add(new Galaxy() { xPos = index, yPos = i });
                        previousIndex = index + 1;
                    }
                }
            }

            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i; j < galaxies.Count; j++)
                {
                    if (i != j)
                    {
                        total += galaxies[i].Distance2(galaxies[j], emptyLines, emptyCols);
                    }
                }
            }
            Console.WriteLine("2) Result is: " + total);
        }

    }
}
