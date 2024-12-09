using AOCShared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class DiskBlock
    {
        public long Value { get; set; } = 0;
        public long Length { get; set; } = 0;

        public DiskBlock(long value, long length)
        {
            Value = value;
            Length = length;
        }
    }

    internal class Day9
    {
        private bool m_part2 = false;
        private string inputString;

        private List<int> expandedString = new List<int>(1000000);

        public Day9(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            bool isBlank = false;
            int count = 0;
            int index = 0;
            foreach (char val in inputString)
            {
                int valInt = val - '0';
                for (int i =0; i < valInt; i++)
                {
                    if (isBlank)
                    {
                        expandedString.Add(-1);
                    }
                    else
                    {
                        expandedString.Add(count);
                    }
                }

                if (!isBlank)
                {
                    count++;
                }

                isBlank = !isBlank;
            }

            int revCount = expandedString.Count - 1;
            for(int i = 0; i < expandedString.Count; i++)
            {
                if (expandedString[i] == -1)
                {
                    int j = revCount;
                    for (; j > i; j--)
                    {
                        if (expandedString[j] != -1) 
                        {
                            expandedString[i] = expandedString[j];
                            expandedString[j] = -1;
                            break;
                        }
                    }

                    if (j <= i)
                    {
                        break;
                    }
                    revCount = j;
                }
            }

            for (int i = 0; i < expandedString.Count; i++)
            {
                if (expandedString[i] != -1)
                {
                    total += i * expandedString[i];
                }
            }
            
            return total;
        }

        private List<DiskBlock> diskBlocks = new List<DiskBlock>();

        public long Calculate2()
        {
            long total = 0;

            bool isBlank = false;
            long count = 0;
            foreach (char val in inputString)
            {
                int valInt = val - '0';

                if (!isBlank)
                {
                    diskBlocks.Add(new DiskBlock(count, valInt));
                    count++;
                }
                else
                {
                    diskBlocks.Add(new DiskBlock(-1, valInt));
                }

                isBlank = !isBlank;
            }

            for (int i = diskBlocks.Count-1; i > 0; i--)
            {
                DiskBlock testBlock = diskBlocks[i];

                if (testBlock.Value < 0)
                {
                    continue;
                }

                int spaceIndex = diskBlocks.FindIndex(x => (x.Value == -1) && (x.Length >= testBlock.Length));
                if ((spaceIndex >= 0) && (spaceIndex < i))
                {
                    DiskBlock replaceBlock = diskBlocks[spaceIndex];
                    if (replaceBlock.Length == testBlock.Length)
                    {
                        replaceBlock.Value = testBlock.Value;
                    }
                    else
                    {
                        DiskBlock newBlock = new DiskBlock(testBlock.Value, testBlock.Length);
                        diskBlocks.Insert(spaceIndex, newBlock);
                        replaceBlock.Length -= newBlock.Length;
                        spaceIndex++;
                        i++;

                        if (spaceIndex < diskBlocks.Count - 1)
                        {
                            if (diskBlocks[spaceIndex + 1].Value == -1)
                            {
                                replaceBlock.Length += diskBlocks[spaceIndex + 1].Length;
                                diskBlocks.RemoveAt(spaceIndex + 1);
                            }
                        }
                    }

                    testBlock.Value = -1;

                    if (i < diskBlocks.Count - 1)
                    {
                        if (diskBlocks[i + 1].Value == -1)
                        {
                            testBlock.Length += diskBlocks[i + 1].Length;
                            diskBlocks.RemoveAt(i + 1);
                        }
                    }

                    if (i > 0)
                    {
                        if (diskBlocks[i - 1].Value == -1)
                        {
                            diskBlocks[i - 1].Length += testBlock.Length;
                            diskBlocks.RemoveAt(i);
                        }
                    }

                }
            }

            count = 0;
            for (int i = 0; i < diskBlocks.Count; i++)
            {
                DiskBlock testBlock = diskBlocks[i];
                if (testBlock.Value != -1)
                {
                    for (int j = 0; j < testBlock.Length; j++)
                    {
                        total += count * testBlock.Value;
                        count++;
                    }
                }
                else
                {
                    count += testBlock.Length;
                }
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
                    inputString = line;
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
