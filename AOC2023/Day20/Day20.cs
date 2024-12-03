using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using AOCShared;

namespace Day20
{
    public enum Pulse
    {
        High,
        Low
    }

    public class SendPulse
    {
        public string Source;
        public string Destination;
        public Pulse Pulse;

        public SendPulse(string source, string dest, Pulse p)
        {
            Source = source;
            Destination = dest;
            Pulse = p;
        }
    }

    public class PulseQueue : Queue<SendPulse>
    {
        public void Send(string source, string dest, Pulse p)
        {
            this.Enqueue(new SendPulse(source, dest, p));
        }

        public SendPulse Get()
        {
            if (this.Count == 0)
            {
                return null;
            }

            return this.Dequeue();
        }
    }

    public class FlipFlop
    {
        public string Name;
        public bool On = false;
        public List<string> Destination = new List<string>();

        public bool IsConjunction = false;
        public Dictionary<string, Pulse> FlipFlopCache = new Dictionary<string, Pulse>();

        public FlipFlop(string line, bool isConjunction)
        {
            int index = line.IndexOf(' ');
            Name = line.Substring(0, index);

            string flips = line.Substring(index + 4);

            List<string> flipflops = StringLibraries.GetListOfStrings(flips, ',');
            foreach (string f in flipflops)
            {
                Destination.Add(f);
            }

            IsConjunction = isConjunction;
        }

        public void AddInput(FlipFlop f)
        {
            FlipFlopCache.Add(f.Name, Pulse.Low);
        }

        public void Set(string source, Pulse p, PulseQueue queue)
        {
            if (!IsConjunction)
            {
                bool send = false;
                Pulse newp = new Pulse();
                switch (p)
                {
                    case Pulse.High:
                        break;
                    case Pulse.Low:
                        if (!On)
                        {
                            On = true;
                            newp = Pulse.High;
                            send = true;
                        }
                        else
                        {
                            On = false;
                            newp = Pulse.Low;
                            send = true;
                        }
                        break;
                }

                if (send)
                {
                    foreach (string val in Destination)
                    {
                        queue.Send(Name, val, newp);
                    }
                }
            }
            else
            {
                bool send = false;
                Pulse newPulse = Pulse.Low;

                FlipFlopCache[source] = p;
                if (FlipFlopCache.All(x => x.Value == Pulse.High))
                {
                    newPulse = Pulse.Low;
                    send = true;
                }
                else 
                {
                    newPulse = Pulse.High;
                    send = true;
                }

                if (send)
                {
                    foreach (string val in Destination)
                    {
                        queue.Send(Name, val, newPulse);
                    }
                }
            }

        }
    }

    internal class Broadcaster
    {
        private bool m_part2 = false;
        PulseQueue queue = new PulseQueue();
        Dictionary<string, FlipFlop> flipFlopList = new Dictionary<string, FlipFlop>();
        List<string> broadcasterModules = new List<string>();

        public Broadcaster(string fileName, bool part2)
        {
            m_part2 = part2;

            StreamReader rdr = new StreamReader(fileName);
            string line = string.Empty;

            while ((line = rdr.ReadLine()) != null)
            {
                if (!string.IsNullOrEmpty(line))
                {

                    if (line.StartsWith("broadcaster"))
                    {
                        broadcasterModules = StringLibraries.GetListOfStrings(line.Substring(15), ',');
                    }
                    else if (line.StartsWith("%"))
                    {
                        FlipFlop f = new FlipFlop(line.Substring(1), false);
                        flipFlopList.Add(f.Name, f);
                    }
                    else if (line.StartsWith("&"))
                    {
                        FlipFlop f = new FlipFlop(line.Substring(1), true);
                        flipFlopList.Add(f.Name, f);
                    }
                }
            }

            foreach (var f in flipFlopList)
            {
                foreach (string dest in f.Value.Destination)
                {
                    if (flipFlopList.ContainsKey(dest))
                    {
                        if (flipFlopList[dest].IsConjunction)
                        {
                            flipFlopList[dest].AddInput(f.Value);
                        }
                    }
                }
            }
        }

        public long Calculate1()
        {
            long total = 0;

            long highTotal = 0;
            long lowTotal = 0;

            for (int i = 0; i < 1000; i++)
            {
                // Inject pulses
                foreach (string dest in broadcasterModules)
                {
                    queue.Send("broadcaster", dest, Pulse.Low);
                }
                lowTotal++;

                while (queue.Count != 0)
                {
                    SendPulse pulse = queue.Get();

                    if (flipFlopList.ContainsKey(pulse.Destination))
                    {
                        FlipFlop f = flipFlopList[pulse.Destination];
                        if (f != null)
                        {
                            f.Set(pulse.Source, pulse.Pulse, queue);
                        }
                    }

                    if (pulse.Pulse == Pulse.High)
                    {
                        highTotal++;
                    }
                    else
                    {
                        lowTotal++;
                    }
                }
            }

            total = highTotal * lowTotal;
            return total;
        }

        public long Calculate2()
        {
            long total = 0;

            Dictionary<string, long> High = new Dictionary<string, long>();

            foreach (string val in flipFlopList["xm"].FlipFlopCache.Keys)
            {
                High.Add(val, 0);
            }

            long highTotal = 0;
            long lowTotal = 0;

            long buttonCount = 0;
            bool finished = false;
            while (!finished)
            {

                buttonCount++;
                // Inject pulses
                foreach (string dest in broadcasterModules)
                {
                    queue.Send("broadcaster", dest, Pulse.Low);
                }
                lowTotal++;

                while (queue.Count != 0)
                {
                    SendPulse pulse = queue.Get();

                    if (flipFlopList.ContainsKey(pulse.Destination))
                    {
                        FlipFlop f = flipFlopList[pulse.Destination];
                        if (f != null)
                        {
                            f.Set(pulse.Source, pulse.Pulse, queue);
                        }
                    }

                    if (pulse.Pulse == Pulse.High)
                    {
                        highTotal++;
                    }
                    else
                    {
                        lowTotal++;
                    }

                    if (High.ContainsKey(pulse.Source) && (pulse.Pulse == Pulse.High))
                    {
                        if (High[pulse.Source] == 0)
                        {
                            High[pulse.Source] = buttonCount;
                        }
                    }

                    if (High.Values.All(x => x > 0))
                    {
                        finished = true;
                        break;
                    }
                }
            }

            total = MathLibraries.LowestCommonMultiple(High.Values.ToList());
            
            return total;
        }


        public long Calculate()
        {
            long total = 0;

            if (!m_part2)
            {
                total = Calculate1();
            }
            else
            {
                total = Calculate2();
            }

            return total;
        }
    }


    internal class Day20
    {
        Broadcaster inputObjects = null;

        internal void ProcessInput(string fileName, bool part2)
        {
            inputObjects = new Broadcaster(fileName, part2);

        }

        internal long Execute1(string fileName)
        {
            long total = 0;

            total += inputObjects.Calculate();

            return total;
        }


        internal long Execute2(string fileName)
        {
            long total = 0;

            total += inputObjects.Calculate();

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
