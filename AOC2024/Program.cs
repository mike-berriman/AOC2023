namespace AOC2024
{

    internal class Program
    {
        const string fileName = @"D:\temp\advent\AOC2024\Day12\TestData1.txt";
        const string fileName2 = @"D:\temp\advent\AOC2024\Day12\InputData.txt";

        List<Day12> inputObjects = new List<Day12>();
        Day12 mainObject = null;
        bool singleObject = false;

        [STAThread]
        static void Main(string[] args)
        {
            Program p = new Program();

            p.singleObject = true;
            p.Execute(fileName, false, 1);
            //p.Execute(fileName2, false, 1);
            //p.Execute(fileName, true, 1);
            //p.Execute(fileName2, true, 1);
            
            Console.ReadKey();
        }


        internal void ProcessMultipleInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    // READER code here
                    Day12 av = new Day12(part2);

                    av.ProcessMultipleInput(line);
                    inputObjects.Add(av);
                }
            }
        }

        internal long Execute(string fileName, bool part2)
        {
            long total = 0;

            if (singleObject)
            {
                if (!part2)
                {
                    total = mainObject.Calculate1();
                }
                else
                {
                    total = mainObject.Calculate2();
                }
            }
            else
            {
                foreach (var obj in inputObjects)
                {
                    if (!part2)
                    {
                        total += obj.Calculate1();
                    }
                    else
                    {
                        total += obj.Calculate2();
                    }
                }
            }

            return total;
        }

        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            if (singleObject)
            {
                mainObject = new Day12(part2);
                mainObject.ProcessSingleInput(fileName);
            }
            else
            {
                ProcessMultipleInput(fileName, part2);
            }

            long total;
            total = Execute(fileName, part2);

            long millis = (long)(DateTime.Now - startTime).TotalMilliseconds;

            Console.WriteLine(counter + ") " + "(" + millis + ") Result is: " + total);
        }
    }
}
