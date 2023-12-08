using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day8
{
    public class Node
    {
        public string L { get; set; }
        public string R { get; set; }

    }

    internal class Day8
    {
        internal void Execute1(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            string instructions = rdr.ReadLine().Trim();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            int total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    string paths = splits[1].Replace("(", "");
                    paths = paths.Replace(")", "");
                    string[] splitPaths = paths.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    nodes.Add(splits[0], new Node() { L = splitPaths[0], R = splitPaths[1] });
                    
                }
            }

            bool found = false;

            string currNode = "AAA";
            string endNode = "ZZZ";
            while (!found)
            {
                foreach (char val in instructions)
                {
                    if (val == 'R')
                    {
                        currNode = nodes[currNode].R;
                    }
                    else
                    {
                        currNode = nodes[currNode].L;
                    }
                    total++;

                    if (currNode == endNode)
                    {
                        found = true;
                        break;
                    }
                }
            }

            Console.WriteLine("1) Result is: " + total);
        }


        public long FindLowestPath(string instructions, Dictionary<string, Node> nodes,
                              string start)
        {
            long total = 0;
            bool found = false;

            string currNode = start;
            while (!found)
            {
                foreach (char val in instructions)
                {
                    if (val == 'R')
                    {
                        currNode = nodes[currNode].R;
                    }
                    else
                    {
                        currNode = nodes[currNode].L;
                    }
                    total++;

                    if (currNode.EndsWith("Z"))
                    {
                        found = true;
                        break;
                    }
                }
            }
            return total;
        }

        public long LowestCommonMultiple(List<long> lengths)
        {
            lengths.Sort();

            int n = lengths.Count;
            long largest = lengths[n - 1];   
            long index = 1;
            long current = largest;

            for (int i = 0; i < lengths.Count; i++)
            {
                if (largest % lengths[i] == 0)
                {
                    continue;  
                }
                else
                {
                    index = index + 1;
                    largest = current * index;
                    i = -1;
                }
            }

            return largest;
        }

        internal void Execute2(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            string instructions = rdr.ReadLine().Trim();
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            long total = 0;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] splits = line.Split('=', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    string paths = splits[1].Replace("(", "");
                    paths = paths.Replace(")", "");
                    string[] splitPaths = paths.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    nodes.Add(splits[0], new Node() { L = splitPaths[0], R = splitPaths[1] });

                }
            }

            List<string> currNodes = nodes.Keys.Where(x => x.EndsWith("A")).ToList();
            List<long> lengths = new List<long>();
            foreach (string node in currNodes)
            {
                lengths.Add(FindLowestPath(instructions, nodes, node));
            }
            total = LowestCommonMultiple(lengths);

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
