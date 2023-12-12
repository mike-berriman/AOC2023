using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day12
{
    public class Line
    {
        public string Sequence { get; set; } = string.Empty;
        public List<long> Backup { get; set; } = new List<long>();

        public string MultipleSequence { get; set; } = string.Empty;
        public List<long> MultipleBackup { get; set; } = new List<long>();

        bool multiple = false;

        public Line()
        {

        }

        public Line(string line, bool multiply)
        {
            string[] splits = line.Split(' ');
            Sequence = splits[0];

            string backup = splits[1];

            MultipleSequence = Sequence;
            string multiBackup = backup;
            for (int i = 0; i < 4; i++)
            {
                MultipleSequence += '?';
                MultipleSequence += splits[0];

                multiBackup += ',';
                multiBackup += backup;
            }

            MultipleBackup = StringLibraries.GetListOfInts(multiBackup, ',');
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

        public bool Match(string tester)
        {
            string[] splits = tester.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            List<int> values = new List<int>();
            foreach (string val in splits)
            {
                values.Add(val.Length);
            }

            return IsMatch(values);

        }

        public long LongMethodCalculation()
        {
            long total = 0;

            //string matchFound = string.Empty;
            //long firstlasttotal = MethodsCalculation(false, ref matchFound);

            //Line l = new Line()
            //{
            //    Sequence = "?" + this.Sequence,
            //    Backup = this.Backup
            //};

            //long middleTotal = l.MethodsCalculation(false, ref matchFound);

            //total = (long)Math.Pow(firstlasttotal, 2) + (long)Math.Pow(middleTotal, 3);

            Sequence = MultipleSequence;
            Backup = MultipleBackup;

            total = MethodsCalculation();


            return total;
        }


        public long MethodsCalculation()
        {
            long count = 0;
            List<int> questions = new List<int>();

            int startPos = 0;
            while (true)
            {
                int index = Sequence.IndexOf('?', startPos);
                if (index < 0)
                {
                    break;
                }
                else
                {
                    questions.Add(index);
                    startPos = index+1;
                }
            }

            long val = (long)Math.Pow(2, questions.Count);

            List<long> masks = new List<long>();
            for (int j = 0; j < questions.Count; j++)
            {
                masks.Add((long)Math.Pow(2, j));
            }

            for (int i = 0; i < val; i++)
            {
                char[] test = Sequence.ToArray();

                for (int j = 0; j < questions.Count; j++)
                {
                    if ((i & masks[j]) > 0)
                    {
                        test[questions[j]] = '.';
                    }
                    else
                    {
                        test[questions[j]] = '#';
                    }
                }

                if (Match(new string(test)))
                {
                    count++;
                }

            }

            return count;
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
                    //Line ln = new Line(line, false);
                    //total += ln.MethodsCalculation();
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

            object locker = new object();
            Parallel.ForEach(lines, line =>
            {
                Line ln = new Line(line, true);
                //long val = ln.MethodsCalculation();
                long val = ln.LongMethodCalculation();

                lock(locker)
                {
                    total += val;
                }

                Console.Write('.');
            });


            Console.WriteLine("2) Result is: " + total);
        }

    }
}
