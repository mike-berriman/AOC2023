using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AOCShared;

namespace Day19
{

    internal class WorkflowStep
    {
        public char parameter { get; set; }
        public char operation { get; set; }
        public long Value { get; set; } = -1;
        public string ReturnValue { get; set; } = string.Empty;

        public WorkflowStep(string line, bool part2)
        {
            List<string> splits = StringLibraries.GetListOfStrings(line, ':');
            if (splits.Count > 1)
            {
                parameter = splits[0][0];
                operation = splits[0][1];
                Value = Convert.ToInt64(splits[0].Substring(2));

                ReturnValue = splits[1];
            }
            else
            {
                ReturnValue = splits[0];
            }
        }

        public long GetPartValue(Part p)
        {
            switch (parameter)
            {
                case 'x':
                    return p.X;
                case 'm':
                    return p.M;
                case 'a':
                    return p.A;
                case 's':
                    return p.S;

            }
            return 0;
        }

        public string Process(Part p)
        {
            if (Value < 0)
            {
                return ReturnValue;
            }
            else
            {
                if (operation == '<')
                {
                    if (GetPartValue(p) < Value)
                    {
                        return ReturnValue;
                    }
                    else
                    {
                        return "";
                    }
                }
                else if (operation == '>')
                {
                    if (GetPartValue(p) > Value)
                    {
                        return ReturnValue;
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            return "";
        }
    }

    internal class Workflow
    {
        public string Name { get; set; } = string.Empty;
        public string WorkflowDef { get; set; } = string.Empty;
        public List<WorkflowStep> steps = new List<WorkflowStep>();

        public Workflow(string line, bool part2)
        {
            var splits = StringLibraries.GetListOfStrings(line, '{');
            Name = splits[0];

            string WorkflowDef = splits[1].Replace("}", "");
            splits = StringLibraries.GetListOfStrings(WorkflowDef, ',');

            foreach (string split in splits)
            {
                WorkflowStep step = new WorkflowStep(split, part2);
                steps.Add(step);
            }
        }

        public bool Process(Part p, Dictionary<string, Workflow> workflows)
        {
            foreach (var step in steps)
            {
                string returnVal = step.Process(p);
                if (returnVal.Equals("A"))
                {
                    return true;
                }
                else if (returnVal.Equals("R"))
                {
                    return false;
                }
                else if (returnVal.Length > 0)
                {
                    Workflow newFlow = workflows[returnVal];
                    return newFlow.Process(p, workflows);
                }
            }

            return false;
        }

        public long FindTotalCombinations(MinMaxCache currentCache, Dictionary<string, Workflow> workflows)
        {
            long total = 0;

            foreach (var step in steps)
            {
                long inc = 1;
                MinMaxCache useCache = null;

                if (step.operation == '<')
                {
                    useCache = new MinMaxCache(currentCache, step.parameter, 1, (int)(step.Value) - 1);
                    currentCache.LessThan(step.Value, step.parameter);
                } else if (step.operation == '>')
                {
                    useCache = new MinMaxCache(currentCache, step.parameter, (int)(step.Value) + 1, 4000);
                    currentCache.GreaterThan(step.Value, step.parameter);
                }
                else
                {
                    useCache = new MinMaxCache(currentCache);
                }

                if (step.ReturnValue == "A")
                {
                    total += useCache.ValuesLeft();
                }
                else if (step.ReturnValue == "R")
                {
                    total += 0;
                }
                else
                {
                    Workflow nextWorkflow = workflows[step.ReturnValue];
                    total += nextWorkflow.FindTotalCombinations(useCache, workflows);
                }
            }

            return total;
        }
    }

    internal class MinMaxCache
    {
        Dictionary<char, MinMax> cache = new Dictionary<char, MinMax>();

        public MinMaxCache()
        {
            cache.Add('x', new MinMax());
            cache.Add('m', new MinMax());
            cache.Add('a', new MinMax());
            cache.Add('s', new MinMax());
        }

        public MinMaxCache(MinMaxCache rhs)
        {
            foreach (var m in rhs.cache)
            {
                cache.Add(m.Key, new MinMax(m.Value));
            }

        }

        public MinMaxCache(MinMaxCache rhs, char parameter, int min, int max)
        {
            foreach (var minmax in rhs.cache)
            {
                if (minmax.Key == parameter)
                {
                    cache.Add(parameter, new MinMax(Math.Max(min, minmax.Value.Min), Math.Min(max, minmax.Value.Max)));
                }
                else
                {
                    cache.Add(minmax.Key, new MinMax(minmax.Value));
                }
            }

        }

        public long ValuesLeft()
        {
            long total = 1;
            foreach (MinMax m in cache.Values)
            {
                total *= (m.Max - m.Min+1);
            }

            return total;
        }

        public void LessThan(long value, char param)
        {
            if (cache.ContainsKey(param))
            {
                MinMax m = cache[param];
                m.Min = Math.Max(m.Min, (int)value);
            }

        }

        public void GreaterThan(long value, char param)
        {
            if (cache.ContainsKey(param))
            {
                MinMax m = cache[param];
                m.Max = Math.Min(m.Max, (int)value);
            }

        }

    }

    internal class MinMax
    {
        public int Min { get; set; } = 1;
        public int Max { get; set; } = 4000;

        public MinMax()
        {

        }

        public MinMax(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public MinMax(MinMax rhs)
        {
            Min = rhs.Min;
            Max = rhs.Max;
        }

        public long Remainder()
        {
            return Max - Min;
        }
    }

    internal class Part
    {
        private bool m_part2 = false;
        public long X { get; set; } = 0;
        public long M { get; set; } = 0;
        public long A { get; set; } = 0;
        public long S { get; set; } = 0;

        public Part(string line, bool part2)
        {
            m_part2 = part2;

            string data = line.Replace("{", "");
            data = data.Replace("}", "");
            List<string> parts = StringLibraries.GetListOfStrings(data, ',');
            foreach(string part in parts)
            {
                switch (part[0])
                {
                    case 'x':
                        X = Convert.ToInt64(part.Substring(2));
                        break;
                    case 'm':
                        M = Convert.ToInt64(part.Substring(2));
                        break;
                    case 'a':
                        A = Convert.ToInt64(part.Substring(2));
                        break;
                    case 's':
                        S = Convert.ToInt64(part.Substring(2));
                        break;
                }
            }
        }

        public long Calculate1(Dictionary<string, Workflow> workflows)
        {
            long total = 0;

            Workflow start = workflows["in"];
            if (start.Process(this, workflows))
            {
                total = X + M + A + S;
            }

            return total;
        }

        public long Calculate2(Dictionary<string, Workflow> workflows)
        {
            long total = 0;

            Workflow start = workflows["in"];

            MinMaxCache cache = new MinMaxCache();

            total = start.FindTotalCombinations(cache, workflows);


            return total;
        }


        public long Calculate(Dictionary<string, Workflow> workflows)
        {
            long total = 0;

            if (!m_part2)
            {
                total = Calculate1(workflows);
            }
            else
            {
                total = Calculate2(workflows);
            }

            return total;
        }
    }


    internal class Day19
    {
        List<Part> inputObjects = new List<Part>();
        Dictionary<string, Workflow> workflows = new Dictionary<string, Workflow>();

        internal void ProcessInput(string fileName, bool part2)
        {
            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            bool parseworkflows = true;
            while ((line = rdr.ReadLine()) != null)
            {
                if (string.IsNullOrEmpty(line))
                {
                    parseworkflows = false;
                }

                if (parseworkflows)
                {
                    Workflow workflow = new Workflow(line, part2);
                    workflows.Add(workflow.Name, workflow);
                }
                else
                {
                    Part av = new Part(line, part2);
                    inputObjects.Add(av);
                }
            }

        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            foreach (var obj in inputObjects)
            {
                total += obj.Calculate(workflows);
            }

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            total = inputObjects[0].Calculate(workflows);

            return total;
        }


        public void Execute(string fileName, bool part2, int counter)
        {
            DateTime startTime = DateTime.Now;

            ProcessInput(fileName, part2);

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
