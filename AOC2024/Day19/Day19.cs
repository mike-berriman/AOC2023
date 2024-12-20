using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day19
    {
        private bool m_part2 = false;
        List<string> m_available = new List<string>();
        List<string> m_required = new List<string>();

        public Day19(bool part2)
        {
            m_part2 = part2;
        }


        public long FindIfMatch(string required)
        {
            long total = 0;
            var possibleMatches = m_available.Where(x => required.StartsWith(x));

            foreach (var match in possibleMatches)
            {
                string nextPart = required.Substring(match.Length);

                if (string.IsNullOrEmpty(nextPart))
                {
                    total++;
                }
                else if (total == 0)
                {
                    total += FindIfMatch(nextPart);
                }
            }

            return total;
        }


        Dictionary<string, long> possibilityCache = new Dictionary<string, long>();

        public long FindAllMatch(string required)
        {
            long total = 0;

            if (possibilityCache.ContainsKey(required))
            {
                total += possibilityCache[required];
            }
            else
            {
                var possibleMatches = m_available.Where(x => required.StartsWith(x));

                foreach (var match in possibleMatches)
                {
                    string nextPart = required.Substring(match.Length);

                    if (string.IsNullOrEmpty(nextPart))
                    {
                        total++;
                    }
                    else
                    {
                        total += FindAllMatch(nextPart);
                    }
                }

                possibilityCache.Add(required, total);
            }

            return total;
        }


        public long Calculate1()
        {
            long total = 0;

            string currentString = string.Empty;
            foreach (string required in m_required)
            {
                if (FindIfMatch(required) > 0)
                {
                    total++;
                }
            }

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            string currentString = string.Empty;
            foreach (string required in m_required)
            {
                total += FindAllMatch(required);
            }

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            bool first = true;
            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    if (first)
                    {
                        m_available = StringLibraries.GetListOfStrings(line, ',');
                        first = false;
                    }
                    else
                    {
                        m_required.Add(line.Trim());
                    }
                }
            }
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
