using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day8Tester
{
    public class Node
    {
        public string L { get; set; }
        public string R { get; set; }

    }

    internal class Day8Tester
    {
        internal void Execute1(string fileName)
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
                    List<string> splits = StringLibraries.GetListOfStrings(line, '=');
                    string paths = splits[1].Replace("(", "").Replace(")", "");
                    List<string> splitPaths = StringLibraries.GetListOfStrings(paths, ',');

                    nodes.Add(splits[0], new Node() { L = splitPaths[0], R = splitPaths[1] });
                }
            }

            total = FindLowestPath(instructions, nodes, "AAA", "ZZZ", null);

            Console.WriteLine("1) Result is: " + total);
        }


        public long FindLowestPath(string instructions, Dictionary<string, Node> nodes,
                                   string start, string end, string endWith)
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

                    if ((!string.IsNullOrEmpty(end) && currNode.Equals(end)) ||
                        (!string.IsNullOrEmpty(endWith) && currNode.EndsWith(endWith)))
                    {
                        found = true;
                        break;
                    }
                }
            }
            return total;
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
                    List<string> splits = StringLibraries.GetListOfStrings(line, '=');
                    string paths = splits[1].Replace("(", "").Replace(")", "");
                    List<string> splitPaths = StringLibraries.GetListOfStrings(paths, ',');

                    nodes.Add(splits[0], new Node() { L = splitPaths[0], R = splitPaths[1] });
                }
            }

            List<string> startNodes = nodes.Keys.Where(x => x.EndsWith("A")).ToList();

            List<long> lengths = new List<long>();
            foreach (string node in startNodes)
            {
                lengths.Add(FindLowestPath(instructions, nodes, node, null, "Z"));
            }
            
            total = MathLibraries.LowestCommonMultiple(lengths);

            Console.WriteLine("2) Result is: " + total);
        }

    }
}
