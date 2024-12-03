using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day2
{
    internal class Day2
    {
        //string fileName = @"D:\temp\advent\AdventOfCoding\Day2\TestData1.txt";
        string fileName = @"D:\temp\advent\AdventOfCoding\Day2\InputData.txt";

        //string fileName2 = @"D:\temp\advent\AdventOfCoding\AdventOfCoding\InputData2.txt";
        private Dictionary<string, int> gameResult = new Dictionary<string, int>();
        private Dictionary<string, int> limits = new Dictionary<string, int>();
        private Dictionary<string, int> minimums = new Dictionary<string, int>();

        private int GetGameNumber(string line)
        {
            int colonIndex = line.IndexOf(':');
            string gameNumber = line.Substring(5, colonIndex - 5);
            return Convert.ToInt32(gameNumber);
        }

        private void ProcessSplit(string split)
        {
            split = split.Trim();
            string[] newSplits = split.Split(' ');
            gameResult[newSplits[1]] = Convert.ToInt32(newSplits[0]);
        }

        private bool isValid()
        {
            foreach (var value in limits)
            {
                if (gameResult.ContainsKey(value.Key))
                {
                    if (gameResult[value.Key] > value.Value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool ProcessSet(string set)
        {
            gameResult = new Dictionary<string, int>();
            string[] splits = set.Trim().Split(new char[] { ',' });

            foreach (string split in splits)
            {
                ProcessSplit(split);
            }

            return false;
        }

        private int CalculateGame(string line)
        {
            int returnVal = -1;

            returnVal = GetGameNumber(line);

            int colonIndex = line.IndexOf(':');

            string gameResults = line.Substring(colonIndex + 1);
            string[] splits = gameResults.Split(new char[] { ';'});

            foreach (string split in splits)
            {
                ProcessSet(split);
                if (!isValid())
                {
                    returnVal = -1;
                    break;
                }
            }

            return returnVal;
        }


        internal void Execute1()
        {
            limits["blue"] = 14;
            limits["red"] = 12;
            limits["green"] = 13;

            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    int returnVal = CalculateGame(line);
                    if (returnVal > 0)
                    {
                        total += returnVal;
                    }
                }
            }

            Console.WriteLine("1) Result is: " + total);
        }

        private void FindMin()
        {
            foreach (var value in gameResult)
            {
                if (minimums.ContainsKey(value.Key))
                {
                    if (minimums[value.Key] < value.Value)
                    {
                        minimums[value.Key] = value.Value;
                    }
                }
                else
                {
                    minimums[value.Key] = value.Value;
                }
            }
        }

        private int CalculateGame2(string line)
        {
            minimums = new Dictionary<string, int>();

//            returnVal = GetGameNumber(line);

            int colonIndex = line.IndexOf(':');

            string gameResults = line.Substring(colonIndex + 1);
            string[] splits = gameResults.Split(new char[] { ';' });

            foreach (string split in splits)
            {
                ProcessSet(split);
                FindMin();
            }

            int total = 1;
            foreach(var val in minimums)
            {
                total *= val.Value;
            }

            return total;
        }

        internal void Execute2()
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    total += CalculateGame2(line);
                }
            }

            Console.WriteLine("2) Result is: " + total);
        }
    }
}