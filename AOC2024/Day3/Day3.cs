using AOCShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC2024
{
    internal class Day3
    {
        private bool m_part2 = false;
        private string inputline = string.Empty;

        public Day3(bool part2)
        {
            m_part2 = part2;
        }

        public long Calculate1()
        {
            long total = 0;

            Regex r = new Regex(@"mul\(\d*,\d*\)");
            foreach (Match m in r.Matches(inputline))
            {
                string value = m.Value.Substring(4, m.Length - 5);
                string[] splits = value.Split(',');
                int val1 = Convert.ToInt32(splits[0]);
                int val2 = Convert.ToInt32(splits[1]);

                total += val1 * val2;
            }

            return total;
        }

        public long Calculate1_noRegex()
        {
            long total = 0;

            int index = 0;
            while (index >= 0)
            {
                int startIndex = inputline.IndexOf("mul(", index);
                int closeIndex = -1;
                if (startIndex >= 0)
                {
                    closeIndex = inputline.IndexOf(")", startIndex + 4);
                    if (closeIndex >= 0)
                    {
                        string value = inputline.Substring(startIndex + 4, closeIndex - (startIndex + 4));

                        bool valid = true;
                        foreach (char v in value)
                        {
                            if (!char.IsDigit(v) && v != ',')
                            {
                                closeIndex = startIndex + 4;
                                valid = false;
                                break;
                            }
                        }

                        if (valid)
                        {
                            string[] splits = value.Split(',');
                            if (splits.Length == 2)
                            {
                                try
                                {
                                    int val1 = Convert.ToInt32(splits[0]);
                                    int val2 = Convert.ToInt32(splits[1]);

                                    total += val1 * val2;
                                }
                                catch (Exception)
                                {

                                }
                            }
                        }

                    }

                }
                index = closeIndex;
            }

            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            Dictionary<int, bool> onOff = new Dictionary<int, bool>();

            Regex doDont = new Regex(@"(don't\(\))|(do\(\))");
            foreach (Match m in doDont.Matches(inputline))
            {
                onOff[m.Index] = m.Value.Length > 4 ? false : true;
            }

            Regex r = new Regex(@"mul\(\d*,\d*\)");
            foreach (Match m in r.Matches(inputline))
            {
                bool lastVal = true;
                int lastIndex = 0;
                foreach (int key in onOff.Keys)
                {
                    if ((m.Index > lastIndex) && (m.Index < key))
                    {
                        break;
                    }

                    lastVal = onOff[key];
                    lastIndex = key;
                }

                if (lastVal)
                {
                    string value = m.Value.Substring(4, m.Length - 5);
                    string[] splits = value.Split(',');
                    int val1 = Convert.ToInt32(splits[0]);
                    int val2 = Convert.ToInt32(splits[1]);

                    total += val1 * val2;
                }
            }

            return total;
        }

        public long Calculate2_noregex()
        {
            long total = 0;
            bool processMult = true;

            int index = 0;
            while (index >= 0)
            {
                int startIndex = inputline.IndexOf("mul(", index);
                int closeIndex = -1;
                if (startIndex >= 0)
                {
                    string partString = inputline.Substring(index, startIndex - index);
                    int turnOn = partString.LastIndexOf("do()");
                    int turnOff = partString.LastIndexOf("don't()");

                    if (turnOn >= 0 || turnOff >= 0)
                    {
                        if (turnOn > turnOff)
                        {
                            processMult = true;
                        }
                        else
                        {
                            processMult = false;
                        }
                    }

                    if (!processMult)
                    {
                        closeIndex = startIndex + 1;
                    }
                    else
                    {
                        closeIndex = inputline.IndexOf(")", startIndex + 4);
                        if (closeIndex >= 0)
                        {
                            string value = inputline.Substring(startIndex + 4, closeIndex - (startIndex + 4));

                            bool valid = true;
                            foreach (char v in value)
                            {
                                if (!char.IsDigit(v) && v != ',')
                                {
                                    closeIndex = startIndex + 4;
                                    valid = false;
                                    break;
                                }
                            }

                            if (valid)
                            {
                                string[] splits = value.Split(',');
                                if (splits.Length == 2)
                                {
                                    try
                                    {
                                        int val1 = Convert.ToInt32(splits[0]);
                                        int val2 = Convert.ToInt32(splits[1]);

                                        total += val1 * val2;
                                    }
                                    catch (Exception)
                                    {

                                    }
                                }
                            }
                        }

                    }

                }
                index = closeIndex;
            }

            return total;
        }


        internal void ProcessSingleInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            inputline = rdr.ReadToEnd();
            rdr.Close();
        }

        public void ProcessMultipleInput(string line)
        {

        }

    }
}
