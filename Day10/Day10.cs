using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day10
{
    internal class Day10
    {
        public int xPos { get; set; } = 0;
        public int yPos { get; set; } = 0;
        public int offsetX { get; set; } = 0;
        public int offsetY { get; set; } = 0;
        public char StartChar = ' ';

        public void Move(char val)
        {
            switch (val)
            {
                case '|':
                    if (offsetY == 1)
                    {
                        yPos++;
                    }
                    else
                    {
                        yPos--;
                    }
                    break;
                    break;
                case '-':
                    if (offsetX == 1)
                    {
                        xPos++;
                    }
                    else
                    {
                        xPos--;
                    }
                    break;
                case 'L':
                    if (offsetY == 1)
                    {
                        offsetY = 0;
                        offsetX = 1;
                        xPos++;
                    }
                    else
                    {
                        offsetX = 0;
                        offsetY = -1;
                        yPos--;
                    }
                    break;
                case 'J':
                    if (offsetY == 1)
                    {
                        offsetY = 0;
                        offsetX = -1;
                        xPos--;
                    }
                    else
                    {
                        offsetX = 0;
                        offsetY = -1;
                        yPos--;
                    }
                    break;
                case '7':
                    if (offsetX == 1)
                    {
                        offsetY = 1;
                        offsetX = 0;
                        yPos++;
                    }
                    else
                    {
                        offsetX = -1;
                        offsetY = 0;
                        xPos--;
                    }
                    break;
                case 'F':
                    if (offsetY == -1)
                    {
                        offsetY = 0;
                        offsetX = 1;
                        xPos++;
                    }
                    else
                    {
                        offsetX = 0;
                        offsetY = 1;
                        yPos++;
                    }
                    break;
            }
        }

        public void FindStartDirection(List<string> lines, int xStart, int yStart)
        {
            int sides = 0;

            if (lines[yStart].Length > (xStart + 1))
            {
                char c = lines[yStart][xStart + 1];
                if ((c == '-') || (c == 'J') || (c == '7'))
                {
                    // R
                    sides += 1;
                }
            }

            if (xStart > 0)
            {
                char c = lines[yStart][xStart - 1];
                if ((c == '-') || (c == 'L') || (c == 'F'))
                {
                    // L
                    sides += 2;
                }
            }

            if (yStart > 0)
            {
                char c = lines[yStart - 1][xStart];
                if ((c == '|') || (c == '7') || (c == 'F'))
                {
                    // T
                    sides += 4;
                }
            }

            if (lines.Count > (yStart + 1))
            {
                char c = lines[yStart + 1][xStart];
                if ((c == '|') || (c == 'L') || (c == 'J'))
                {
                    // B
                    sides += 8;
                }
            }

            switch (sides)
            {
                case 3:
                    StartChar = '-';
                    offsetX = 1;
                    break;
                case 5:
                    StartChar = 'L';
                    offsetX = 1;
                    break;
                case 9:
                    StartChar = 'F';
                    offsetX = 1;
                    break;
                case 6:
                    StartChar = 'J';
                    offsetX = -1;
                    break;
                case 10:
                    StartChar = '7';
                    offsetX = -1;
                    break;
                case 12:
                    StartChar = '|';
                    offsetY = 1;
                    break;

            }
        }

        public void FindStartDirection(List<char[]> lines, int xStart, int yStart)
        {
            int sides = 0;

            if (lines[yStart].Length > (xStart + 1))
            {
                char c = lines[yStart][xStart + 1];
                if ((c == '-') || (c == 'J') || (c == '7'))
                {
                    // R
                    sides += 1;
                }
            }

            if (xStart > 0)
            {
                char c = lines[yStart][xStart - 1];
                if ((c == '-') || (c == 'L') || (c == 'F'))
                {
                    // L
                    sides += 2;
                }
            }

            if (yStart > 0)
            {
                char c = lines[yStart - 1][xStart];
                if ((c == '|') || (c == '7') || (c == 'F'))
                {
                    // T
                    sides += 4;
                }
            }

            if (lines.Count > (yStart + 1))
            {
                char c = lines[yStart + 1][xStart];
                if ((c == '|') || (c == 'L') || (c == 'J'))
                {
                    // B
                    sides += 8;
                }
            }

            switch (sides)
            {
                case 3:
                    StartChar = '-';
                    offsetX = 1;
                    break;
                case 5:
                    StartChar = 'L';
                    offsetX = 1;
                    break;
                case 9:
                    StartChar = 'F';
                    offsetX = 1;
                    break;
                case 6:
                    StartChar = 'J';
                    offsetX = -1;
                    break;
                case 10:
                    StartChar = '7';
                    offsetX = -1;
                    break;
                case 12:
                    StartChar = '|';
                    offsetY = 1;
                    break;

            }
        }

        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            xPos = 0;
            yPos = 0;
            offsetX = 0;
            offsetY = 0;

            List<string> lines = new List<string>();
            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            int startX = 0;
            int startY = 0;

            for (int i = 0; i < lines.Count; i++)
            {
                int index = lines[i].IndexOf('S');
                if (index >= 0)
                {
                    startX = index;
                    startY = i;
                    break;
                }
            }

            long count = 1;

            FindStartDirection(lines, startX, startY);

            xPos = startX + offsetX;
            yPos = startY + offsetY;

            while ((xPos != startX) || (yPos != startY))
            {
                Move(lines[yPos][xPos]);
                count++;
            }

            total = count / 2;

            Console.WriteLine("1) Result is: " + total);
        }


        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            List<char[]> path = new List<char[]>();
            List<char[]> lines = new List<char[]>();
            long total = 0;
            int startX = 0;
            int startY = 0;
            
            offsetX = 0;
            offsetY = 0;


            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (line.IndexOf('S') >= 0)
                    {
                        startX = line.IndexOf('S');
                        startY = lines.Count;
                    }
                    lines.Add(line.ToCharArray());
                    path.Add(line.ToCharArray());
                }
            }

            FindStartDirection(path, startX, startY);

            path[startY][startX] = StartChar;
            lines[startY][startX] = '*';

            xPos = startX + offsetX;
            yPos = startY + offsetY;

            while ((xPos != startX) || (yPos != startY))
            {
                int lasty = yPos;
                int lastx = xPos;

                Move(path[yPos][xPos]);

                lines[lasty][lastx] = '*';
            }

            using (StreamWriter writer = new StreamWriter("d:\\temp\\test.txt"))
            {

                for (int i = 0; i < lines.Count; i++)
                {
                    total += FindInternal(writer, lines[i], path[i]);

                }
            }

            Console.WriteLine("2) Result is: " + total);
        }

        private long FindInternal(StreamWriter writer, char[] line, char[] path)
        {
            long count = 0;
            bool inside = false;

            for (int i = 0; i < line.Length; i++)
            {
                char v = line[i];
                char p = path[i];

                if (v == '*')
                {
                    if ((p != '-') && (p != 'F') && (p != '7'))
                    {
                        inside = !inside;
                        path[i] = '*';
                    }
                    else
                    {
                        path[i] = ' ';
                    }
                }
                else
                {
                    if (inside)
                    {
                        count++;
                        line[i] = '1';
                        path[i] = '1';
                    }
                    else
                    {
                        line[i] = '.';
                        path[i] = '.';
                    }

                }
            }

            writer.WriteLine(path);

            return count;
        }

    }
}
