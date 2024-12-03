using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day12
{
    public class RecursionDetails
    {
        public int currIndex;
        public int backupNumberIndex;
        public int numberChunkCount;
        public string recursiveSequenceTest = string.Empty;

        public override int GetHashCode()
        {
            return (currIndex * 1000000) + (backupNumberIndex * 1000) + numberChunkCount;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as RecursionDetails);
        }

        public bool Equals(RecursionDetails obj)
        {
            return obj != null && obj.GetHashCode() == this.GetHashCode();
        }
    }

    public class Line
    {
        public string Sequence { get; set; } = string.Empty;

        public List<long> Backup { get; set; } = new List<long>();
        bool multiple = false;

        public Line()
        {

        }

        public Line(string line, bool multiply)
        {
            string[] splits = line.Split(' ');
            Sequence = splits[0];

            string backup = splits[1];

            if (multiply)
            {
                string multiBackup = backup;
                for (int i = 0; i < 4; i++)
                {
                    multiBackup += ',';
                    multiBackup += backup;

                    Sequence += "?";
                    Sequence += splits[0];
                }
                backup = multiBackup;
            }

            Backup = StringLibraries.GetListOfInts(backup, ',');
            multiple = multiply;
        }

        public bool IsMatch(List<int> testValues)
        {
            if (testValues.Count != Backup.Count)
            {
                return false;
            }

            for (int i = 0; i < testValues.Count; i++ )
            {
                if (testValues[i] != Backup[i])
                {
                    return false;
                }
            }

            return true;
        }


        char[] questionArray = new char[2] { '.', '#' };
        Dictionary<RecursionDetails, long> cache = new Dictionary<RecursionDetails, long>();

        public long Recurse(RecursionDetails details)
        {
            long count = 0;

            if (cache.ContainsKey(details))
            {
                return cache[details];
            }

            if (details.currIndex == Sequence.Length)
            {
                if (details.numberChunkCount == 0)
                {
                    // Ended on a '.'
                    if (details.backupNumberIndex == Backup.Count)
                    {
                        return 1;

                    }
                    else
                    {
                        return 0;

                    }
                }
                else if ((details.backupNumberIndex == Backup.Count-1) && (details.numberChunkCount == Backup[details.backupNumberIndex]))
                {
                    // Ended on a '#'
                    return 1;
                }
                else
                {
                    return 0;
                }
            }

            char[] useChar = new char[] { Sequence[details.currIndex] };
            if (Sequence[details.currIndex] == '?')
            {
                useChar = questionArray;
            }

            foreach (char c in useChar)
            {
                if (c == '.')
                {
                    if (details.numberChunkCount > 0)
                    {
                        // Just coming out of a chunk of #
                        if ((details.backupNumberIndex >= Backup.Count) || (details.numberChunkCount != Backup[details.backupNumberIndex]))
                        {
                            // Not a match - bail.
                            continue;
                        }

                        if (details.backupNumberIndex < Backup.Count)
                        {
                            RecursionDetails newDetails = new RecursionDetails()
                            {
                                currIndex = details.currIndex + 1,
                                backupNumberIndex = details.backupNumberIndex + 1,
                                numberChunkCount = 0,
                                recursiveSequenceTest = details.recursiveSequenceTest + c
                            };
                            count += Recurse(newDetails);
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        RecursionDetails newDetails = new RecursionDetails()
                        {
                            currIndex = details.currIndex + 1,
                            backupNumberIndex = details.backupNumberIndex,
                            numberChunkCount = 0,
                            recursiveSequenceTest = details.recursiveSequenceTest + c
                        };
                        count += Recurse(newDetails);
                    }
                }
                else if (c == '#')
                {
                    RecursionDetails newDetails = new RecursionDetails()
                    {
                        currIndex = details.currIndex + 1,
                        backupNumberIndex = details.backupNumberIndex,
                        numberChunkCount = details.numberChunkCount + 1,
                        recursiveSequenceTest = details.recursiveSequenceTest + c
                    };

                    count += Recurse(newDetails);
                }
            }

            cache[details] = count;
            return count;
        }

        public long CalculateRecursive()
        {
            
            return Recurse(new RecursionDetails());
        }

    }


    internal class Day12
    {
        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    Line ln = new Line(line, false);

                    long val = ln.CalculateRecursive();
                    total += val;
                }
            }

            Console.WriteLine("1) Result is: " + total);
        }


        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            long total = 0;
            List<string> lines = new List<string>();
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    lines.Add(line);
                }
            }

            object lockObj = new object();
            int count = 0;

            DateTime startTime = DateTime.Now;

            foreach (string thisLine in lines)
            //Parallel.ForEach(lines, thisLine =>
            {
                Line ln = new Line(thisLine, true);

                long val = ln.CalculateRecursive();

                //lock(lockObj)
                {
                    total += val;
                    //Console.WriteLine(count++ + " - " + total);
                }
            }
            //);

            Console.WriteLine("Time taken: " + (DateTime.Now - startTime).TotalMilliseconds);
            Console.WriteLine("2) Result is: " + total);

        }

    }
}
