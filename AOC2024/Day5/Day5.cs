using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    public class Rule
    {
        public List<long> OrderedRules { get; set; } = new List<long>();

        public bool isOk(PrintList toBePrinted)
        {
            bool success = true;
            bool allThere = true;
            
            foreach (long val in OrderedRules)
            {
                if (!toBePrinted.Prints.Contains(val))
                {
                    allThere = false;
                    break;
                }
            }

            if (allThere)
            {
                if (toBePrinted.Prints.IndexOf(OrderedRules[0]) > toBePrinted.Prints.IndexOf(OrderedRules[1]))
                {
                    success = false;
                }
            }

            return success;
        }

        public bool isValid(PrintList toBePrinted)
        {
            bool allThere = true;

            foreach (long val in OrderedRules)
            {
                if (!toBePrinted.Prints.Contains(val))
                {
                    allThere = false;
                    break;
                }
            }

            return allThere;
        }
    }

    public class PrintList
    {
        public List<long> Prints { get; set; } = new List<long>();

        public void Reorder(List<Rule> validRules)
        {
            foreach (Rule rule in validRules)
            {
                if (!rule.isOk(this))
                {
                    int index1 = Prints.IndexOf(rule.OrderedRules[0]);
                    int index2 = Prints.IndexOf(rule.OrderedRules[1]);

                    long temp = Prints[index1];
                    Prints[index1] = Prints[index2];
                    Prints[index2] = temp;
                }
            }
        }

        public bool isCorrect(List<Rule> validRules)
        {
            bool success = true;

            foreach (var rule in validRules)
            {
                if (!rule.isOk(this))
                {
                    success = false;
                    break;
                }
            }

            return success;
        }

    }



    internal class Day5
    {
        private bool m_part2 = false;

        public List<Rule> ruleList = new List<Rule>();
        public List<PrintList> printList = new List<PrintList>();

        public Day5(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            foreach (var print in printList)
            {
                bool success = true;

                foreach (var rule in ruleList)
                {
                    if (!rule.isOk(print))
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    total += print.Prints[(print.Prints.Count / 2)];
                }

            }


            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            foreach (var print in printList)
            {
                List<Rule> validRules = new List<Rule>();

                foreach (var rule in ruleList)
                {
                    if (rule.isValid(print))
                    {
                        validRules.Add(rule);
                    }
                }

                bool reordered = false;
                while (!print.isCorrect(validRules))
                {
                    reordered = true;
                    print.Reorder(validRules);
                }

                if (reordered)
                {
                    total += print.Prints[(print.Prints.Count / 2)];
                }

            }

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            bool readingRules = true;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (readingRules)
                    {
                        List<long> rule = StringLibraries.GetListOfInts(line, '|');
                        ruleList.Add(new Rule() {  OrderedRules = rule });
                    }
                    else
                    {
                        List<long> list = StringLibraries.GetListOfInts(line, ',');
                        printList.Add(new PrintList() { Prints = list });
                    }
                }
                else
                {
                    readingRules = false;
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
