using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;
using Microsoft.Win32.SafeHandles;

namespace Day13
{
    internal class Day13
    {

        public class Pattern
        {
            public List<string> Lines { get; set; } = new List<string>();

            public long CountVericalReflections(bool smudges)
            {
                Pattern newPattern = new Pattern();

                for (int i = 0; i < Lines[0].Length; i++)
                {
                    newPattern.Lines.Add(new string(Lines.Select(x => x[i]).ToArray()));
                }

                if (smudges)
                {
                    return newPattern.FindWithSmudges();
                }
                else
                {
                    return newPattern.CountHorizontalReflections(smudges, -1);
                }
            }

            public long CountHorizontalReflections(bool smudges, long previousReflect)
            {
                long total = 0;

                List<int> possibleReflections = new List<int>();
                for (int i = 0; i < Lines.Count - 1; i++)
                {
                    if (Lines[i + 1].Equals(Lines[i]))
                    {
                        possibleReflections.Add(i);
                    }
                }


                foreach (int reflect in possibleReflections)
                {
                    bool reflection = true;

                    for (int i = 0; i <= reflect; i++)
                    {
                        if ((reflect+1 + i) < Lines.Count)
                        {
                            if (!Lines[reflect-i].Equals(Lines[reflect+1+i]))
                            {
                                reflection = false;
                                break;
                            }
                        }
                    }

                    if (reflection)
                    {
                        if ((previousReflect < 0) || ((reflect+1) != previousReflect))
                        {
                            total += (reflect + 1);
                        }
                    }
                }

                return total;
            }

            public long FindWithSmudges()
            {
                long originalTotal = CountHorizontalReflections(false, -1);

                int lineLength = Lines[0].Length;

                for (int loop = 1; loop < Lines.Count-1; loop += 2)
                {
                    int inc = loop;

                    for (int i = 0; i < Lines.Count-inc; i++)
                    {
                        if (!Lines[i].Equals(Lines[i+inc]))
                        {
                            int mismatchCount = 0;
                            for (int j = 0; j < lineLength; j++)
                            {
                                if (Lines[i][j] != Lines[i + inc][j])
                                {
                                    mismatchCount++;
                                }
                            }

                            if (mismatchCount == 1)
                            {
                                Pattern newPattern = new Pattern();
                                newPattern.Lines = new List<string>(Lines);
                                newPattern.Lines[i] = newPattern.Lines[i + inc];

                                long newCount = newPattern.CountHorizontalReflections(false, originalTotal);

                                if ((newCount > 0) && (newCount != originalTotal))
                                {
                                    return newCount;
                                }
                                else
                                {
                                    Pattern newPattern2 = new Pattern();
                                    newPattern2.Lines = new List<string>(Lines);
                                    newPattern2.Lines[i+inc] = newPattern2.Lines[i];

                                    newCount = newPattern2.CountHorizontalReflections(false, originalTotal);

                                    if ((newCount > 0) && (newCount != originalTotal))
                                    {
                                        return newCount;
                                    }

                                }
                            }
                        }
                    }
                }

                return 0;
            }



            public long FindEflectionTotal(bool smudges)
            {
                if (smudges)
                {
                    return (FindWithSmudges() * 100) + CountVericalReflections(smudges);
                }
                else
                {
                    return (CountVericalReflections(smudges)) + (CountHorizontalReflections(smudges, -1) * 100);

                }

            }
        }

        internal void Execute1(string fileName)
        {
            long total = 0;
            List<Pattern> patterns = new List<Pattern>();
            using (StreamReader rdr = new StreamReader(fileName))
            {
                string line = string.Empty;

                Pattern pattern = new Pattern();
                while ((line = rdr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        pattern.Lines.Add(line);
                    }
                    else
                    {
                        patterns.Add(pattern);
                        pattern = new Pattern();
                    }
                }

                patterns.Add(pattern);
            }

            int count = 0;
            foreach (Pattern p in patterns)
            {
                long val = p.FindEflectionTotal(false);

                Console.WriteLine(count++ + " - " + val);

                total += val;
            }


            Console.WriteLine("1) Result is: " + total);
        }






        internal void Execute2(string fileName)
        {
            long total = 0;
            List<Pattern> patterns = new List<Pattern>();
            using (StreamReader rdr = new StreamReader(fileName))
            {
                string line = string.Empty;

                Pattern pattern = new Pattern();
                while ((line = rdr.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        pattern.Lines.Add(line);
                    }
                    else
                    {
                        patterns.Add(pattern);
                        pattern = new Pattern();
                    }
                }

                patterns.Add(pattern);
            }

            int count = 0;
            foreach (Pattern p in patterns)
            {
                long val = p.FindEflectionTotal(true);

                Console.WriteLine(count++ + " - " + val);

                total += val;
            }

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
