using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day15
{
    internal class Lens
    {
        public string Label { get; set; } = string.Empty;
        public int FocalLength { get; set; } = 0;
    }


    internal class AdventClass
    {
        List<List<Lens>> boxes = new List<List<Lens>>();

        List<string> values = new List<string>();
        public AdventClass(string line, bool part2)
        {
            values = StringLibraries.GetListOfStrings(line, ',');

            for (int i = 0; i < 256; i++)
            {
                boxes.Add(new List<Lens>());
            }
        }

        public long asciiHash(string valString)
        {
            long total = 0;

            foreach (char val in valString)
            {
                total += (long)val;
                total *= 17;

                total = total % 256;
             }

            return total;
        }

        public long Calculate()
        {
            long total = 0;

            foreach (string value in values)
            {
                total += asciiHash(value);
            }
            return total;
        }

        public long Calculate2()
        {
            long total = 0;
            List<string> lensLabels = new List<string>();

            foreach (string value in values)
            {
                string operation = "=";
                List<string> splits = StringLibraries.GetListOfStrings(value, operation[0]);
                if (splits.Count < 2)
                {
                    operation = "-";
                    splits = StringLibraries.GetListOfStrings(value, operation[0]);
                }

                int boxNum = (int)asciiHash(splits[0]);
                int index = boxes[boxNum].FindIndex(x => x.Label.Equals(splits[0]));

                if (operation.Equals("="))
                {
                    if (index < 0)
                    {
                        Lens lens = new Lens()
                        {
                            Label = splits[0],
                            FocalLength = Convert.ToInt32(splits[1])
                        };

                        boxes[boxNum].Add(lens);

                        if (!lensLabels.Contains(lens.Label))
                        {
                            lensLabels.Add(lens.Label);
                        }
                    }
                    else
                    {
                        boxes[boxNum][index].FocalLength = Convert.ToInt32(splits[1]);
                    }
                }
                else
                {
                    if (index >= 0)
                    {
                        boxes[boxNum].RemoveAt(index);
                    }
                }

            }

            foreach (string lensLabel in lensLabels)
            {
                for (int j = 0; j < boxes.Count; j++)
                {
                    var box = boxes[j];

                    if (box.Count > 0)
                    {
                        int index = box.FindIndex(x => x.Label.Equals(lensLabel));

                        if (index >= 0)
                        {
                            long focusPower = j + 1;

                            focusPower *= ((index + 1) * box[index].FocalLength);

                            total += focusPower;
                        }
                    }
                }
            }

            return total;




        }

    }


    internal class Day15
    {
        AdventClass inputObjects = null;

        internal void ProcessInput(string fileName)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    inputObjects = new AdventClass(line, false);
                    break;

                    
                }
            }

        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            ProcessInput(fileName);
            total = inputObjects.Calculate();

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            ProcessInput(fileName);
            total = inputObjects.Calculate2();

            return total;
        }


        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            long total;
            if (!part2)
            {
                total = Execute1(fileName);
            }
            else
            {
                total = Execute2(fileName);
            }

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(counter + ") " + "(" + millis + ") Result is: " + total);

        }

    }
}
